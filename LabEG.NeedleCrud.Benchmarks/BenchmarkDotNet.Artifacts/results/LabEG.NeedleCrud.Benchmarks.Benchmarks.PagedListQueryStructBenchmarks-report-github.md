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
| CreateSimpleFilter     |  60.32 ns | 0.584 ns | 0.546 ns | 0.0076 |     128 B |
| CreateComplexFilter    |  88.08 ns | 0.644 ns | 0.602 ns | 0.0119 |     200 B |
| CreateSimpleSort       |  11.39 ns | 0.303 ns | 0.284 ns | 0.0019 |      32 B |
| CreateComplexSort      |  12.48 ns | 0.429 ns | 0.402 ns | 0.0038 |      64 B |
| AccessFilterProperties | 107.47 ns | 1.454 ns | 1.360 ns | 0.0186 |     312 B |
| AccessSortProperties   |  28.57 ns | 1.023 ns | 0.854 ns | 0.0095 |     160 B |
| CreateMultipleFilters  | 326.91 ns | 3.346 ns | 2.966 ns | 0.0486 |     816 B |
| CreateMultipleSorts    |  37.86 ns | 1.182 ns | 1.106 ns | 0.0105 |     176 B |
