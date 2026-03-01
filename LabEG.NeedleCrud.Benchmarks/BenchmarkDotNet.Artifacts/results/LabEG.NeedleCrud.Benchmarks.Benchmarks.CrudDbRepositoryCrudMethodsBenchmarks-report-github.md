```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-KQXXMH : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=15  UnrollFactor=1  
WarmupCount=3  

```
| Method  | Provider   | Mean       | Error     | StdDev    | Allocated |
|-------- |----------- |-----------:|----------:|----------:|----------:|
| **Create**  | **InMemory**   |   **171.1 μs** |  **33.74 μs** |  **29.91 μs** |   **5.27 KB** |
| GetById | InMemory   |   254.4 μs |  51.02 μs |  47.73 μs |  21.78 KB |
| GetAll  | InMemory   | 1,033.6 μs | 254.38 μs | 225.50 μs | 199.92 KB |
| Update  | InMemory   |   423.3 μs |  75.41 μs |  66.85 μs |   24.3 KB |
| Delete  | InMemory   |   354.3 μs |  73.20 μs |  68.47 μs |  26.72 KB |
| **Create**  | **PostgreSQL** | **2,122.4 μs** | **188.25 μs** | **166.88 μs** |  **24.02 KB** |
| GetById | PostgreSQL | 1,384.9 μs | 273.57 μs | 213.59 μs |  14.03 KB |
| GetAll  | PostgreSQL | 2,238.4 μs | 365.36 μs | 341.76 μs | 197.59 KB |
| Update  | PostgreSQL | 2,802.4 μs | 207.04 μs | 161.65 μs |  37.78 KB |
| Delete  | PostgreSQL | 2,887.3 μs | 467.94 μs | 437.71 μs |  27.17 KB |
