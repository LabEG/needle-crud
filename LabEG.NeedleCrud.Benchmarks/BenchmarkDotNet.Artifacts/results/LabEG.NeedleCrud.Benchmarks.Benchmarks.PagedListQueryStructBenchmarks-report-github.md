```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3


```
| Method                 | Mean      | Error    | StdDev   | Gen0   | Allocated |
|----------------------- |----------:|---------:|---------:|-------:|----------:|
| CreateSimpleFilter     |  62.08 ns | 1.223 ns | 1.547 ns | 0.0076 |     128 B |
| CreateComplexFilter    |  89.62 ns | 0.909 ns | 0.806 ns | 0.0119 |     200 B |
| CreateSimpleSort       |  11.08 ns | 0.235 ns | 0.220 ns | 0.0019 |      32 B |
| CreateComplexSort      |  12.13 ns | 0.261 ns | 0.321 ns | 0.0038 |      64 B |
| AccessFilterProperties | 112.58 ns | 2.303 ns | 3.303 ns | 0.0186 |     312 B |
| AccessSortProperties   |  27.42 ns | 0.583 ns | 1.020 ns | 0.0095 |     160 B |
| CreateMultipleFilters  | 316.87 ns | 3.500 ns | 3.273 ns | 0.0486 |     816 B |
| CreateMultipleSorts    |  36.23 ns | 0.339 ns | 0.317 ns | 0.0105 |     176 B |
