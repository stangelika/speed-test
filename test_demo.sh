#!/bin/bash

# Build the application
echo "Building Speed Test Application..."
cd SpeedTestApp
dotnet build --quiet

# Run the application with simulated input
echo "Running speed test with simulated user input..."
echo -e "1\ny\n2\n" | timeout 15s dotnet run

echo ""
echo "Checking for saved results..."
if ls SpeedTest_*.txt 1> /dev/null 2>&1; then
    echo "✅ Results file created successfully!"
    echo "Latest result file contents:"
    echo "----------------------------------------"
    cat $(ls -t SpeedTest_*.txt | head -1)
    echo "----------------------------------------"
else
    echo "❌ No results file found"
fi