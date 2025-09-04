using System;
using System.Threading.Tasks;
using System.IO;
using System.Text;

namespace SpeedTestApp
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("=================================");
            Console.WriteLine("    Internet Speed Test Tool     ");
            Console.WriteLine("=================================");
            Console.WriteLine();

            // Check for internet connectivity and use appropriate tester
            Console.WriteLine("Checking internet connectivity...");
            var useRealTester = await CheckInternetConnectivity();
            
            ISpeedTester speedTester;
            
            if (useRealTester)
            {
                Console.WriteLine("Internet connection detected. Using real speed tests.");
                speedTester = new SpeedTester();
            }
            else
            {
                Console.WriteLine("No internet connection detected. Using demo mode with simulated results.");
                speedTester = new MockSpeedTester();
            }

            await RunMainLoop(speedTester);
            speedTester.Dispose();
        }

        static async Task<bool> CheckInternetConnectivity()
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(5);
                    var response = await client.GetAsync("https://www.google.com");
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        static async Task RunMainLoop(ISpeedTester speedTester)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("Choose an option:");
                Console.WriteLine("1. Run Speed Test");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice (1-2): ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await RunSpeedTest(speedTester);
                        break;
                    case "2":
                        Console.WriteLine("Thank you for using Speed Test Tool!");
                        return;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        static async Task RunSpeedTest(ISpeedTester speedTester)
        {
            Console.WriteLine();
            Console.WriteLine("Starting speed test...");
            Console.WriteLine("======================");

            var results = new StringBuilder();
            results.AppendLine($"Speed Test Results - {DateTime.Now}");
            results.AppendLine("================================");

            try
            {
                // Test Ping
                Console.WriteLine("Testing ping...");
                Console.WriteLine("Connecting to test server...");
                var ping = await speedTester.TestPingAsync();
                Console.WriteLine($"Ping: {ping:F1} ms");
                results.AppendLine($"Ping: {ping:F1} ms");

                // Test Download Speed
                Console.WriteLine("Testing download speed...");
                Console.WriteLine("Downloading test file...");
                var downloadSpeed = await speedTester.TestDownloadSpeedAsync();
                Console.WriteLine($"Download Speed: {downloadSpeed:F2} Mbps");
                results.AppendLine($"Download Speed: {downloadSpeed:F2} Mbps");

                // Test Upload Speed
                Console.WriteLine("Testing upload speed...");
                Console.WriteLine("Uploading test data...");
                var uploadSpeed = await speedTester.TestUploadSpeedAsync();
                Console.WriteLine($"Upload Speed: {uploadSpeed:F2} Mbps");
                results.AppendLine($"Upload Speed: {uploadSpeed:F2} Mbps");

                Console.WriteLine();
                Console.WriteLine("Speed test completed!");

                // Ask user if they want to save results
                Console.Write("Do you want to save these results to a file? (y/n): ");
                string? saveChoice = Console.ReadLine();

                if (saveChoice?.ToLower() == "y" || saveChoice?.ToLower() == "yes")
                {
                    await SaveResults(results.ToString(), ping, downloadSpeed, uploadSpeed);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during speed test: {ex.Message}");
            }
        }

        static async Task SaveResults(string results, double ping, double downloadSpeed, double uploadSpeed)
        {
            try
            {
                string fileName = $"SpeedTest_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt";
                
                // Add detailed results
                var fullResults = new StringBuilder();
                fullResults.AppendLine(results);
                fullResults.AppendLine();
                fullResults.AppendLine("Summary:");
                fullResults.AppendLine($"- Ping: {ping:F1} ms");
                fullResults.AppendLine($"- Download: {downloadSpeed:F2} Mbps");
                fullResults.AppendLine($"- Upload: {uploadSpeed:F2} Mbps");
                fullResults.AppendLine();
                fullResults.AppendLine("Test performed using SpeedTestApp");

                await File.WriteAllTextAsync(fileName, fullResults.ToString());
                Console.WriteLine($"Results saved to: {fileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving results: {ex.Message}");
            }
        }
    }
}
