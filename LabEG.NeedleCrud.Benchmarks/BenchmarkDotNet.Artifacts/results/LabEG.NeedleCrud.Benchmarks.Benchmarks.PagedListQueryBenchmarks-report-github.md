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
| ParseSimpleFilter     |  83.87 ns |  1.154 ns |  1.023 ns |  6.66 |    0.14 | 0.0153 |     256 B |        2.91 |
| ParseComplexFilter    | 346.98 ns |  3.955 ns |  3.302 ns | 27.54 |    0.56 | 0.0520 |     872 B |        9.91 |
| ParseSimpleSort       |  32.02 ns |  0.694 ns |  0.616 ns |  2.54 |    0.07 | 0.0095 |     160 B |        1.82 |
| ParseComplexSort      |  87.07 ns |  2.684 ns |  2.510 ns |  6.91 |    0.23 | 0.0191 |     320 B |        3.64 |
| ParseSimpleGraph      | 151.86 ns |  1.717 ns |  1.606 ns | 12.05 |    0.25 | 0.0200 |     336 B |        3.82 |
| ParseComplexGraph     | 288.55 ns |  7.184 ns |  6.369 ns | 22.90 |    0.64 | 0.0305 |     512 B |        5.82 |
| ParseUrlEncodedFilter | 204.07 ns |  3.516 ns |  3.289 ns | 16.20 |    0.39 | 0.0291 |     488 B |        5.55 |
| ParseAllParameters    | 709.58 ns | 16.228 ns | 14.386 ns | 56.32 |    1.50 | 0.0906 |    1528 B |       17.36 |
| ParseMinimal          |  12.60 ns |  0.251 ns |  0.235 ns |  1.00 |    0.03 | 0.0053 |      88 B |        1.00 |
