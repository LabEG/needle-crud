```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-AGBPQC : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=15  WarmupCount=3  

```
| Method                    | Provider   | Mean        | Error     | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |----------- |------------:|----------:|----------:|-------:|-------:|----------:|
| **GetPaged_Simple**           | **InMemory**   |    **15.56 μs** |  **0.285 μs** |  **0.266 μs** | **2.3193** | **0.0916** |  **38.05 KB** |
| GetPaged_SimpleWithFilter | InMemory   |    27.16 μs |  3.937 μs |  3.490 μs | 2.5635 | 0.1221 |  42.31 KB |
| GetPaged_SimpleWithSort   | InMemory   |    31.90 μs |  0.711 μs |  0.593 μs | 2.8687 | 0.1221 |     47 KB |
| GetPaged_ComplexFilter    | InMemory   |    44.13 μs |  0.781 μs |  0.652 μs | 3.1738 |      - |  54.39 KB |
| GetPaged_ComplexSort      | InMemory   |    45.58 μs |  1.058 μs |  0.938 μs | 3.6621 |      - |  61.21 KB |
| GetPaged_ComplexFull      | InMemory   |    57.55 μs |  4.715 μs |  4.180 μs | 3.4180 |      - |  59.98 KB |
| GetPaged_SimpleGraph      | InMemory   |    33.46 μs |  1.712 μs |  1.601 μs | 3.4180 |      - |  57.62 KB |
| GetPaged_ComplexGraph     | InMemory   |    74.95 μs |  3.678 μs |  2.871 μs | 4.8828 |      - |  87.22 KB |
| **GetPaged_Simple**           | **PostgreSQL** |   **943.90 μs** |  **5.325 μs** |  **4.721 μs** |      **-** |      **-** |   **17.3 KB** |
| GetPaged_SimpleWithFilter | PostgreSQL |   974.56 μs | 15.562 μs | 14.556 μs |      - |      - |  21.31 KB |
| GetPaged_SimpleWithSort   | PostgreSQL | 1,030.84 μs | 12.206 μs | 11.418 μs |      - |      - |  23.82 KB |
| GetPaged_ComplexFilter    | PostgreSQL | 1,053.63 μs | 13.586 μs | 11.345 μs | 1.9531 |      - |  32.78 KB |
| GetPaged_ComplexSort      | PostgreSQL | 1,061.19 μs | 15.886 μs | 14.859 μs | 1.9531 |      - |  36.16 KB |
| GetPaged_ComplexFull      | PostgreSQL | 1,062.80 μs | 15.186 μs | 12.681 μs | 1.9531 |      - |  39.37 KB |
| GetPaged_SimpleGraph      | PostgreSQL | 1,090.89 μs | 19.167 μs | 17.929 μs |      - |      - |  23.05 KB |
| GetPaged_ComplexGraph     | PostgreSQL | 1,226.79 μs |  8.947 μs |  7.932 μs | 1.9531 |      - |  38.06 KB |
