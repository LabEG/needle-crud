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
| **SimpleCase_RepositoryMethod**  | **InMemory**   |   **217.7 μs** |  **47.19 μs** |  **44.14 μs** |   **46.8 KB** |
| SimpleCase_DirectDbContext   | InMemory   |   173.4 μs |  35.24 μs |  31.24 μs |  42.77 KB |
| MediumCase_RepositoryMethod  | InMemory   |   353.3 μs |  47.43 μs |  44.37 μs |  86.22 KB |
| MediumCase_DirectDbContext   | InMemory   |   330.7 μs |  29.73 μs |  27.81 μs |  77.75 KB |
| ComplexCase_RepositoryMethod | InMemory   |   447.2 μs |  65.05 μs |  57.67 μs |   89.8 KB |
| ComplexCase_DirectDbContext  | InMemory   |   325.5 μs |  38.54 μs |  36.05 μs |  77.34 KB |
| **SimpleCase_RepositoryMethod**  | **PostgreSQL** | **1,718.8 μs** | **104.95 μs** |  **87.64 μs** |  **29.83 KB** |
| SimpleCase_DirectDbContext   | PostgreSQL | 1,740.1 μs | 100.77 μs |  84.15 μs |  25.45 KB |
| MediumCase_RepositoryMethod  | PostgreSQL | 3,098.9 μs | 217.75 μs | 193.03 μs |   65.5 KB |
| MediumCase_DirectDbContext   | PostgreSQL | 3,143.6 μs | 260.14 μs | 230.61 μs |  58.55 KB |
| ComplexCase_RepositoryMethod | PostgreSQL | 3,153.0 μs | 165.82 μs | 155.11 μs |  69.39 KB |
| ComplexCase_DirectDbContext  | PostgreSQL | 2,974.1 μs | 198.60 μs | 176.05 μs |  60.81 KB |
