```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-NTRUNJ : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=5  WarmupCount=3  

```
| Method                | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| ParseSimpleFilter     | 117.94 ns |  7.079 ns |  1.838 ns |  8.90 |    0.55 | 0.0153 |     256 B |        2.91 |
| ParseComplexFilter    | 411.83 ns | 55.440 ns | 14.398 ns | 31.07 |    2.12 | 0.0520 |     872 B |        9.91 |
| ParseSimpleSort       |  44.64 ns | 20.125 ns |  5.226 ns |  3.37 |    0.41 | 0.0095 |     160 B |        1.82 |
| ParseComplexSort      |  97.14 ns | 14.493 ns |  2.243 ns |  7.33 |    0.47 | 0.0191 |     320 B |        3.64 |
| ParseSimpleGraph      | 126.49 ns | 13.010 ns |  3.379 ns |  9.54 |    0.62 | 0.0172 |     288 B |        3.27 |
| ParseComplexGraph     | 252.00 ns | 23.806 ns |  3.684 ns | 19.01 |    1.18 | 0.0353 |     592 B |        6.73 |
| ParseUrlEncodedFilter | 206.53 ns | 20.446 ns |  5.310 ns | 15.58 |    1.01 | 0.0291 |     488 B |        5.55 |
| ParseAllParameters    | 703.18 ns | 59.110 ns | 15.351 ns | 53.05 |    3.38 | 0.0954 |    1608 B |       18.27 |
| ParseMinimal          |  13.30 ns |  3.588 ns |  0.932 ns |  1.00 |    0.09 | 0.0053 |      88 B |        1.00 |
