```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-MEHJPP : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=5  WarmupCount=1  

```
| Method                    | Provider   | Mean        | Error      | StdDev     | Gen0   | Gen1   | Allocated |
|-------------------------- |----------- |------------:|-----------:|-----------:|-------:|-------:|----------:|
| **GetPaged_Simple**           | **InMemory**   |    **22.18 μs** |   **5.471 μs** |   **1.421 μs** | **2.3193** | **0.0916** |  **38.05 KB** |
| GetPaged_SimpleWithFilter | InMemory   |    41.11 μs |  40.309 μs |  10.468 μs | 2.4414 |      - |  42.31 KB |
| GetPaged_SimpleWithSort   | InMemory   |    38.91 μs |   1.181 μs |   0.183 μs | 2.8687 | 0.1221 |     47 KB |
| GetPaged_ComplexFilter    | InMemory   |    56.75 μs |   7.414 μs |   1.147 μs | 3.1738 |      - |  54.39 KB |
| GetPaged_ComplexSort      | InMemory   |    53.14 μs |  17.917 μs |   4.653 μs | 3.6621 |      - |  61.21 KB |
| GetPaged_ComplexFull      | InMemory   |   108.31 μs | 135.136 μs |  35.094 μs | 3.4180 |      - |  60.42 KB |
| GetPaged_SimpleGraph      | InMemory   |    49.79 μs |  46.306 μs |  12.026 μs | 3.4180 |      - |  57.41 KB |
| GetPaged_ComplexGraph     | InMemory   |   146.03 μs | 278.869 μs |  72.421 μs | 4.8828 |      - |  87.05 KB |
| **GetPaged_Simple**           | **PostgreSQL** |   **993.02 μs** | **170.023 μs** |  **26.311 μs** |      **-** |      **-** |  **17.26 KB** |
| GetPaged_SimpleWithFilter | PostgreSQL | 1,051.32 μs | 297.896 μs |  77.363 μs |      - |      - |  21.29 KB |
| GetPaged_SimpleWithSort   | PostgreSQL | 1,131.85 μs | 294.653 μs |  76.520 μs |      - |      - |  23.79 KB |
| GetPaged_ComplexFilter    | PostgreSQL | 1,192.40 μs | 370.640 μs |  96.254 μs | 1.9531 |      - |  32.87 KB |
| GetPaged_ComplexSort      | PostgreSQL | 1,132.42 μs | 374.713 μs |  97.312 μs | 1.9531 |      - |  36.02 KB |
| GetPaged_ComplexFull      | PostgreSQL | 1,185.03 μs | 259.239 μs |  40.118 μs | 1.9531 |      - |  39.33 KB |
| GetPaged_SimpleGraph      | PostgreSQL | 1,181.80 μs | 194.909 μs |  30.162 μs |      - |      - |  22.78 KB |
| GetPaged_ComplexGraph     | PostgreSQL | 1,386.37 μs | 452.470 μs | 117.505 μs | 1.9531 |      - |  37.96 KB |
