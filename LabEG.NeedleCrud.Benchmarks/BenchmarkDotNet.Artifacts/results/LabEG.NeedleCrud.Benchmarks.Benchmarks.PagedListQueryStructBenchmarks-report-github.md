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
| CreateSimpleFilter     |  60.11 ns | 1.007 ns | 0.942 ns | 0.0076 |     128 B |
| CreateComplexFilter    |  90.14 ns | 1.860 ns | 1.740 ns | 0.0119 |     200 B |
| CreateSimpleSort       |  11.31 ns | 0.170 ns | 0.159 ns | 0.0019 |      32 B |
| CreateComplexSort      |  12.07 ns | 0.237 ns | 0.210 ns | 0.0038 |      64 B |
| AccessFilterProperties | 112.31 ns | 3.233 ns | 3.024 ns | 0.0186 |     312 B |
| AccessSortProperties   |  25.91 ns | 0.597 ns | 0.529 ns | 0.0095 |     160 B |
| CreateMultipleFilters  | 321.36 ns | 2.088 ns | 1.851 ns | 0.0486 |     816 B |
| CreateMultipleSorts    |  35.07 ns | 0.224 ns | 0.210 ns | 0.0105 |     176 B |
