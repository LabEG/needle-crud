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
| **GetPaged_Simple**                   | **InMemory**   |    **16.01 μs** |  **0.256 μs** |  **0.240 μs** |  **2.3193** | **0.0916** |  **38.05 KB** |
| GetPaged_SimpleWithFilter         | InMemory   |    25.54 μs |  1.174 μs |  1.098 μs |  2.5635 | 0.1221 |  42.31 KB |
| GetPaged_SimpleWithSort           | InMemory   |    32.84 μs |  0.498 μs |  0.466 μs |  2.8687 | 0.1221 |     47 KB |
| GetPaged_ComplexFilter            | InMemory   |    45.61 μs |  2.403 μs |  1.876 μs |  2.9297 |      - |  54.39 KB |
| GetPaged_ComplexSort              | InMemory   |    47.50 μs |  1.800 μs |  1.596 μs |  3.6621 |      - |  61.21 KB |
| GetPaged_ComplexFull              | InMemory   |    57.36 μs |  2.786 μs |  2.326 μs |  3.4180 |      - |  60.54 KB |
| GetPaged_SimpleGraph              | InMemory   |    32.69 μs |  0.900 μs |  0.798 μs |  3.4180 |      - |  57.62 KB |
| GetPaged_ComplexGraph             | InMemory   |    75.53 μs |  6.411 μs |  5.353 μs |  4.8828 |      - |  87.22 KB |
| GetPaged_FilterByNavLevel2        | InMemory   |   110.59 μs |  6.551 μs |  6.128 μs | 10.2539 | 0.4883 | 171.06 KB |
| GetPaged_SortByNavLevel2          | InMemory   |   108.16 μs |  6.364 μs |  5.642 μs | 13.6719 | 1.4648 | 227.21 KB |
| GetPaged_FilterAndSortByNavLevel2 | InMemory   |   250.14 μs | 10.599 μs |  8.851 μs | 28.3203 | 2.9297 | 465.87 KB |
| GetPaged_Review_FilterByNavLevel3 | InMemory   |   215.63 μs |  4.367 μs |  3.409 μs | 30.2734 | 3.9063 | 509.66 KB |
| GetPaged_Review_SortByNavLevel3   | InMemory   |   245.64 μs |  3.387 μs |  3.002 μs | 47.8516 | 9.7656 | 796.49 KB |
| **GetPaged_Simple**                   | **PostgreSQL** |   **914.02 μs** | **13.565 μs** | **12.025 μs** |       **-** |      **-** |  **17.31 KB** |
| GetPaged_SimpleWithFilter         | PostgreSQL |   943.39 μs |  6.996 μs |  5.842 μs |       - |      - |  21.32 KB |
| GetPaged_SimpleWithSort           | PostgreSQL |   959.09 μs |  4.696 μs |  3.667 μs |       - |      - |  23.84 KB |
| GetPaged_ComplexFilter            | PostgreSQL | 1,042.42 μs |  9.925 μs |  8.288 μs |  1.9531 |      - |  32.79 KB |
| GetPaged_ComplexSort              | PostgreSQL | 1,060.01 μs | 18.943 μs | 16.793 μs |  1.9531 |      - |  36.01 KB |
| GetPaged_ComplexFull              | PostgreSQL | 1,054.54 μs | 10.931 μs |  9.690 μs |  1.9531 |      - |  39.37 KB |
| GetPaged_SimpleGraph              | PostgreSQL | 1,071.93 μs |  7.461 μs |  6.230 μs |       - |      - |  23.06 KB |
| GetPaged_ComplexGraph             | PostgreSQL | 1,202.99 μs |  6.114 μs |  5.420 μs |  1.9531 |      - |  38.08 KB |
| GetPaged_FilterByNavLevel2        | PostgreSQL | 1,148.89 μs | 16.270 μs | 13.586 μs |       - |      - |  28.88 KB |
| GetPaged_SortByNavLevel2          | PostgreSQL | 1,198.24 μs | 17.029 μs | 15.096 μs |  1.9531 |      - |  35.67 KB |
| GetPaged_FilterAndSortByNavLevel2 | PostgreSQL | 1,369.88 μs |  9.933 μs |  8.805 μs |  1.9531 |      - |  46.34 KB |
| GetPaged_Review_FilterByNavLevel3 | PostgreSQL | 1,303.10 μs | 23.390 μs | 20.735 μs |       - |      - |  31.56 KB |
| GetPaged_Review_SortByNavLevel3   | PostgreSQL | 1,489.13 μs | 23.224 μs | 21.724 μs |  1.9531 |      - |  38.23 KB |
