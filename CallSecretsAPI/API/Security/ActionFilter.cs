using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace API.Security
{
    public class ActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse
                    (
                    HttpStatusCode.NotFound,
                    new { message = "" },
                    actionContext.ControllerContext.Configuration.Formatters.JsonFormatter
                    );
            }
            else 
            {
                var headers = actionContext.Request.Headers;
            }
            

        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {

        }
    }
}