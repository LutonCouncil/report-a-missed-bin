using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Waste_Bin_Collections.Models.DTOs
{
    /// <summary>
    /// LLPG Address Data Transformation Object
    /// </summary>
    public class LLPGAddressDTO
    {
        public string Id { get; set; }

        private decimal? paoStartNo, paoEndNo, saoStartNo, saoEndNo;

        private string paoDesc, paoStartSfx, paoEndSfx, saoDesc, saoStartSfx, saoEndSfx, streetName, easting, northing;

        public string Easting { get { return easting; } }
        public string Northing { get { return northing; } }

        public string StreetName { 
            get {
                if (String.IsNullOrEmpty(streetName)) {
                    throw new ArgumentNullException("Street name cannot be null");
                }

                StringBuilder sb = new StringBuilder();

                if (!String.IsNullOrEmpty(saoDesc)) {
                    sb.Append(saoDesc);
                    sb.Append(" ");
                }

                string saoStart = String.Concat(saoStartNo, saoStartSfx);
                string saoEnd = String.Concat(saoEndNo, saoEndSfx);

                if (saoStart.Length > 0 && saoEnd.Length > 0)
                {
                    sb.Append(saoStart);
                    sb.Append(" - ");
                    sb.Append(saoEnd);
                }
                else if (saoStart.Length > 0 && saoEnd.Length == 0)
                {
                    sb.Append(saoStart);
                    sb.Append(" ");
                }
                else if (saoStart.Length == 0 && saoEnd.Length > 0)
                {
                    sb.Append(saoEnd);
                    sb.Append(" ");
                }

                if (!String.IsNullOrEmpty(paoDesc))
                {
                    sb.Append(paoDesc);
                    sb.Append(" ");
                }

                string paoStart = String.Concat(paoStartNo, paoStartSfx);
                string paoEnd = String.Concat(paoEndNo, paoEndSfx);

                if (paoStart.Length > 0 && paoEnd.Length > 0)
                {
                    sb.Append(paoStart);
                    sb.Append(" - ");
                    sb.Append(paoEnd);
                }
                else if (paoStart.Length > 0 && paoEnd.Length == 0)
                {
                    sb.Append(paoStart);
                    sb.Append(" ");
                }
                else if (paoStart.Length == 0 && paoEnd.Length > 0)
                {
                    sb.Append(paoEnd);
                    sb.Append(" ");
                }

                sb.Append(streetName);

                return sb.ToString();
            }
        }

        public LLPGAddressDTO() { }

        public LLPGAddressDTO(string id, decimal paoStartNo, string paoStartSfx, decimal paoEndNo, string paoEndSfx, string paoDesc, decimal saoStartNo, string saoStartSfx, decimal saoEndNo, string saoEndSfx, string saoDesc, string streetName, string easting, string northing )
        {
            this.Id = id;
            
            this.paoStartNo = paoStartNo;
            this.paoStartSfx = paoStartSfx;
            this.paoEndNo = paoEndNo;
            this.paoEndSfx = paoEndSfx;
            this.paoDesc = paoDesc;

            this.saoStartNo = saoStartNo;
            this.saoStartSfx = saoStartSfx;
            this.saoEndNo = saoEndNo;
            this.saoEndSfx = saoEndSfx;
            this.saoDesc = saoDesc;

            this.streetName = streetName;

            this.easting = easting;
            this.northing = northing;
        }

        public LLPGAddressDTO(vwFullExtract address)
        {
            this.Id = address.UPRN;

            this.paoStartNo = address.PAO_START_NO;
            this.paoStartSfx = address.PAO_START_SFX;
            this.paoEndNo = address.PAO_END_NO;
            this.paoEndSfx = address.PAO_END_SFX;
            this.paoDesc = address.PAO_DESC;

            this.saoStartNo = address.SAO_START_NO;
            this.saoStartSfx = address.SAO_START_SFX;
            this.saoEndNo = address.SAO_END_NO;
            this.saoEndSfx = address.SAO_END_SFX;
            this.saoDesc = address.SAO_DESC;

            this.streetName = address.STREET_NAME;

            this.easting = address.MAP_EAST;
            this.northing = address.MAP_NORTH;
        }
    }
}