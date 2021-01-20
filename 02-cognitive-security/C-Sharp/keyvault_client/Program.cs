using System;
using Azure;
using Microsoft.Extensions.Configuration;
using Azure.AI.TextAnalytics;
using static System.Environment;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace keyvault_client
{
    class Program
    {

        private static string cogSvcEndpoint;
        private static string cogSvcKey;
        static void Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                cogSvcEndpoint = configuration["CognitiveServicesEndpoint"];
                string keyVaultName = configuration["KeyVault"];
                string appTenant = configuration["TenantId"];
                string appId = configuration["AppId"];
                string appPassword = configuration["AppPassword"];

                // Get cognitive services key from keyvault using the service principal credentials
                var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
                ClientSecretCredential credential = new ClientSecretCredential(appTenant, appId, appPassword);
                var keyVaultClient = new SecretClient(keyVaultUri, credential);
                KeyVaultSecret secretKey = keyVaultClient.GetSecret("Cognitive-Services-Key");
                cogSvcKey = secretKey.Value;

                // Get user input (until they enter "quit")
                string userText = "";
                while (userText.ToLower() != "quit")
                {
                    Console.WriteLine("\nEnter some text ('quit' to stop)");
                    userText = Console.ReadLine();
                    if (userText.ToLower() != "quit")
                    {
                        // Call function to detect language
                        string language = GetLanguage(userText);
                        Console.WriteLine("Language: " + language);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static string GetLanguage(string text)
        {

            // Create client using endpoint and key
            AzureKeyCredential credentials = new AzureKeyCredential(cogSvcKey);
            Uri endpoint = new Uri(cogSvcEndpoint);
            var client = new TextAnalyticsClient(endpoint, credentials);

            // Call the service to get the detected language
            DetectedLanguage detectedLanguage = client.DetectLanguage(text);
            return(detectedLanguage.Name);

        }
    }
}
