```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-AGBPQC : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

IterationCount=15  WarmupCount=3  

```
| Method                 | Mean      | Error    | StdDev   | Gen0   | Allocated |
|----------------------- |----------:|---------:|---------:|-------:|----------:|
| CreateSimpleFilter     |  57.65 ns | 0.710 ns | 0.664 ns | 0.0076 |     128 B |
| CreateComplexFilter    |  87.03 ns | 1.028 ns | 0.962 ns | 0.0119 |     200 B |
| CreateSimpleSort       |  10.97 ns | 0.287 ns | 0.268 ns | 0.0019 |      32 B |
| CreateComplexSort      |  11.64 ns | 0.148 ns | 0.131 ns | 0.0038 |      64 B |
| AccessFilterProperties | 123.65 ns | 1.424 ns | 1.262 ns | 0.0186 |     312 B |
| AccessSortProperties   |  30.17 ns | 2.629 ns | 2.459 ns | 0.0095 |     160 B |
| CreateMultipleFilters  | 346.04 ns | 8.944 ns | 8.366 ns | 0.0486 |     816 B |
| CreateMultipleSorts    |  37.16 ns | 0.391 ns | 0.346 ns | 0.0105 |     176 B |
