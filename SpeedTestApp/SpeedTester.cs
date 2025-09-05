using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Text;

namespace SpeedTestApp
{
    public class SpeedTester : ISpeedTester
    {
        private readonly HttpClient httpClient;
        private const string DOWNLOAD_TEST_URL = "https://httpbin.org/bytes/1048576"; // 1MB file for faster testing
        private const string UPLOAD_TEST_URL = "https://httpbin.org/post";
        private const string PING_HOST = "8.8.8.8"; // Google DNS

        public SpeedTester()
        {
            httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(2);
        }

        public async Task<double> TestPingAsync()
        {
            try
            {
                using (var ping = new Ping())
                {
                    var reply = await ping.SendPingAsync(PING_HOST, 5000);
                    if (reply.Status == IPStatus.Success)
                    {
                        return reply.RoundtripTime;
                    }
                    else
                    {
                        throw new Exception($"Ping failed: {reply.Status}");
                    }
                }
            }
            catch (Exception)
            {
                // Fallback: try alternative ping method
                return await PingAlternativeAsync();
            }
        }

        private async Task<double> PingAlternativeAsync()
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                using (var response = await httpClient.GetAsync("https://www.google.com", HttpCompletionOption.ResponseHeadersRead))
                {
                    stopwatch.Stop();
                    
                    if (response.IsSuccessStatusCode)
                    {
                        return stopwatch.ElapsedMilliseconds;
                    }
                    else
                    {
                        throw new Exception("Alternative ping failed");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Ping test failed: {ex.Message}");
            }
        }

        public async Task<double> TestDownloadSpeedAsync()
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();
                
                // Download test data
                using (var response = await httpClient.GetAsync(DOWNLOAD_TEST_URL))
                {
                    var data = await response.Content.ReadAsByteArrayAsync();
                    stopwatch.Stop();

                    // Calculate speed in Mbps
                    double sizeInMB = data.Length / (1024.0 * 1024.0);
                    double timeInSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                    
                    if (timeInSeconds > 0)
                    {
                        double speedMbps = (sizeInMB * 8) / timeInSeconds; // Convert to Megabits per second
                        return speedMbps;
                    }
                    else
                    {
                        return 0;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Download speed test failed: {ex.Message}");
            }
        }

        public async Task<double> TestUploadSpeedAsync()
        {
            try
            {
                // Create smaller test data (512KB for faster testing)
                byte[] testData = new byte[512 * 1024];
                new Random().NextBytes(testData);

                using (var content = new ByteArrayContent(testData))
                {
                    content.Headers.Add("Content-Type", "application/octet-stream");

                    var stopwatch = Stopwatch.StartNew();
                    
                    // Upload test data
                    using (var response = await httpClient.PostAsync(UPLOAD_TEST_URL, content))
                    {
                        stopwatch.Stop();

                        if (response.IsSuccessStatusCode)
                        {
                            // Calculate speed in Mbps
                            double sizeInMB = testData.Length / (1024.0 * 1024.0);
                            double timeInSeconds = stopwatch.ElapsedMilliseconds / 1000.0;
                            
                            if (timeInSeconds > 0)
                            {
                                double speedMbps = (sizeInMB * 8) / timeInSeconds; // Convert to Megabits per second
                                return speedMbps;
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            throw new Exception($"Upload failed with status: {response.StatusCode}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Upload speed test failed: {ex.Message}");
            }
        }

        public void Dispose()
        {
            httpClient?.Dispose();
        }
    }
}