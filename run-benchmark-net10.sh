#!/bin/bash

# Benchmark runner script for .NET 10.0
# Usage: ./run-benchmark-net10.sh [benchmark-filter]

FRAMEWORK="net10.0"
CONFIGURATION="Release"
BENCHMARK_DIR="LabEG.NeedleCrud.Benchmarks"

echo "Running benchmarks with .NET 10.0..."
echo "Filter: $FILTER"
echo ""

cd "$BENCHMARK_DIR" && dotnet run -c $CONFIGURATION --framework $FRAMEWORK
