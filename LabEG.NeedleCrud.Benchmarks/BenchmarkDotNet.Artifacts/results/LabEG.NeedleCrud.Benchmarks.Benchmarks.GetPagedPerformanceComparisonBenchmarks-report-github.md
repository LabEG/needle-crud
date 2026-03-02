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
| **SimpleCase_RepositoryMethod**  | **InMemory**   |   **323.0 μs** |  **78.45 μs** |  **73.38 μs** |  **47.09 KB** |
| SimpleCase_DirectDbContext   | InMemory   |   284.5 μs |  32.69 μs |  25.52 μs |  42.49 KB |
| MediumCase_RepositoryMethod  | InMemory   |   384.7 μs |  45.13 μs |  42.22 μs |  85.09 KB |
| MediumCase_DirectDbContext   | InMemory   |   327.4 μs |  28.53 μs |  25.29 μs |  77.14 KB |
| ComplexCase_RepositoryMethod | InMemory   |   355.5 μs |  47.75 μs |  42.33 μs |   88.7 KB |
| ComplexCase_DirectDbContext  | InMemory   |   319.4 μs |  33.54 μs |  31.37 μs |  76.99 KB |
| **SimpleCase_RepositoryMethod**  | **PostgreSQL** | **1,942.2 μs** |  **93.53 μs** |  **87.49 μs** |  **29.83 KB** |
| SimpleCase_DirectDbContext   | PostgreSQL | 1,835.4 μs |  84.27 μs |  78.82 μs |  25.75 KB |
| MediumCase_RepositoryMethod  | PostgreSQL | 3,024.9 μs | 165.77 μs | 146.95 μs |  65.62 KB |
| MediumCase_DirectDbContext   | PostgreSQL | 3,025.4 μs | 233.24 μs | 206.77 μs |     59 KB |
| ComplexCase_RepositoryMethod | PostgreSQL | 3,089.8 μs | 188.97 μs | 176.77 μs |  69.28 KB |
| ComplexCase_DirectDbContext  | PostgreSQL | 2,969.1 μs | 124.96 μs | 110.78 μs |  59.31 KB |
