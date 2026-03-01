```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-AGBPQC : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=15  WarmupCount=3  

```
| Method                | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
|---------------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
| ParseSimpleFilter     |  77.76 ns |  1.531 ns |  1.278 ns |  6.38 |    0.24 | 0.0153 |     256 B |        2.91 |
| ParseComplexFilter    | 370.99 ns |  3.902 ns |  3.047 ns | 30.42 |    1.06 | 0.0520 |     872 B |        9.91 |
| ParseSimpleSort       |  30.77 ns |  0.345 ns |  0.288 ns |  2.52 |    0.09 | 0.0095 |     160 B |        1.82 |
| ParseComplexSort      |  87.32 ns |  1.173 ns |  1.097 ns |  7.16 |    0.26 | 0.0191 |     320 B |        3.64 |
| ParseSimpleGraph      | 142.98 ns |  2.937 ns |  2.748 ns | 11.72 |    0.46 | 0.0200 |     336 B |        3.82 |
| ParseComplexGraph     | 259.97 ns |  7.205 ns |  6.740 ns | 21.32 |    0.90 | 0.0305 |     512 B |        5.82 |
| ParseUrlEncodedFilter | 194.32 ns |  2.526 ns |  2.363 ns | 15.93 |    0.57 | 0.0291 |     488 B |        5.55 |
| ParseAllParameters    | 690.78 ns | 21.557 ns | 20.165 ns | 56.64 |    2.51 | 0.0906 |    1528 B |       17.36 |
| ParseMinimal          |  12.21 ns |  0.466 ns |  0.436 ns |  1.00 |    0.05 | 0.0053 |      88 B |        1.00 |
