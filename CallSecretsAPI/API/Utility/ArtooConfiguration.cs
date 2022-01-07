using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using API.Models;

namespace API.Utility
{
    public class ArtooConfiguration
    {
        public static string apiGatewayJanus = ConfigurationManager.AppSettings["endpointGateway"].ToString();
        public static int connectionTimeout = Convert.ToInt32(ConfigurationManager.AppSettings["timeout"].ToString());
        public static int retryCount = Convert.ToInt32(ConfigurationManager.AppSettings["retryCount"].ToString());
        public static int sleepInSeconds = Convert.ToInt32(ConfigurationManager.AppSettings["sleepInSeconds"].ToString());
        public static int cacheTime = Convert.ToInt32(ConfigurationManager.AppSettings["timeCacheValidInMinutes"].ToString());

        public static string GetEndpointGateway()
        {
            try
            {
                string endpoint = ConfigurationManager.AppSettings["endpointGateway"].ToString();
                if (!string.IsNullOrEmpty(endpoint))
                {
                    return endpoint;
                }

                return "No gateway";                
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public static string ConfigureSecretName(SecretRequest secret, string subscriptionId)
        {
            try
            {
                if(!string.IsNullOrEmpty(secret.Code) && !string.IsNullOrEmpty(subscriptionId))
                {
                    return $"{secret.Code}-{subscriptionId}";
                }

                return string.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static List<string> GetHeadersForValidation()
        {
            try
            {
                return ConfigurationManager.AppSettings["headersForValidation"].ToString().Split(',').ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}