```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-AGBPQC : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=15  WarmupCount=3  

```
| Method                | Mean      | Error    | StdDev   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |----------:|---------:|---------:|------:|--------:|-------:|----------:|------------:|
| ParseSimpleFilter     |  84.04 ns | 2.198 ns | 2.056 ns |  7.08 |    0.18 | 0.0153 |     256 B |        2.91 |
| ParseComplexFilter    | 404.05 ns | 3.338 ns | 2.959 ns | 34.05 |    0.40 | 0.0520 |     872 B |        9.91 |
| ParseSimpleSort       |  31.74 ns | 2.162 ns | 1.805 ns |  2.68 |    0.15 | 0.0095 |     160 B |        1.82 |
| ParseComplexSort      |  92.18 ns | 9.078 ns | 8.492 ns |  7.77 |    0.70 | 0.0191 |     320 B |        3.64 |
| ParseSimpleGraph      | 151.54 ns | 6.590 ns | 6.164 ns | 12.77 |    0.52 | 0.0200 |     336 B |        3.82 |
| ParseComplexGraph     | 253.76 ns | 4.974 ns | 4.653 ns | 21.39 |    0.43 | 0.0305 |     512 B |        5.82 |
| ParseUrlEncodedFilter | 199.23 ns | 1.770 ns | 1.656 ns | 16.79 |    0.21 | 0.0291 |     488 B |        5.55 |
| ParseAllParameters    | 697.71 ns | 9.080 ns | 8.049 ns | 58.81 |    0.86 | 0.0906 |    1528 B |       17.36 |
| ParseMinimal          |  11.87 ns | 0.124 ns | 0.116 ns |  1.00 |    0.01 | 0.0053 |      88 B |        1.00 |
