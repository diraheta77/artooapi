using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using API.Utility;

namespace API.Security
{
    public class AuthorizationFilter : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (actionContext == null)
            {
                HandleUnauthorizedRequest(actionContext);
            }

            return IsValidHeaders(actionContext);
        }

        private bool IsValidHeaders(HttpActionContext actionContext)
        {
            var headers = actionContext.Request.Headers;
            List<string> headersValidations = ArtooConfiguration.GetHeadersForValidation();

            foreach(string head in headersValidations)
            {
                if(!headers.Contains(head.Trim()))
                {
                    return false;
                }
            }
          
            //if (!headers.Contains("X-Origin-IP-Address"))
            //{
            //    return false;
            //}
            //if (!headers.Contains("X-Subscription-Id"))
            //{
            //    return false;
            //}
            //if (!headers.Contains("X-Origin-Country"))
            //{
            //    return false;
            //}

            return true;
        }
    }
}