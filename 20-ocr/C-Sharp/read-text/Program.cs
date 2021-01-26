using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

// Import namespaces


namespace read_text
{
    class Program
    {

        private static ComputerVisionClient cvClient;
        static async Task Main(string[] args)
        {
            try
            {
                // Get config settings from AppSettings
                IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
                IConfigurationRoot configuration = builder.Build();
                string cogSvcEndpoint = configuration["CognitiveServicesEndpoint"];
                string cogSvcKey = configuration["CognitiveServiceKey"];

                // Authenticate Computer Vision client
                


                // Menu for text reading functions
                Console.WriteLine("1: Use OCR API\n2: Use Read API\n3: Read handwriting\nAny other key to quit");
                Console.WriteLine("Enter a number:");
                string command = Console.ReadLine();
                string imageFile;
                switch (command)
                {
                    case "1":
                        imageFile = "images/Lincoln.jpg";
                        await GetTextOcr(imageFile);
                        break;
                    case "2":
                        imageFile = "images/Rome.pdf";
                        await GetTextRead(imageFile);
                        break;
                    case "3":
                        imageFile = "images/Note.jpg";
                        await GetTextRead(imageFile);
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

        static async Task GetTextOcr(string imageFile)
        {
            Console.WriteLine($"Reading text in {imageFile}\n");

                
        }

        static async Task GetTextRead(string imageFile)
        {
            Console.WriteLine($"Reading text in {imageFile}\n");

     
        }
    }
}
