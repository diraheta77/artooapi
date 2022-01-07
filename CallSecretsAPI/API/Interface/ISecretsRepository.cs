using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using API.Models;

namespace API.Interface
{
    public interface ISecretsRepository
    {
        string GetSecret();

        string GetSecret(string SubID);

        SecretResponse GetSecret(SecretRequest secretRequest, string subscriptionId, string cacheKey = "");
    }
}
