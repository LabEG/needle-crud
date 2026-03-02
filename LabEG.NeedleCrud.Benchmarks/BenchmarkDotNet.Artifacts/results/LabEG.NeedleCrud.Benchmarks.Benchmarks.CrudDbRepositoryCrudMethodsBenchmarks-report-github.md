```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-KQXXMH : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=15  UnrollFactor=1  
WarmupCount=3  

```
| Method  | Provider   | Mean       | Error     | StdDev    | Allocated |
|-------- |----------- |-----------:|----------:|----------:|----------:|
| **Create**  | **InMemory**   |   **193.3 μs** |  **27.48 μs** |  **22.94 μs** |   **5.27 KB** |
| GetById | InMemory   |   264.9 μs |  42.15 μs |  37.36 μs |  21.78 KB |
| GetAll  | InMemory   | 1,022.9 μs | 231.64 μs | 216.68 μs | 203.05 KB |
| Update  | InMemory   |   360.6 μs |  78.70 μs |  73.62 μs |   24.3 KB |
| Delete  | InMemory   |   356.5 μs |  67.29 μs |  62.95 μs |  26.42 KB |
| **Create**  | **PostgreSQL** | **1,914.2 μs** | **189.86 μs** | **168.31 μs** |   **24.8 KB** |
| GetById | PostgreSQL | 1,323.0 μs | 182.07 μs | 152.04 μs |  14.03 KB |
| GetAll  | PostgreSQL | 2,030.0 μs | 775.33 μs | 725.24 μs | 192.95 KB |
| Update  | PostgreSQL | 3,106.0 μs | 172.72 μs | 144.23 μs |  37.99 KB |
| Delete  | PostgreSQL | 2,638.9 μs | 188.29 μs | 166.92 μs |  27.79 KB |
