using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Filters;

namespace Waste_Bin_Collections.Classes.Filters.Authorization
{
    /// <summary>
    /// An authorization filter to check the contents of the "XSRF-TOKEN" cookie and the
    /// "X-XSRF-TOKEN" Http request header. If they match then the request is not due to
    /// a XSRF attack.
    /// 
    /// This works because the browser will honour the same-origin policy when dealing with
    /// the token in the cookie. A malicious script cannot read or change the value of the
    /// cookie from another web page.
    /// </summary>
    public class XsrfAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = ValidateXsrfToken(actionContext);
            base.OnAuthorization(actionContext);
        }

        public override System.Threading.Tasks.Task OnAuthorizationAsync(System.Web.Http.Controllers.HttpActionContext actionContext, System.Threading.CancellationToken cancellationToken)
        {
            actionContext.Response = ValidateXsrfToken(actionContext);
            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }

        /// <summary>
        /// Returns the HTTP message to send back to the client. If the cookie and HTTP header XSRF tokens match
        /// then this function will return null, i.e. we don't need to send back a special response, just carry on
        /// processing the request. If the token do not match then the appropriate response message is constructed
        /// and the calling code should stop processing the request and return the response to the user.
        /// </summary>
        /// <param name="actionContext">The <code>HttpActionContext</code> object representing the action being processed.</param>
        /// <returns><code>null</code> if the tokens match, a <code>HttpResponseMessage</code> otherwise</returns>
        private HttpResponseMessage ValidateXsrfToken(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Get Http header
            string xsrfHttpHeader;
            try
            {
                xsrfHttpHeader = actionContext.Request.Headers.GetValues("X-XSRF-TOKEN").First();
            }
            catch (InvalidOperationException e)
            {
                return actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Missing XSRF token");
            }
            catch (ArgumentNullException e)
            {
                return actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Missing XSRF token");
            }

            //Get cookie
            string cookie = actionContext.Request.Headers.GetCookies("XSRF-TOKEN").FirstOrDefault()["XSRF-TOKEN"].Value;
            if (cookie == null)
            {
                return actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.BadRequest, "Missing XSRF cookie");
            }

            //Compares the value in the cookie with the value in the HTTP header
            if (!cookie.Equals(xsrfHttpHeader))
            {
                return actionContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.Forbidden, "XSRF tokens do not match");
            }
            else
            {
                return null;
            }
        
        }
    }
}