using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Azure.KeyVault;
using System;
using Common.Logging;

namespace Divergent.Sales.Config
{
    public class ResourceConfig
    {
        private static readonly ILog Log = LogManager.GetLogger<EndpointConfig>();
        static ResourceConfig()
        {
            ClientSecret = Environment.GetEnvironmentVariable("azure:Microservices-ClientSecret");
            AppId = Environment.GetEnvironmentVariable("azure:Microservices-AppId");
            Log.Debug($"AppId : {AppId}");
            Log.Debug($"ClientSecret : {ClientSecret}");
        }

        private static string ClientSecret { get; }
        public static string AppId { get; }



        private static async Task<string> GetAccessToken(string authority, string _, string __)
        {
            var context = new AuthenticationContext(authority);
            ClientCredential clientCredential = new ClientCredential(AppId, ClientSecret);
            var tokenResponse = await context.AcquireTokenAsync("https://vault.azure.net", clientCredential);
            return tokenResponse.AccessToken;
        }

        public static async Task<string> GetSecret(string secretIdentifier)
        {
            var a = new KeyVaultClient.AuthenticationCallback(GetAccessToken);
            var kv = new KeyVaultClient(a);
            var sec = await kv.GetSecretAsync(secretIdentifier);
            return sec.Value;
        }
    }


}
