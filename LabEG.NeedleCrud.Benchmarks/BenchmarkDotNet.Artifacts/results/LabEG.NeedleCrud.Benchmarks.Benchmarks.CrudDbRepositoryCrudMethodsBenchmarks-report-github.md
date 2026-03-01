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
| **Create**  | **InMemory**   |   **191.6 μs** |   **146.92 μs** |  **38.15 μs** |   **5.27 KB** |
| GetById | InMemory   |   226.9 μs |    84.94 μs |  22.06 μs |  21.78 KB |
| GetAll  | InMemory   |   885.2 μs |   527.98 μs | 137.12 μs | 203.05 KB |
| Update  | InMemory   |   325.6 μs |   222.98 μs |  34.51 μs |   24.3 KB |
| Delete  | InMemory   |   386.8 μs |   204.81 μs |  53.19 μs |  26.42 KB |
| **Create**  | **PostgreSQL** | **1,999.9 μs** |   **599.09 μs** | **155.58 μs** |  **24.88 KB** |
| GetById | PostgreSQL | 1,341.6 μs |   781.34 μs | 120.91 μs |  14.11 KB |
| GetAll  | PostgreSQL | 2,695.6 μs | 2,168.90 μs | 335.64 μs | 200.77 KB |
| Update  | PostgreSQL | 3,048.8 μs |   685.14 μs | 177.93 μs |  38.68 KB |
| Delete  | PostgreSQL | 2,873.3 μs |   947.22 μs | 146.58 μs |  28.81 KB |
