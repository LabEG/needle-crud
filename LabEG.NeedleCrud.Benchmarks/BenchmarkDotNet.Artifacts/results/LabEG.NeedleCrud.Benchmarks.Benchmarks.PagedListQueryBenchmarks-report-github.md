```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-NTRUNJ : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=5  WarmupCount=3  

```
| Method                | Mean      | Error     | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
| ParseSimpleFilter     |  77.35 ns |  2.953 ns | 0.457 ns |  6.55 |    0.10 | 0.0153 |     256 B |        2.91 |
| ParseComplexFilter    | 432.72 ns |  8.543 ns | 2.218 ns | 36.66 |    0.54 | 0.0520 |     872 B |        9.91 |
| ParseSimpleSort       |  35.14 ns |  9.330 ns | 2.423 ns |  2.98 |    0.19 | 0.0095 |     160 B |        1.82 |
| ParseComplexSort      |  81.96 ns |  7.057 ns | 1.833 ns |  6.94 |    0.17 | 0.0191 |     320 B |        3.64 |
| ParseSimpleGraph      | 152.14 ns | 14.508 ns | 3.768 ns | 12.89 |    0.34 | 0.0200 |     336 B |        3.82 |
| ParseComplexGraph     | 253.40 ns |  5.434 ns | 1.411 ns | 21.47 |    0.32 | 0.0305 |     512 B |        5.82 |
| ParseUrlEncodedFilter | 196.27 ns |  4.859 ns | 0.752 ns | 16.63 |    0.24 | 0.0291 |     488 B |        5.55 |
| ParseAllParameters    | 668.95 ns | 20.884 ns | 3.232 ns | 56.67 |    0.83 | 0.0906 |    1528 B |       17.36 |
| ParseMinimal          |  11.81 ns |  0.692 ns | 0.180 ns |  1.00 |    0.02 | 0.0053 |      88 B |        1.00 |
