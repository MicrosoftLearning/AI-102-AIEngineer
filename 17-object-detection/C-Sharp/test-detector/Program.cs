using System;
using System.IO;
using System.Drawing;
using Microsoft.Extensions.Configuration;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Prediction;

namespace test_detector
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

                // Load the image and prepare for drawing
                String image_file = "produce.jpg";
                Image image = Image.FromFile(image_file);
                int h = image.Height;
                int w = image.Width;
                Graphics graphics = Graphics.FromImage(image);
                Pen pen = new Pen(Color.Magenta, 3);
                Font font = new Font("Arial", 16);
                SolidBrush brush = new SolidBrush(Color.Black);

                using (var image_data = File.OpenRead(image_file))
                {
                    // Make a prediction against the new project
                    Console.WriteLine("Detecting objects in " + image_file);
                    var result = prediction_client.DetectImage(project_id, model_name, image_data);

                    // Loop over each prediction
                    foreach (var prediction in result.Predictions)
                    {
                        // Get each prediction with a probability > 50%
                        if (prediction.Probability > 0.5)
                        {
                            // The bounding box sizes are proportional - convert to absolute
                            int left = Convert.ToInt32(prediction.BoundingBox.Left * w);
                            int top = Convert.ToInt32(prediction.BoundingBox.Top * h);
                            int height = Convert.ToInt32(prediction.BoundingBox.Height * h);
                            int width =  Convert.ToInt32(prediction.BoundingBox.Width * w);
                            // Draw the bounding box
                            Rectangle rect = new Rectangle(left, top, width, height);
                            graphics.DrawRectangle(pen, rect);
                            // Annotate with the predicted label
                            graphics.DrawString(prediction.TagName,font,brush,left,top);
                
                        }
                    }
                }
                // Save the annotated image
                String output_file = "output.jpg";
                image.Save(output_file);
                Console.WriteLine("Results saved in " + output_file);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

        }
    }
}
