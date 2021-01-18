using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

// Import namespaces



namespace analyze_faces
{
    class Program
    {

        private static FaceClient faceClient;
        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string cogSvcEndpoint = configuration["CognitiveServicesEndpoint"];
                string cogSvcKey = configuration["CognitiveServiceKey"];

                // Authenticate Face client


                // Menu for face functions
                Console.WriteLine("1: Detect faces\n2: Compare faces\n3: Train a facial recognition model\n4: Recognize faces\n5: Verify a face\nAny other key to quit");
                Console.WriteLine("Enter a number:");
                string command = Console.ReadLine();
                switch (command)
                {
                    case "1":
                        await DetectFaces("images/people.jpg");
                        break;
                    case "2":
                        string personImage = "images/person1.jpg"; // Also try person2.jpg
                        await CompareFaces(personImage, "images/people.jpg");
                        break;
                    case "3":
                        List<string> names = new List<string>(){"Aisha", "Sama"};
                        await TrainModel("employees_group", "employees", names);
                        break;
                    case "4":
                        await RecognizeFaces("images/people.jpg", "employees_group");
                        break;
                    case "5":
                        await VerifyFace("images/person1.jpg", "Aisha", "employees_group");
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        static async Task DetectFaces(string imageFile)
        {
            Console.WriteLine($"Detecting faces in {imageFile}");

            // Specify facial features to be retrieved


            // Get faces
 
 
        }

        static async Task CompareFaces(string image1, string image2)
        {
            Console.WriteLine($"Comparing faces in {image1} and {image2}");


        }

        static async Task TrainModel(string groupId, string groupName, List<string> imageFolders)
        {
            Console.WriteLine($"Creating model for {groupId}");



        }

        static async Task RecognizeFaces(string imageFile, string groupId)
        {
            Console.WriteLine($"Recognizing faces in {imageFile}");

        
        }

        static async Task VerifyFace(string personImage, string personName, string groupId)
        {
            Console.WriteLine($"Verifying the person in {personImage} is {personName}");

            string result = "Not verified";

            // Get the ID of the person from the people group


            // print the result
            Console.WriteLine(result);
        }
    }
}
