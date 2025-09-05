using System;
using System.Threading.Tasks;

namespace SpeedTestApp
{
    public interface ISpeedTester
    {
        Task<double> TestPingAsync();
        Task<double> TestDownloadSpeedAsync();
        Task<double> TestUploadSpeedAsync();
        void Dispose();
    }
}