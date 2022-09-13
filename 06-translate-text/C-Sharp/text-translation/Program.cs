using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace translate_text
{
    class Program
    {
        private static string translatorEndpoint = "https://api.cognitive.microsofttranslator.com";
        private static string cogSvcKey;
        private static string cogSvcRegion;

        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                cogSvcKey = configuration["CognitiveServiceKey"];
                cogSvcRegion = configuration["CognitiveServiceRegion"];

                // Set console encoding to unicode
                Console.InputEncoding = Encoding.Unicode;
                Console.OutputEncoding = Encoding.Unicode;

                // Analyze each text file in the reviews folder
                var folderPath = Path.GetFullPath("./reviews");  
                DirectoryInfo folder = new DirectoryInfo(folderPath);
                foreach (var file in folder.GetFiles("*.txt"))
                {
                    // Read the file contents
                    Console.WriteLine("\n-------------\n" + file.Name);
                    StreamReader sr = file.OpenText();
                    var text = sr.ReadToEnd();
                    sr.Close();
                    Console.WriteLine("\n" + text);

                    // Detect the language
                    string language = await GetLanguage(text);
                    Console.WriteLine("Language: " + language);

                    // Translate if not already English
                    if (language != "en")
                    {
                        string translatedText = await Translate(text,language);
                        Console.WriteLine("\nTranslation:\n" + translatedText);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task<string> GetLanguage(string text)
        {
            // Default language is English
            string language = "en";

            // Use the Translator detect function
            // Use the Translator detect function
            object[] body = new object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    // Build the request
                    string path = "/detect?api-version=3.0";
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(translatorEndpoint + path);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", cogSvcKey);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", cogSvcRegion);

                    // Send the request and get response
                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                    // Read response as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse JSON array and get language
                    JArray jsonResponse = JArray.Parse(responseContent);
                    language = (string)jsonResponse[0]["language"]; 
                }
            }

            // return the language
            return language;
        }

        static async Task<string> Translate(string text, string sourceLanguage)
        {
            string translation = "";

            // Use the Translator translate function
            // Use the Translator translate function
            object[] body = new object[] { new { Text = text } };
            var requestBody = JsonConvert.SerializeObject(body);
            using (var client = new HttpClient())
            {
                using (var request = new HttpRequestMessage())
                {
                    // Build the request
                    string path = "/translate?api-version=3.0&from=" + sourceLanguage + "&to=en" ;
                    request.Method = HttpMethod.Post;
                    request.RequestUri = new Uri(translatorEndpoint + path);
                    request.Content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                    request.Headers.Add("Ocp-Apim-Subscription-Key", cogSvcKey);
                    request.Headers.Add("Ocp-Apim-Subscription-Region", cogSvcRegion);

                    // Send the request and get response
                    HttpResponseMessage response = await client.SendAsync(request).ConfigureAwait(false);
                    // Read response as a string
                    string responseContent = await response.Content.ReadAsStringAsync();

                    // Parse JSON array and get translation
                    JArray jsonResponse = JArray.Parse(responseContent);
                    translation = (string)jsonResponse[0]["translations"][0]["text"];  
                }
            }

            // Return the translation
            return translation;

        }
    }
}

