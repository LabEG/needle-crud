#!/bin/bash

# Benchmark runner script for .NET 10.0
# Usage: ./run-benchmark-net10.sh [benchmark-filter]

FRAMEWORK="net10.0"
CONFIGURATION="Release"

echo "Running benchmarks with .NET 10.0..."
echo "Filter: $FILTER"
echo ""

dotnet run -c $CONFIGURATION --framework $FRAMEWORK
