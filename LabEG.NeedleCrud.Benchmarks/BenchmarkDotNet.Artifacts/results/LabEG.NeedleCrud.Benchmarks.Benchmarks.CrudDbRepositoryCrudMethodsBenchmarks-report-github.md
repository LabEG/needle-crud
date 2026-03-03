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
| **Create**  | **InMemory**   |   **157.2 μs** |  **25.04 μs** |  **22.20 μs** |   **5.27 KB** |
| GetById | InMemory   |   260.2 μs |  51.85 μs |  48.50 μs |  21.78 KB |
| GetAll  | InMemory   | 1,076.0 μs | 246.01 μs | 230.12 μs | 219.16 KB |
| Update  | InMemory   |   128.6 μs |  23.24 μs |  19.40 μs |   5.11 KB |
| Delete  | InMemory   |   352.9 μs |  84.55 μs |  79.09 μs |  26.42 KB |
| **Create**  | **PostgreSQL** | **1,898.2 μs** | **238.85 μs** | **211.73 μs** |  **24.02 KB** |
| GetById | PostgreSQL | 1,260.9 μs | 186.17 μs | 155.46 μs |  13.99 KB |
| GetAll  | PostgreSQL | 3,139.8 μs | 320.91 μs | 284.48 μs | 204.38 KB |
| Update  | PostgreSQL | 1,870.1 μs | 175.27 μs | 155.38 μs |  27.24 KB |
| Delete  | PostgreSQL | 2,644.5 μs | 222.32 μs | 197.08 μs |   27.8 KB |
