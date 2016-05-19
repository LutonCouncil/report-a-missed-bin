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
    /// Exception filter attribute to handle permission related exceptions
    /// </summary>
    public class ForbiddenExceptionFilterAttribute : ExceptionFilterAttribute
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
            //Chose to throw InvalidOperationException for permission issues because there isn't
            //a builtin exception for permission denied
            if (actionExecutedContext.Exception is InvalidOperationException)
            {
                var message = actionExecutedContext.Exception.Message;
                var resp = actionExecutedContext.Request.CreateErrorResponse(System.Net.HttpStatusCode.Forbidden, message);

                return resp;
            }

            return actionExecutedContext.Response;
        }
    }
}