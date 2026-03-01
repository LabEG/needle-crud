```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-KQXXMH : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=15  UnrollFactor=1  
WarmupCount=3  

```
| Method                                   | Provider   | Mean           | Error         | StdDev        | Median         | Allocated |
|----------------------------------------- |----------- |---------------:|--------------:|--------------:|---------------:|----------:|
| **AddFilter_NoFilters**                      | **InMemory**   |     **2,560.0 ns** |   **1,109.38 ns** |   **1,037.72 ns** |     **2,200.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | InMemory   |    34,757.1 ns |  13,080.97 ns |  11,595.94 ns |    30,100.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | InMemory   |    40,914.3 ns |   9,405.22 ns |   8,337.48 ns |    39,150.0 ns |    5768 B |
| AddFilter_ComplexFull                    | InMemory   |    46,528.6 ns |  14,268.50 ns |  12,648.65 ns |    41,150.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | InMemory   |    39,706.7 ns |  13,934.13 ns |  13,033.99 ns |    40,200.0 ns |    1888 B |
| AddSort_NoSort                           | InMemory   |     1,921.4 ns |     635.33 ns |     563.20 ns |     1,700.0 ns |      88 B |
| AddSort_SimpleWithSort                   | InMemory   |    49,573.3 ns |  10,510.36 ns |   9,831.40 ns |    46,900.0 ns |    5440 B |
| AddSort_ComplexSort                      | InMemory   |    60,466.7 ns |  13,277.50 ns |  12,419.78 ns |    62,100.0 ns |   14480 B |
| AddSort_ComplexFull                      | InMemory   |    57,966.7 ns |  13,449.16 ns |  12,580.35 ns |    59,200.0 ns |   10816 B |
| AddSort_ComplexGraph                     | InMemory   |    52,313.3 ns |  12,632.95 ns |  11,816.87 ns |    49,900.0 ns |    6008 B |
| ExtractIncludes_SimpleGraph              | InMemory   |    15,913.3 ns |   6,017.73 ns |   5,628.99 ns |    15,400.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | InMemory   |    17,706.7 ns |   4,552.43 ns |   4,258.35 ns |    16,000.0 ns |    1240 B |
| GetMemberExpression_Simple               | InMemory   |     9,235.7 ns |   3,116.34 ns |   2,762.55 ns |     8,550.0 ns |     160 B |
| GetMemberExpression_Nested               | InMemory   |    14,142.9 ns |   5,939.68 ns |   5,265.38 ns |    13,900.0 ns |     376 B |
| ToCamelCase_Short                        | InMemory   |     1,835.7 ns |     414.14 ns |     367.12 ns |     1,900.0 ns |      32 B |
| ToCamelCase_Long                         | InMemory   |     1,525.0 ns |     205.26 ns |     160.26 ns |     1,500.0 ns |      56 B |
| ToType_Int                               | InMemory   |     1,820.0 ns |     752.91 ns |     704.27 ns |     1,700.0 ns |      24 B |
| ToType_Bool                              | InMemory   |     1,678.6 ns |     964.51 ns |     855.01 ns |     1,600.0 ns |      24 B |
| ToType_DateTime                          | InMemory   |     3,785.7 ns |     842.84 ns |     747.16 ns |     3,900.0 ns |      24 B |
| ToType_String                            | InMemory   |       171.4 ns |     135.86 ns |     120.44 ns |       100.0 ns |         - |
| Combined_Simple_Count                    | InMemory   |    72,864.3 ns |  22,871.36 ns |  20,274.87 ns |    68,100.0 ns |   15712 B |
| Combined_SimpleWithFilter_FilterAndCount | InMemory   |   114,740.0 ns |  32,166.97 ns |  30,089.01 ns |   102,900.0 ns |   19320 B |
| Combined_SimpleWithSort_SortAndCount     | InMemory   |   114,846.7 ns |  34,292.64 ns |  32,077.36 ns |   111,100.0 ns |   22520 B |
| Combined_ComplexFilter_FilterAndCount    | InMemory   |   157,900.0 ns |  31,814.24 ns |  29,759.06 ns |   144,900.0 ns |   28968 B |
| Combined_ComplexSort_SortAndCount        | InMemory   |   154,173.3 ns |  28,306.45 ns |  26,477.87 ns |   151,900.0 ns |   33864 B |
| Combined_ComplexFull_FilterSortAndCount  | InMemory   |   205,420.0 ns |  35,615.20 ns |  33,314.48 ns |   213,700.0 ns |   39616 B |
| Combined_ComplexGraph_FilterSortAndCount | InMemory   |   169,280.0 ns |  46,845.60 ns |  43,819.41 ns |   162,300.0 ns |   26536 B |
| **AddFilter_NoFilters**                      | **PostgreSQL** |     **2,671.4 ns** |     **755.34 ns** |     **669.59 ns** |     **2,500.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | PostgreSQL |    40,033.3 ns |  11,207.21 ns |  10,483.23 ns |    41,600.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | PostgreSQL |    80,885.7 ns |  22,652.65 ns |  20,080.98 ns |    79,100.0 ns |    5768 B |
| AddFilter_ComplexFull                    | PostgreSQL |    72,446.7 ns |  27,562.84 ns |  25,782.30 ns |    61,100.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | PostgreSQL |    57,350.0 ns |  14,853.58 ns |  13,167.31 ns |    55,500.0 ns |    1888 B |
| AddSort_NoSort                           | PostgreSQL |     2,614.3 ns |     590.08 ns |     523.09 ns |     2,650.0 ns |      88 B |
| AddSort_SimpleWithSort                   | PostgreSQL |    72,180.0 ns |   8,886.46 ns |   8,312.40 ns |    72,100.0 ns |    5440 B |
| AddSort_ComplexSort                      | PostgreSQL |   104,700.0 ns |  25,429.75 ns |  23,787.00 ns |   104,600.0 ns |   14480 B |
| AddSort_ComplexFull                      | PostgreSQL |    57,820.0 ns |  12,747.92 ns |  11,924.42 ns |    57,400.0 ns |   10816 B |
| AddSort_ComplexGraph                     | PostgreSQL |    90,278.6 ns |  21,479.95 ns |  19,041.42 ns |    91,550.0 ns |    6008 B |
| ExtractIncludes_SimpleGraph              | PostgreSQL |    23,720.0 ns |   9,051.48 ns |   8,466.76 ns |    27,000.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | PostgreSQL |    35,407.1 ns |  10,429.89 ns |   9,245.83 ns |    37,250.0 ns |    1240 B |
| GetMemberExpression_Simple               | PostgreSQL |    23,821.4 ns |   5,679.66 ns |   5,034.87 ns |    24,200.0 ns |     160 B |
| GetMemberExpression_Nested               | PostgreSQL |    22,020.0 ns |   5,792.44 ns |   5,418.25 ns |    23,200.0 ns |     376 B |
| ToCamelCase_Short                        | PostgreSQL |     3,666.7 ns |     935.19 ns |     874.78 ns |     3,900.0 ns |      32 B |
| ToCamelCase_Long                         | PostgreSQL |     3,113.3 ns |     872.14 ns |     815.80 ns |     3,000.0 ns |      56 B |
| ToType_Int                               | PostgreSQL |     1,520.0 ns |     398.37 ns |     372.64 ns |     1,500.0 ns |      24 B |
| ToType_Bool                              | PostgreSQL |     1,126.7 ns |     357.93 ns |     334.81 ns |     1,000.0 ns |      24 B |
| ToType_DateTime                          | PostgreSQL |     5,700.0 ns |   1,243.45 ns |   1,163.12 ns |     5,500.0 ns |      24 B |
| ToType_String                            | PostgreSQL |       135.7 ns |      94.97 ns |      84.19 ns |       100.0 ns |         - |
| Combined_Simple_Count                    | PostgreSQL |   770,953.8 ns |  78,617.08 ns |  65,648.82 ns |   781,700.0 ns |    6744 B |
| Combined_SimpleWithFilter_FilterAndCount | PostgreSQL |   820,338.5 ns |  70,904.62 ns |  59,208.57 ns |   803,800.0 ns |    9880 B |
| Combined_SimpleWithSort_SortAndCount     | PostgreSQL |   975,846.2 ns | 143,279.96 ns | 119,645.26 ns |   957,500.0 ns |   13368 B |
| Combined_ComplexFilter_FilterAndCount    | PostgreSQL |   878,269.2 ns |  96,396.18 ns |  80,495.18 ns |   858,100.0 ns |   19184 B |
| Combined_ComplexSort_SortAndCount        | PostgreSQL |   824,569.2 ns |  57,997.11 ns |  48,430.21 ns |   816,400.0 ns |   24344 B |
| Combined_ComplexFull_FilterSortAndCount  | PostgreSQL | 1,229,542.9 ns | 166,413.91 ns | 147,521.60 ns | 1,260,250.0 ns |   30144 B |
| Combined_ComplexGraph_FilterSortAndCount | PostgreSQL | 1,000,407.7 ns | 123,821.09 ns | 103,396.22 ns |   978,400.0 ns |   17216 B |
