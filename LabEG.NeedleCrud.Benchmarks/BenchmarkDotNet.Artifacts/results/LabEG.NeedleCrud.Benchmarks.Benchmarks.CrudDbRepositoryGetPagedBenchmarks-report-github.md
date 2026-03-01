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
| **GetPaged_Simple**           | **InMemory**   |    **15.54 μs** |   **1.593 μs** |   **0.414 μs** | **2.3193** | **0.0916** |  **38.05 KB** |
| GetPaged_SimpleWithFilter | InMemory   |    24.33 μs |   3.582 μs |   0.554 μs | 2.5635 | 0.1221 |  42.31 KB |
| GetPaged_SimpleWithSort   | InMemory   |    30.73 μs |   0.749 μs |   0.194 μs | 2.8687 | 0.1221 |     47 KB |
| GetPaged_ComplexFilter    | InMemory   |    44.65 μs |   5.793 μs |   0.896 μs | 3.1738 |      - |  54.39 KB |
| GetPaged_ComplexSort      | InMemory   |    45.51 μs |   5.582 μs |   0.864 μs | 3.6621 |      - |  61.21 KB |
| GetPaged_ComplexFull      | InMemory   |    56.36 μs |   8.501 μs |   1.316 μs | 3.6621 |      - |  59.97 KB |
| GetPaged_SimpleGraph      | InMemory   |    35.92 μs |  38.332 μs |   5.932 μs | 3.4180 |      - |  57.62 KB |
| GetPaged_ComplexGraph     | InMemory   |    95.91 μs | 187.622 μs |  29.035 μs | 4.8828 |      - |  87.22 KB |
| **GetPaged_Simple**           | **PostgreSQL** | **1,016.02 μs** | **228.946 μs** |  **59.456 μs** |      **-** |      **-** |   **17.3 KB** |
| GetPaged_SimpleWithFilter | PostgreSQL | 1,049.45 μs | 256.606 μs |  66.640 μs |      - |      - |  21.25 KB |
| GetPaged_SimpleWithSort   | PostgreSQL | 1,122.18 μs | 393.759 μs | 102.258 μs |      - |      - |  23.83 KB |
| GetPaged_ComplexFilter    | PostgreSQL | 1,103.52 μs | 345.617 μs |  89.756 μs | 1.9531 |      - |  32.78 KB |
| GetPaged_ComplexSort      | PostgreSQL | 1,060.56 μs | 231.584 μs |  35.838 μs | 1.9531 |      - |  36.52 KB |
| GetPaged_ComplexFull      | PostgreSQL | 1,163.84 μs | 411.569 μs | 106.883 μs | 1.9531 |      - |  39.37 KB |
| GetPaged_SimpleGraph      | PostgreSQL | 1,137.38 μs | 324.543 μs |  84.283 μs |      - |      - |  23.06 KB |
| GetPaged_ComplexGraph     | PostgreSQL | 1,309.95 μs | 439.322 μs | 114.091 μs | 1.9531 |      - |  38.08 KB |
