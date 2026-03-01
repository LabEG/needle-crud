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
| **SimpleCase_RepositoryMethod**  | **InMemory**   |   **205.1 μs** |  **47.22 μs** |  **44.17 μs** |   **46.8 KB** |
| SimpleCase_DirectDbContext   | InMemory   |   169.9 μs |  24.51 μs |  22.93 μs |  42.49 KB |
| MediumCase_RepositoryMethod  | InMemory   |   396.9 μs |  80.55 μs |  75.35 μs |   85.2 KB |
| MediumCase_DirectDbContext   | InMemory   |   312.0 μs |  37.76 μs |  35.32 μs |   76.8 KB |
| ComplexCase_RepositoryMethod | InMemory   |   373.2 μs |  28.57 μs |  26.73 μs |  90.42 KB |
| ComplexCase_DirectDbContext  | InMemory   |   374.8 μs |  54.43 μs |  50.92 μs |  77.13 KB |
| **SimpleCase_RepositoryMethod**  | **PostgreSQL** | **1,620.8 μs** | **155.08 μs** | **137.48 μs** |  **29.83 KB** |
| SimpleCase_DirectDbContext   | PostgreSQL | 1,641.8 μs | 195.82 μs | 173.59 μs |  25.77 KB |
| MediumCase_RepositoryMethod  | PostgreSQL | 3,130.4 μs | 226.60 μs | 200.87 μs |  65.73 KB |
| MediumCase_DirectDbContext   | PostgreSQL | 3,168.1 μs | 196.18 μs | 173.91 μs |  58.94 KB |
| ComplexCase_RepositoryMethod | PostgreSQL | 2,981.1 μs | 170.94 μs | 159.90 μs |  69.55 KB |
| ComplexCase_DirectDbContext  | PostgreSQL | 3,116.1 μs | 261.49 μs | 218.36 μs |  60.25 KB |
