using System;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace rest_client
{
    class Program
    {
        private static string cogSvcEndpoint;
        private static string cogSvcKey;
        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                cogSvcEndpoint = configuration["CognitiveServicesEndpoint"];
                cogSvcKey = configuration["CognitiveServiceKey"];

                // Set console encoding to unicode
                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;

                // Get user input (until they enter "quit")
                string userText = "";
                while (userText.ToLower() != "quit")
                {
                    Console.WriteLine("Enter some text ('quit' to stop)");
                    userText = Console.ReadLine();
                    if (userText.ToLower() != "quit")
                    {
                        // Call function to detect language
                        await GetLanguage(userText);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static async Task GetLanguage(string text)
        {
            // Construct the JSON request body
            try
            {
                JObject jsonBody = new JObject(
                    // Create a collection of documents (we'll only use one, but you could have more)
                    new JProperty("documents",
                    new JArray(
                        new JObject(
                            // Each document needs a unique ID and some text
                            new JProperty("id", 1),
                            new JProperty("text", text)
                    ))));
                
                // Encode as UTF-8
                UTF8Encoding utf8 = new UTF8Encoding(true, true);
                byte[] encodedBytes = utf8.GetBytes(jsonBody.ToString());
                
                // Let's take a look at the JSON we'll send to the service
                Console.WriteLine(utf8.GetString(encodedBytes, 0, encodedBytes.Length));

                // Make an HTTP request to the REST interface
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);

                // Add the authentication key to the header
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", cogSvcKey);

                // Use the endpoint to access the Text Analytics language API
                var uri = cogSvcEndpoint + "text/analytics/v3.0/languages?" + queryString;

                // Send the request and get the response
                HttpResponseMessage response;
                using (var content = new ByteArrayContent(encodedBytes))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PostAsync(uri, content);
                }

                // If the call was successful, get the response
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    // Display the JSON response in full (just so we can see it)
                    string responseContent = await response.Content.ReadAsStringAsync();
                    JObject results = JObject.Parse(responseContent);
                    Console.WriteLine(results.ToString());

                    // Extract the detected language name for each document
                    foreach (JObject document in results["documents"])
                    {
                        Console.WriteLine("\nLanguage: " + (string)document["detectedLanguage"]["name"]);
                    }
                }
                else
                {
                    // Something went wrong, write the whole response
                    Console.WriteLine(response.ToString());
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
