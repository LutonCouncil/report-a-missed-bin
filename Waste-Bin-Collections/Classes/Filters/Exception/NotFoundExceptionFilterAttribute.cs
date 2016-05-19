using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Filters;

namespace Waste_Bin_Collections.Classes.Filters.Exception
{
    /// <summary>
    /// Exception filter attribute to handle invalid method calls where arguments are null,
    /// out of range or there is a null reference
    /// </summary>
    public class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            actionExecutedContext.Response = HandleException(actionExecutedContext);
        }

        public override Task OnExceptionAsync(HttpActionExecutedContext actionExecutedContext, System.Threading.CancellationToken cancellationToken)
        {
            actionExecutedContext.Response = HandleException(actionExecutedContext);
            return base.OnExceptionAsync(actionExecutedContext, cancellationToken);
        }

        private HttpResponseMessage HandleException(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Exception is ArgumentNullException ||
                actionExecutedContext.Exception is ArgumentOutOfRangeException ||
                actionExecutedContext.Exception is NullReferenceException)
            {
                var message = actionExecutedContext.Exception.Message;
                var resp = actionExecutedContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.NotFound, message);

                return resp;
            }

            return actionExecutedContext.Response;
        }
    }
}