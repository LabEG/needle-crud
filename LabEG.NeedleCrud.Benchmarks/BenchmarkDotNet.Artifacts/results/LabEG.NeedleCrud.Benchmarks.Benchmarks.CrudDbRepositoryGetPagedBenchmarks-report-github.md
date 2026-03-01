```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-MEHJPP : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=5  WarmupCount=1  

```
| Method                    | Provider   | Mean        | Error      | StdDev    | Gen0   | Gen1   | Allocated |
|-------------------------- |----------- |------------:|-----------:|----------:|-------:|-------:|----------:|
| **GetPaged_Simple**           | **InMemory**   |    **15.89 μs** |   **3.444 μs** |  **0.894 μs** | **2.3193** | **0.0916** |  **38.05 KB** |
| GetPaged_SimpleWithFilter | InMemory   |    24.11 μs |   1.030 μs |  0.159 μs | 2.5635 | 0.1221 |  42.31 KB |
| GetPaged_SimpleWithSort   | InMemory   |    31.85 μs |   2.224 μs |  0.578 μs | 2.8687 | 0.1221 |     47 KB |
| GetPaged_ComplexFilter    | InMemory   |    47.62 μs |  40.091 μs |  6.204 μs | 3.1738 |      - |  54.39 KB |
| GetPaged_ComplexSort      | InMemory   |    48.01 μs |  28.831 μs |  4.462 μs | 3.6621 |      - |  61.69 KB |
| GetPaged_ComplexFull      | InMemory   |    94.27 μs | 124.014 μs | 32.206 μs | 3.4180 |      - |  60.45 KB |
| GetPaged_SimpleGraph      | InMemory   |    46.08 μs |  54.940 μs | 14.268 μs | 3.4180 |      - |  57.62 KB |
| GetPaged_ComplexGraph     | InMemory   |   154.37 μs | 239.844 μs | 62.287 μs | 4.8828 |      - |  87.22 KB |
| **GetPaged_Simple**           | **PostgreSQL** |   **978.32 μs** | **241.293 μs** | **62.663 μs** |      **-** |      **-** |   **17.3 KB** |
| GetPaged_SimpleWithFilter | PostgreSQL | 1,089.43 μs | 333.021 μs | 86.484 μs |      - |      - |  21.31 KB |
| GetPaged_SimpleWithSort   | PostgreSQL | 1,100.31 μs | 282.693 μs | 73.415 μs |      - |      - |  23.82 KB |
| GetPaged_ComplexFilter    | PostgreSQL | 1,142.62 μs | 361.473 μs | 93.873 μs | 1.9531 |      - |  32.78 KB |
| GetPaged_ComplexSort      | PostgreSQL | 1,104.98 μs | 334.800 μs | 86.946 μs | 1.9531 |      - |  36.21 KB |
| GetPaged_ComplexFull      | PostgreSQL | 1,149.29 μs | 332.133 μs | 86.254 μs | 1.9531 |      - |  39.37 KB |
| GetPaged_SimpleGraph      | PostgreSQL | 1,164.88 μs | 273.798 μs | 71.105 μs |      - |      - |  23.04 KB |
| GetPaged_ComplexGraph     | PostgreSQL | 1,316.16 μs | 371.276 μs | 96.419 μs | 1.9531 |      - |  38.05 KB |
