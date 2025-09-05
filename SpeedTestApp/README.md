# Speed Test Application

A C# console application that tests internet speed (Download, Upload, Ping) and saves results to files.

## Features

- **Ping Test**: Tests latency to servers
- **Download Speed Test**: Measures download bandwidth
- **Upload Speed Test**: Measures upload bandwidth  
- **File Saving**: Save test results to text files
- **Auto-Detection**: Automatically detects internet connectivity
- **Demo Mode**: Works offline with simulated results for testing

## Requirements

- .NET 8.0 or later
- Internet connection (optional - works in demo mode without internet)

## How to Build

```bash
cd SpeedTestApp
dotnet build
```

## How to Run

```bash
cd SpeedTestApp
dotnet run
```

## How to Use

1. Run the application
2. Choose option "1" to run speed test
3. Wait for tests to complete
4. Choose "y" to save results to file
5. Choose option "2" to exit

## Project Structure

- `Program.cs` - Main application entry point and user interface
- `ISpeedTester.cs` - Interface for speed testing implementations
- `SpeedTester.cs` - Real internet speed testing implementation
- `MockSpeedTester.cs` - Simulated speed testing for demo mode
- `SpeedTestApp.csproj` - Project configuration

## Implementation Details

The application automatically detects internet connectivity:
- **With Internet**: Uses real HTTP requests to test actual speeds
- **Without Internet**: Uses simulated results for demonstration

Results are saved with timestamps in the format: `SpeedTest_YYYY-MM-DD_HH-mm-ss.txt`