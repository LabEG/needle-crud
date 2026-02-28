```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-NZVBQK : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=5  UnrollFactor=1  
WarmupCount=1  

```
| Method  | Provider   | Mean       | Error       | StdDev      | Allocated |
|-------- |----------- |-----------:|------------:|------------:|----------:|
| **Create**  | **InMemory**   |   **294.4 μs** |    **383.8 μs** |    **99.66 μs** |   **5.27 KB** |
| GetById | InMemory   |   406.9 μs |    506.2 μs |   131.45 μs |  22.73 KB |
| GetAll  | InMemory   | 1,007.3 μs |    807.7 μs |   209.75 μs | 203.99 KB |
| Update  | InMemory   |   355.8 μs |    186.2 μs |    48.36 μs |  25.25 KB |
| Delete  | InMemory   |   345.6 μs |    222.5 μs |    57.79 μs |  27.37 KB |
| **Create**  | **PostgreSQL** | **8,598.5 μs** | **11,709.5 μs** | **1,812.07 μs** |  **25.02 KB** |
| GetById | PostgreSQL | 1,758.3 μs |    576.2 μs |   149.63 μs |  14.76 KB |
| GetAll  | PostgreSQL | 2,760.2 μs |  3,382.4 μs |   523.44 μs | 201.23 KB |
| Update  | PostgreSQL | 9,028.2 μs | 16,584.6 μs | 4,306.98 μs |   39.4 KB |
| Delete  | PostgreSQL | 8,485.0 μs | 17,753.5 μs | 4,610.54 μs |  29.46 KB |
