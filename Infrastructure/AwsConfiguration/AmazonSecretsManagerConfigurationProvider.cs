using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon;
using Amazon.Runtime;

namespace Infrastructure.AwsConfiguration
{
    public class AmazonSecretsManagerConfigurationProvider: ConfigurationProvider
    {
        private readonly string _region;
        private readonly string _secretName;

        public AmazonSecretsManagerConfigurationProvider(string region, string secretName)
        {
            _region = region;
            _secretName = secretName;
        }

        public override void Load()
        {
            var secret = GetSecret();

            Data = JsonSerializer.Deserialize<Dictionary<string, string>>(secret);
        }

        private string GetSecret()
        {
            var request = new GetSecretValueRequest
            {
                SecretId = _secretName,
                VersionStage = "AWSCURRENT" // VersionStage defaults to AWSCURRENT if unspecified.
            };

            using (var client = new AmazonSecretsManagerClient("AKIA27DDRANTZ6ESXHMX", "hplpTDC4SfMx7ExbPcBWQLt1hoWmQMqKWdUfx4wn", RegionEndpoint.GetBySystemName(_region)))
            {
                var response = client.GetSecretValueAsync(request).Result;

                string secretString;


                if (response.SecretString != null)
                {
                    secretString = response.SecretString;
                }
                else
                {
                    var memoryStream = response.SecretBinary;
                    var reader = new StreamReader(memoryStream);
                    secretString = Encoding.UTF8.GetString(Convert.FromBase64String(reader.ReadToEnd()));
                }

                return secretString;
            }
        }
    }

}
