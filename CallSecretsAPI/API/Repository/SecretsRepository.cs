using System;
using System.Configuration;
using API.Interface;
using API.Models;
using Pfcti.Toolbox.Artoo.Consumers.Net.Impl;
using API.Utility;
using System.Runtime.Caching;

namespace API.Repository
{
    public class SecretsRepository : ISecretsRepository
    {
        private readonly string CacheKey = ConfigurationManager.AppSettings["CacheKey"].ToString();
        private readonly string timeCache = ConfigurationManager.AppSettings["timeCacheValidInMinutes"].ToString();

        public string GetSecret()
        {
            try
            {
                using (var secretService = new SecretService(
                    new ArtooConsumerImpl(
                    apiGatewayEndpoint: "http://10.31.21.10:8123/",
                    subscriptionId: "AZ10100010001",
                    connectionTimeoutInSeconds: 2,
                    retryCount: 3,
                    sleepDurationInSeconds: 2)))
                {
                    //ArtooConsumerImpl artoo = new ArtooConsumerImpl();
                    //string resp = artoo.GetSecret("ENIGMA-AESGCM256-KEY-PF10100020003");
                    var secret = secretService.GetAesGcm256Key();
                    return secret;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string GetSecret(string pSubscriptionId)
        {
            string vSecretValue = string.Empty;
            string pSecretName = 
                $"{ConfigurationManager.AppSettings["secretName"].ToString()}-{pSubscriptionId}";
            string subscriptionId = ConfigurationManager.AppSettings["subscriptionId"].ToString();
            string endpoint = ConfigurationManager.AppSettings["endpointGateway"].ToString();
            try
            {
                using (var artooConsumer = new ArtooConsumerImpl(
                    apiGatewayEndpoint: endpoint,
                    subscriptionId: subscriptionId,
                    connectionTimeoutInSeconds: 2,
                    retryCount: 3,
                    sleepDurationInSeconds: 2))
                {
                    vSecretValue = artooConsumer.GetSecret(pSecretName);
                }
            }
            catch (Exception ex)
            {
                vSecretValue = ex.Message;                
            }
            return vSecretValue;
        }

        public SecretResponse GetSecret(SecretRequest secretRequest, string subscriptionId, string cacheKey = "")
        {
            string vSecretValue = string.Empty;
            string vSecretName = string.Empty;

            try
            {
                if (cacheKeyIsExists(cacheKey))
                {
                    return new SecretResponse
                    {
                        Value = getCacheValue(cacheKey)
                    };
                }

                //si no existe el valor almacenado en la cache..
                vSecretName = ArtooConfiguration.ConfigureSecretName(secretRequest, subscriptionId);

                using (var artooConsumer = new ArtooConsumerImpl(
                    apiGatewayEndpoint: ArtooConfiguration.apiGatewayJanus,
                    subscriptionId: subscriptionId,
                    connectionTimeoutInSeconds: ArtooConfiguration.connectionTimeout,
                    retryCount: ArtooConfiguration.retryCount,
                    sleepDurationInSeconds: ArtooConfiguration.sleepInSeconds))
                {
                    vSecretValue = artooConsumer.GetSecret(vSecretName);
                }

                //agrego el valor del secreto a la cache..
                if (!string.IsNullOrEmpty(cacheKey))
                    AddCacheValue(cacheKey, vSecretValue);

                return new SecretResponse
                {
                    Value = vSecretValue
                };

            }
            catch (Exception ex)
            {
                return new SecretResponse
                {
                    Value = ex.Message
                };
            }

        }

        private bool cacheKeyIsExists(string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;

            if (string.IsNullOrEmpty(cacheKey))
                return false;

            return cache.Contains(cacheKey);
        }


        private string getCacheValue(string cacheKey)
        {
            ObjectCache cache = MemoryCache.Default;
            return (string)cache.Get(cacheKey);
        }

        private void AddCacheValue(string cacheKey, object value)
        {
            ObjectCache cache = MemoryCache.Default;
            // inicializo la cache    
            CacheItemPolicy cacheItemPolicy = new CacheItemPolicy();
            // se establece el tiempo de memoria cache
            cacheItemPolicy.AbsoluteExpiration = DateTime.Now.AddMinutes(ArtooConfiguration.cacheTime);

            // agrego la cache con su valor
            cache.Add(cacheKey, value, cacheItemPolicy);

        }

    }
}