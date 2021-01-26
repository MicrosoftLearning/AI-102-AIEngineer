using System;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Linq;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

// dotnet add package Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training --version 2.0.0

namespace train_detector
{
    class Program
    {

        static CustomVisionTrainingClient training_client;
        static Project custom_vision_project;
        static void Main(string[] args)
        {
            // Get Configuration Settings
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            string training_endpoint = configuration["TrainingEndpoint"];
            string training_key = configuration["TrainingKey"];
            Guid project_id = Guid.Parse(configuration["ProjectID"]);

            try
            {
                // Authenticate a client for the training API
                training_client = new CustomVisionTrainingClient(new Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.ApiKeyServiceClientCredentials(training_key))
                {
                    Endpoint = training_endpoint
                };

                // Get the Custom Vision project
                custom_vision_project = training_client.GetProject(project_id);

                // Upload and tag images
                Upload_Images("images");
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        static void Upload_Images(string folder)
        {
            // Get the tags defined in the project
            IList<Tag> tags = training_client.GetTags(custom_vision_project.Id);

            // Create a list of images with tagged regions
            var imageFileEntries = new List<ImageFileCreateEntry>();

            // Get the images and tagged regions from the JSON file
            string tag_json = File.ReadAllText("tagged-images.json");
            using (JsonDocument document = JsonDocument.Parse(tag_json))
            {
                JsonElement files = document.RootElement.GetProperty("files");
                foreach (JsonElement file in files.EnumerateArray())
                {
                    // Get the filename
                    string filename = file.GetProperty("filename").GetString();

                    // Get the tagged regions
                    JsonElement tagged_regions = file.GetProperty("tags");
                    List<Region> regions = new List<Region>();
                    foreach (JsonElement tag in tagged_regions.EnumerateArray())
                    {
                        string tag_name = tag.GetProperty("tag").GetString();
                        // Look up the tag ID for this tag name
                        var tag_item = tags.FirstOrDefault(t => t.Name == tag_name);
                        Guid tag_id = tag_item.Id;
                        Double left = tag.GetProperty("left").GetDouble();
                        Double top = tag.GetProperty("top").GetDouble();
                        Double width = tag.GetProperty("width").GetDouble();
                        Double height = tag.GetProperty("height").GetDouble();
                        // Add a region for this tag using the coordinates and dimensions in the JSON
                        regions.Add (new Region(tag_id, left, top, width, height));
                    }

                    // Add the image and its regions to the list
                    imageFileEntries.Add(new ImageFileCreateEntry(filename, File.ReadAllBytes(Path.Combine(folder,filename)), null, regions));
                }
            }
            // Upload the list of images as a batch
            Console.WriteLine("Uploading " + imageFileEntries.Count() + " tagged images...");
            ImageCreateSummary result = training_client.CreateImagesFromFiles(custom_vision_project.Id, new ImageFileCreateBatch(imageFileEntries));
            if(result.IsBatchSuccessful)
            {
                Console.WriteLine("Upload complete.");
            }
            else
            {
                Console.WriteLine("Something went wrong.");
                foreach(ImageCreateResult image in result.Images)
                {
                    Console.WriteLine(image.Status);
                }
            }
        }
    }
}
