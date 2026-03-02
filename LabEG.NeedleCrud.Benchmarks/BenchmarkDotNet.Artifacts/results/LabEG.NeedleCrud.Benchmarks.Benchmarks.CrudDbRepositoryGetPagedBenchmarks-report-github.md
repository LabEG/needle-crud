```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-AGBPQC : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=15  WarmupCount=3  

```
| Method                            | Provider   | Mean        | Error     | StdDev    | Gen0    | Gen1   | Allocated |
|---------------------------------- |----------- |------------:|----------:|----------:|--------:|-------:|----------:|
| **GetPaged_Simple**                   | **InMemory**   |    **15.84 μs** |  **0.173 μs** |  **0.145 μs** |  **2.3193** | **0.0610** |  **38.05 KB** |
| GetPaged_SimpleWithFilter         | InMemory   |    24.17 μs |  0.465 μs |  0.388 μs |  2.5635 | 0.1221 |  42.31 KB |
| GetPaged_SimpleWithSort           | InMemory   |    32.82 μs |  1.274 μs |  1.191 μs |  2.8687 | 0.1221 |     47 KB |
| GetPaged_ComplexFilter            | InMemory   |    44.58 μs |  0.934 μs |  0.780 μs |  3.1738 |      - |  54.39 KB |
| GetPaged_ComplexSort              | InMemory   |    47.23 μs |  2.120 μs |  1.879 μs |  3.6621 | 0.1221 |  61.22 KB |
| GetPaged_ComplexFull              | InMemory   |    57.28 μs |  3.481 μs |  2.907 μs |  3.4180 |      - |  60.08 KB |
| GetPaged_SimpleGraph              | InMemory   |    33.15 μs |  1.094 μs |  0.970 μs |  3.4180 |      - |  57.62 KB |
| GetPaged_ComplexGraph             | InMemory   |    96.42 μs | 24.993 μs | 23.378 μs |  4.8828 |      - |  87.22 KB |
| GetPaged_FilterByNavLevel2        | InMemory   |    83.04 μs |  5.143 μs |  4.015 μs |  9.2773 | 0.4883 | 157.93 KB |
| GetPaged_SortByNavLevel2          | InMemory   |   112.30 μs |  7.505 μs |  7.020 μs | 13.6719 | 0.9766 | 226.74 KB |
| GetPaged_FilterAndSortByNavLevel2 | InMemory   |   215.02 μs | 20.261 μs | 17.961 μs | 26.3672 | 2.9297 | 442.54 KB |
| GetPaged_Review_FilterByNavLevel3 | InMemory   |   176.95 μs |  6.560 μs |  5.121 μs | 29.2969 | 3.9063 | 491.18 KB |
| GetPaged_Review_SortByNavLevel3   | InMemory   |   269.16 μs | 17.667 μs | 15.661 μs | 47.8516 | 9.7656 | 796.49 KB |
| **GetPaged_Simple**                   | **PostgreSQL** |   **955.36 μs** |  **9.825 μs** |  **8.709 μs** |       **-** |      **-** |  **17.29 KB** |
| GetPaged_SimpleWithFilter         | PostgreSQL |   987.69 μs | 26.710 μs | 23.678 μs |       - |      - |  21.31 KB |
| GetPaged_SimpleWithSort           | PostgreSQL | 1,020.49 μs | 14.886 μs | 13.196 μs |       - |      - |  23.82 KB |
| GetPaged_ComplexFilter            | PostgreSQL | 1,065.29 μs | 13.120 μs | 11.630 μs |  1.9531 |      - |  32.78 KB |
| GetPaged_ComplexSort              | PostgreSQL | 1,051.01 μs |  8.560 μs |  7.148 μs |  1.9531 |      - |  36.04 KB |
| GetPaged_ComplexFull              | PostgreSQL | 1,065.32 μs |  7.129 μs |  6.320 μs |  1.9531 |      - |  39.49 KB |
| GetPaged_SimpleGraph              | PostgreSQL | 1,085.31 μs | 15.029 μs | 14.058 μs |       - |      - |  23.05 KB |
| GetPaged_ComplexGraph             | PostgreSQL | 1,230.56 μs | 37.366 μs | 33.124 μs |  1.9531 |      - |  38.06 KB |
| GetPaged_FilterByNavLevel2        | PostgreSQL | 1,205.69 μs | 32.088 μs | 30.015 μs |       - |      - |  28.78 KB |
| GetPaged_SortByNavLevel2          | PostgreSQL | 1,259.04 μs | 28.762 μs | 26.904 μs |  1.9531 |      - |  35.53 KB |
| GetPaged_FilterAndSortByNavLevel2 | PostgreSQL | 1,407.30 μs | 27.144 μs | 24.063 μs |  1.9531 |      - |  46.59 KB |
| GetPaged_Review_FilterByNavLevel3 | PostgreSQL | 1,345.42 μs | 47.927 μs | 44.831 μs |       - |      - |  31.34 KB |
| GetPaged_Review_SortByNavLevel3   | PostgreSQL | 1,446.14 μs | 63.667 μs | 56.440 μs |  1.9531 |      - |  38.23 KB |
