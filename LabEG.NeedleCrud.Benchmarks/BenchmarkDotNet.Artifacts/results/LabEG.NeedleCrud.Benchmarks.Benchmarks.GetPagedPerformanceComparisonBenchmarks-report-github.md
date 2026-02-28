```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-MEHJPP : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=5  WarmupCount=1  

```
| Method                       | Provider   | Mean        | Error      | StdDev     | Gen0   | Allocated |
|----------------------------- |----------- |------------:|-----------:|-----------:|-------:|----------:|
| **SimpleCase_RepositoryMethod**  | **InMemory**   |    **35.96 μs** |  **14.029 μs** |   **2.171 μs** | **2.6855** |  **45.01 KB** |
| SimpleCase_DirectDbContext   | InMemory   |    27.47 μs |   0.868 μs |   0.134 μs | 2.4414 |   40.2 KB |
| MediumCase_RepositoryMethod  | InMemory   |   110.41 μs | 161.962 μs |  25.064 μs | 3.9063 |  68.75 KB |
| MediumCase_DirectDbContext   | InMemory   |   103.83 μs |  60.512 μs |   9.364 μs | 4.3945 |  73.95 KB |
| ComplexCase_RepositoryMethod | InMemory   |   111.98 μs | 161.115 μs |  24.933 μs | 4.3945 |  72.43 KB |
| ComplexCase_DirectDbContext  | InMemory   |   175.08 μs | 256.522 μs |  39.697 μs | 5.8594 | 106.79 KB |
| **SimpleCase_RepositoryMethod**  | **PostgreSQL** | **1,174.89 μs** | **207.404 μs** |  **53.862 μs** |      **-** |  **25.95 KB** |
| SimpleCase_DirectDbContext   | PostgreSQL | 1,177.68 μs | 287.091 μs |  74.557 μs |      - |  20.73 KB |
| MediumCase_RepositoryMethod  | PostgreSQL | 1,225.67 μs | 448.570 μs |  69.417 μs | 1.9531 |  48.16 KB |
| MediumCase_DirectDbContext   | PostgreSQL | 2,348.23 μs | 360.612 μs |  93.650 μs |      - |  53.86 KB |
| ComplexCase_RepositoryMethod | PostgreSQL | 1,275.76 μs | 435.993 μs | 113.226 μs | 1.9531 |  51.36 KB |
| ComplexCase_DirectDbContext  | PostgreSQL | 2,449.90 μs | 296.523 μs |  45.887 μs | 3.9063 |  71.95 KB |
