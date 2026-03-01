```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-NZVBQK : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=5  UnrollFactor=1  
WarmupCount=1  

```
| Method  | Provider   | Mean       | Error       | StdDev    | Allocated |
|-------- |----------- |-----------:|------------:|----------:|----------:|
| **Create**  | **InMemory**   |   **181.6 μs** |   **188.69 μs** |  **49.00 μs** |   **5.27 KB** |
| GetById | InMemory   |   237.6 μs |    69.00 μs |  10.68 μs |  21.78 KB |
| GetAll  | InMemory   |   794.4 μs |   158.30 μs |  24.50 μs | 203.05 KB |
| Update  | InMemory   |   327.4 μs |   166.91 μs |  43.35 μs |   24.3 KB |
| Delete  | InMemory   |   317.8 μs |   143.96 μs |  37.39 μs |  26.42 KB |
| **Create**  | **PostgreSQL** | **2,211.9 μs** |   **517.15 μs** |  **80.03 μs** |   **24.7 KB** |
| GetById | PostgreSQL | 1,353.1 μs |   199.58 μs |  30.89 μs |  14.11 KB |
| GetAll  | PostgreSQL | 2,980.1 μs | 1,882.42 μs | 488.86 μs | 200.77 KB |
| Update  | PostgreSQL | 3,113.8 μs | 1,653.76 μs | 429.48 μs |  38.68 KB |
| Delete  | PostgreSQL | 3,131.0 μs |   806.61 μs | 209.47 μs |  28.81 KB |
