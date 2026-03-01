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
| **SimpleCase_RepositoryMethod**  | **InMemory**   |   **179.0 μs** |  **11.91 μs** |   **9.95 μs** |  **47.12 KB** |
| SimpleCase_DirectDbContext   | InMemory   |   160.4 μs |  10.87 μs |   9.64 μs |  42.49 KB |
| MediumCase_RepositoryMethod  | InMemory   |   329.4 μs |  11.43 μs |  10.13 μs |  85.09 KB |
| MediumCase_DirectDbContext   | InMemory   |   285.6 μs |  12.87 μs |  11.41 μs |  77.09 KB |
| ComplexCase_RepositoryMethod | InMemory   |   385.2 μs |  36.44 μs |  34.09 μs |  90.68 KB |
| ComplexCase_DirectDbContext  | InMemory   |   293.9 μs |   8.29 μs |   7.35 μs |  77.19 KB |
| **SimpleCase_RepositoryMethod**  | **PostgreSQL** | **1,551.1 μs** | **127.63 μs** | **113.15 μs** |  **29.83 KB** |
| SimpleCase_DirectDbContext   | PostgreSQL | 1,500.8 μs |  75.07 μs |  62.69 μs |  25.45 KB |
| MediumCase_RepositoryMethod  | PostgreSQL | 2,943.8 μs | 232.75 μs | 206.33 μs |   65.5 KB |
| MediumCase_DirectDbContext   | PostgreSQL | 2,874.2 μs | 171.65 μs | 160.56 μs |  60.45 KB |
| ComplexCase_RepositoryMethod | PostgreSQL | 3,243.6 μs | 557.99 μs | 494.65 μs |   69.4 KB |
| ComplexCase_DirectDbContext  | PostgreSQL | 2,992.9 μs | 208.42 μs | 194.96 μs |  59.79 KB |
