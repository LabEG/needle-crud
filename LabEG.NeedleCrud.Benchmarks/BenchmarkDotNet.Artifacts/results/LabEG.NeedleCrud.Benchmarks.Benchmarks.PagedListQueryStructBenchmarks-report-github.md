```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3


```
| Method                 | Mean      | Error    | StdDev   | Gen0   | Allocated |
|----------------------- |----------:|---------:|---------:|-------:|----------:|
| CreateSimpleFilter     |  61.66 ns | 1.240 ns | 1.476 ns | 0.0076 |     128 B |
| CreateComplexFilter    |  95.92 ns | 1.941 ns | 2.157 ns | 0.0119 |     200 B |
| CreateSimpleSort       |  11.80 ns | 0.251 ns | 0.223 ns | 0.0019 |      32 B |
| CreateComplexSort      |  12.37 ns | 0.244 ns | 0.240 ns | 0.0038 |      64 B |
| AccessFilterProperties | 113.92 ns | 2.294 ns | 3.062 ns | 0.0186 |     312 B |
| AccessSortProperties   |  28.20 ns | 0.496 ns | 0.464 ns | 0.0095 |     160 B |
| CreateMultipleFilters  | 338.10 ns | 6.592 ns | 7.847 ns | 0.0486 |     816 B |
| CreateMultipleSorts    |  37.79 ns | 0.780 ns | 0.898 ns | 0.0105 |     176 B |
