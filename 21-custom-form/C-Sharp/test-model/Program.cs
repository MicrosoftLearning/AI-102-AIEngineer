using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

// import namespaces
using Azure;
using Azure.AI.FormRecognizer;  
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;

namespace test_model
{
    class Program
    {     
        static async Task Main(string[] args)
        {     
            try
            {   
                // Get configuration settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string formEndpoint = configuration["FormEndpoint"];
                string formKey = configuration["FormKey"];
                string modelId = configuration["ModelId"];
                
                // Authenticate Form Recognizer Client 
                var credential = new AzureKeyCredential(formKey);
                var recognizerClient = new FormRecognizerClient(new Uri(formEndpoint), credential);

                // Get form url for testing   
                string image_file = "test1.jpg";
                using (var image_data = File.OpenRead(image_file))
                {
                    // Use trained model with new form 
                    RecognizedFormCollection forms = await recognizerClient
                    .StartRecognizeCustomForms(modelId, image_data)
                    .WaitForCompletionAsync();
                    
                    foreach (RecognizedForm form in forms)
                    {
                        Console.WriteLine($"Form of type: {form.FormType}");
                        foreach (FormField field in form.Fields.Values)
                        {
                            Console.WriteLine($"Field '{field.Name}: ");

                            if (field.LabelData != null)
                            {
                                Console.WriteLine($"    Label: '{field.LabelData.Text}");
                            }

                            Console.WriteLine($"    Value: '{field.ValueData.Text}");
                            Console.WriteLine($"    Confidence: '{field.Confidence}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }         
        }
    }
}