using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;

// dotnet add package Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction --version 2.0.0

namespace test_classifier
{
    class Program
    {
        static CustomVisionPredictionClient prediction_client;

        static void Main(string[] args)
        {
            try
            {
                // Get Configuration Settings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string prediction_endpoint = configuration["PredictionEndpoint"];
                string prediction_key = configuration["PredictionKey"];
                Guid project_id = Guid.Parse(configuration["ProjectID"]);
                string model_name = configuration["ModelName"];

                // Authenticate a client for the prediction API
                prediction_client = new CustomVisionPredictionClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction.ApiKeyServiceClientCredentials(prediction_key))
                {
                    Endpoint = prediction_endpoint
                };

                // Classify test images
                String[] images = Directory.GetFiles("test-images");
                foreach(var image in images)
                {
                    Console.Write(image + ": ");
                    MemoryStream image_data = new MemoryStream(File.ReadAllBytes(image));
                    var result = prediction_client.ClassifyImage(project_id, model_name, image_data);

                    // Loop over each label prediction and print any with probability > 50%
                    foreach (var prediction in result.Predictions)
                    {
                        if (prediction.Probability > 0.5)
                        {
                            Console.WriteLine($"{prediction.TagName} ({prediction.Probability:P1})");
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
