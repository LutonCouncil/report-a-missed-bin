using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Waste_Bin_Collections.Classes;
using Waste_Bin_Collections.Classes.Filters.Exception;
using Waste_Bin_Collections.Models;
using Waste_Bin_Collections.Models.DTOs;

namespace Waste_Bin_Collections.Controllers
{
    /// <summary>
    /// Lookup LLPG addresses by road name or full post code
    /// </summary>
    [NotFoundExceptionFilter]
    [ForbiddenExceptionFilter]
    public class StreetController : ApiController
    {
        private Models.LLPGEntities db;
        private string regexPostCode = "^[a-z]{1,2}[0-9]";
        
        //Full post code without a space
        private string regexFullPostCode = "^[a-z]{2}[0-9]{2}[a-z]{2}$";

        public StreetController()
        {
            db = new Models.LLPGEntities();
        }

        // GET api/search/street/lu1 2rd
        // GET api/search/street/upper georg
        /// <summary>
        /// Returns a collection of LLPG addresses with the given road name or full post code
        /// </summary>
        /// <param name="id">The road name (or first part) or full post code</param>
        /// <returns>A collection of LLPG addresses</returns>
        public IEnumerable<LLPGAddressDTO> Get(string id)
        {
            string searchTerm = id.Replace(" ", "").ToLower();

            //There's a lot of addresses in the Gazetteer... do you really want to pull
            //back everything with a post code starting with "lu"?..... No, you don't!
            if (searchTerm.Length < 3)
            {
                throw new NullReferenceException("Please provide 3 or more characters.");
            }
            else
            {
                //Check to see if this looks like a post code
                if (LooksLikePostCode(searchTerm) && !IsFullPostCode(searchTerm))
                {
                    //Looks like it's a post code but not a full one
                    throw new InvalidOperationException("Please provide a full post code");
                }
            }
            
            //If you got here, we can do a search
            var addresses = db.vwFullExtracts
                .Where( a => 
                    ( a.LPI_LOGICAL_STATUS.Trim().Equals("1") &&
                         (
                            ( a.Primary.ToLower().Equals("r") && 
                                ( a.Secondary.ToLower().Equals("d") || 
                                    a.Secondary.ToLower().Equals("h") || 
                                    a.Secondary.ToLower().Equals("i")
                                )
                            ) || 
                            a.Primary.ToLower().Equals("x")
                         )                
                    ) && 
                    ( a.STREET_NAME.Replace(" ", "").ToLower().StartsWith(searchTerm) || 
                        a.POSTCODE.Replace(" ", "").ToLower().StartsWith(searchTerm)
                    )
                )
            .OrderBy(o => o.STREET_NAME)
            .ThenBy(o => o.PAO_END_SFX)
            .ThenBy(o => o.PAO_END_NO)
            .ThenBy(o => o.PAO_START_SFX)
            .ThenBy(o => o.PAO_START_NO)
            .ThenBy(o => o.PAO_DESC)
            .ThenBy(o => o.SAO_END_SFX)
            .ThenBy(o => o.SAO_END_NO)
            .ThenBy(o => o.SAO_START_SFX)
            .ThenBy(o => o.SAO_START_NO)
            .ThenBy(o => o.SAO_DESC);
            
            if (addresses == null || addresses.Count() < 1)
            {
                throw new NullReferenceException(String.Format("No addresses matching '{0}' could be found.", id));
            }

            List<LLPGAddressDTO> addressList = new List<LLPGAddressDTO>();

            foreach (var addr in addresses)
            {
                addressList.Add(new LLPGAddressDTO(addr));
            }

            return addressList;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Checks whether the parameter looks like a post code or not
        /// </summary>
        /// <param name="pc">The <code>string</code> to check</param>
        /// <returns><code>True</code> if the <code>string</code> looks like a post, <code>False</code> otherwise.</returns>
        private bool LooksLikePostCode(string pc)
        {
            return Regex.IsMatch(pc, regexPostCode);
        }

        /// <summary>
        /// Checks whether the parameter is a full Luton post code or not
        /// </summary>
        /// <param name="pc">The <code>string</code> to check</param>
        /// <returns><code>True</code> if the <code>string</code> is a full Luton post, <code>False</code> otherwise.</returns>
        private bool IsFullPostCode(string pc)
        {
            return Regex.IsMatch(pc, regexFullPostCode);
        }
    }
}
