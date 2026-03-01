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
| ParseSimpleFilter     |  93.88 ns |  2.759 ns | 0.717 ns |  7.34 |    0.21 | 0.0153 |     256 B |        2.91 |
| ParseComplexFilter    | 373.01 ns | 20.669 ns | 3.199 ns | 29.18 |    0.84 | 0.0520 |     872 B |        9.91 |
| ParseSimpleSort       |  32.48 ns |  2.724 ns | 0.708 ns |  2.54 |    0.09 | 0.0095 |     160 B |        1.82 |
| ParseComplexSort      |  81.11 ns |  4.206 ns | 1.092 ns |  6.34 |    0.19 | 0.0191 |     320 B |        3.64 |
| ParseSimpleGraph      | 151.26 ns |  8.854 ns | 2.299 ns | 11.83 |    0.37 | 0.0200 |     336 B |        3.82 |
| ParseComplexGraph     | 266.09 ns | 28.236 ns | 7.333 ns | 20.82 |    0.78 | 0.0305 |     512 B |        5.82 |
| ParseUrlEncodedFilter | 190.75 ns | 11.783 ns | 1.823 ns | 14.92 |    0.43 | 0.0291 |     488 B |        5.55 |
| ParseAllParameters    | 698.55 ns | 32.499 ns | 8.440 ns | 54.65 |    1.63 | 0.0906 |    1528 B |       17.36 |
| ParseMinimal          |  12.79 ns |  1.503 ns | 0.390 ns |  1.00 |    0.04 | 0.0053 |      88 B |        1.00 |
