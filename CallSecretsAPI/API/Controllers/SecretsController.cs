using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Repository;
using API.Interface;
using API.Models;
using API.Security;

namespace API.Controllers
{
    [Route("securitydomain/api/v1/secrets")]
    public class SecretsController : ApiController
    {
        public ISecretsRepository _repository;

        public SecretsController(ISecretsRepository repository)
        {
            _repository = repository;
        }

        [AuthorizationFilter]
        [HttpPost]        
        public IHttpActionResult Post([FromBody] SecretRequest secretRequest)
        {
            SecretResponse secret;

            try
            {
               string subscriptionId = Request.Headers.GetValues("X-Subscription-Id").First();
               if(Request.Headers.Contains("cacheKey"))
               {
                    string cacheKey = Request.Headers.GetValues("cacheKey").First();
                    secret = _repository.GetSecret(secretRequest, subscriptionId, cacheKey);
               }
               else
               {
                    secret = _repository.GetSecret(secretRequest, subscriptionId);
               }    
               
            }
            catch (Exception ex)
            {
                return InternalServerError();
            }
            return Ok(secret);
        }


        [HttpGet]
        public IHttpActionResult Get()
        {
            return Ok(_repository.GetSecret());
        }

    }
}
