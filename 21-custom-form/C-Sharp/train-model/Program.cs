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

namespace train_model
{
    class Program
    {     
        static async Task Main(string[] args)
        {   
            try
            {   
                // Get configuration settings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string formEndpoint = configuration["FormEndpoint"];
                string formKey = configuration["FormKey"];
                string trainingStorageUri = configuration["StorageUri"];
                
                // Authenticate Form Training Client 
                var credential = new AzureKeyCredential(formKey);
                var trainingClient = new FormTrainingClient(new Uri(formEndpoint), credential);  

                // Train model 
                CustomFormModel model = await trainingClient
                .StartTrainingAsync(new Uri(trainingStorageUri), useTrainingLabels: false)
                .WaitForCompletionAsync();

                // Get model info
                Console.WriteLine($"Custom Model Info:");
                Console.WriteLine($"    Model Id: {model.ModelId}");
                Console.WriteLine($"    Model Status: {model.Status}");
                Console.WriteLine($"    Training model started on: {model.TrainingStartedOn}");
                Console.WriteLine($"    Training model completed on: {model.TrainingCompletedOn}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }  
        }
    }
}