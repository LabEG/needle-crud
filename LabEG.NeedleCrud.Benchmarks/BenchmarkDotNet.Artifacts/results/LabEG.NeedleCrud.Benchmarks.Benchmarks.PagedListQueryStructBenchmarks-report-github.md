```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3


```
| Method                 | Mean      | Error    | StdDev    | Gen0   | Allocated |
|----------------------- |----------:|---------:|----------:|-------:|----------:|
| CreateSimpleFilter     |  60.45 ns | 0.683 ns |  0.638 ns | 0.0076 |     128 B |
| CreateComplexFilter    |  89.50 ns | 1.810 ns |  1.859 ns | 0.0119 |     200 B |
| CreateSimpleSort       |  11.28 ns | 0.152 ns |  0.142 ns | 0.0019 |      32 B |
| CreateComplexSort      |  12.12 ns | 0.262 ns |  0.473 ns | 0.0038 |      64 B |
| AccessFilterProperties | 108.21 ns | 1.095 ns |  1.024 ns | 0.0186 |     312 B |
| AccessSortProperties   |  27.42 ns | 0.593 ns |  1.237 ns | 0.0095 |     160 B |
| CreateMultipleFilters  | 341.05 ns | 6.879 ns | 14.808 ns | 0.0486 |     816 B |
| CreateMultipleSorts    |  38.78 ns | 0.762 ns |  0.713 ns | 0.0105 |     176 B |
