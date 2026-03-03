```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-KQXXMH : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=15  UnrollFactor=1  
WarmupCount=3  

```
| Method                       | Provider   | Mean       | Error     | StdDev    | Allocated |
|----------------------------- |----------- |-----------:|----------:|----------:|----------:|
| **SimpleCase_RepositoryMethod**  | **InMemory**   |   **192.3 μs** |  **12.21 μs** |  **11.42 μs** |   **46.8 KB** |
| SimpleCase_DirectDbContext   | InMemory   |   171.2 μs |  22.53 μs |  19.97 μs |  42.63 KB |
| MediumCase_RepositoryMethod  | InMemory   |   358.3 μs |  43.02 μs |  40.24 μs |  85.64 KB |
| MediumCase_DirectDbContext   | InMemory   |   300.0 μs |  19.58 μs |  17.36 μs |     77 KB |
| ComplexCase_RepositoryMethod | InMemory   |   378.7 μs |  37.82 μs |  33.53 μs |  90.12 KB |
| ComplexCase_DirectDbContext  | InMemory   |   309.2 μs |  24.88 μs |  20.78 μs |  77.94 KB |
| **SimpleCase_RepositoryMethod**  | **PostgreSQL** | **1,790.0 μs** | **196.47 μs** | **174.16 μs** |  **29.84 KB** |
| SimpleCase_DirectDbContext   | PostgreSQL | 1,712.5 μs | 229.69 μs | 203.61 μs |   25.6 KB |
| MediumCase_RepositoryMethod  | PostgreSQL | 3,007.1 μs | 149.58 μs | 139.92 μs |  66.16 KB |
| MediumCase_DirectDbContext   | PostgreSQL | 2,961.4 μs | 152.46 μs | 135.15 μs |  59.86 KB |
| ComplexCase_RepositoryMethod | PostgreSQL | 3,133.3 μs | 154.17 μs | 144.21 μs |  69.73 KB |
| ComplexCase_DirectDbContext  | PostgreSQL | 2,971.2 μs | 280.32 μs | 248.49 μs |  59.91 KB |
