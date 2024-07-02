﻿using Infrastructure.AwsConfiguration;

namespace API.Extensions
{
    public static class AwsExtensions
    {
        public static void AddAmazonSecretsManager(this IConfigurationBuilder configurationBuilder,
                        string region,
                        string secretName)
        {
            var configurationSource = new AmazonSecretsManagerConfigurationSource(region, secretName);
            configurationBuilder.Add(configurationSource);
        }
    }
}
