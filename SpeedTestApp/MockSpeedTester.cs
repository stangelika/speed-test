using System;
using System.Threading.Tasks;

namespace SpeedTestApp
{
    public class MockSpeedTester : ISpeedTester
    {
        private readonly Random random;

        public MockSpeedTester()
        {
            random = new Random();
        }

        public async Task<double> TestPingAsync()
        {
            // Simulate ping test delay
            await Task.Delay(1000);
            
            // Return a realistic ping value (20-100ms)
            return 20 + (random.NextDouble() * 80);
        }

        public async Task<double> TestDownloadSpeedAsync()
        {
            // Simulate download test delay
            await Task.Delay(2000);
            
            // Return a realistic download speed (10-100 Mbps)
            return 10 + (random.NextDouble() * 90);
        }

        public async Task<double> TestUploadSpeedAsync()
        {
            // Simulate upload test delay
            await Task.Delay(2000);
            
            // Return a realistic upload speed (5-50 Mbps, typically slower than download)
            return 5 + (random.NextDouble() * 45);
        }

        public void Dispose()
        {
            // Nothing to dispose in mock version
        }
    }
}