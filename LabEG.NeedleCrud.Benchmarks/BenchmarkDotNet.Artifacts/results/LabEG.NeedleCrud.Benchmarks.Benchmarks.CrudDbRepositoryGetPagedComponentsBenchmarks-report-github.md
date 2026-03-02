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
| **AddFilter_NoFilters**                      | **InMemory**   |     **2,573.3 ns** |     **689.84 ns** |     **645.28 ns** |     **2,300.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | InMemory   |    26,033.3 ns |   2,116.89 ns |   1,652.73 ns |    26,600.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | InMemory   |    38,157.1 ns |   3,503.77 ns |   3,106.00 ns |    39,050.0 ns |    5768 B |
| AddFilter_ComplexFull                    | InMemory   |    40,500.0 ns |   4,118.67 ns |   3,215.59 ns |    40,550.0 ns |    4808 B |
| AddFilter_ComplexGraph                   | InMemory   |    29,950.0 ns |   2,471.31 ns |   1,929.44 ns |    29,450.0 ns |    1888 B |
| AddFilter_NavLevel2                      | InMemory   |    45,176.9 ns |   3,757.86 ns |   3,137.98 ns |    43,600.0 ns |    5776 B |
| AddFilter_NavLevel3                      | InMemory   |    62,253.3 ns |  13,567.38 ns |  12,690.93 ns |    58,100.0 ns |    5760 B |
| AddSort_NoSort                           | InMemory   |     2,853.8 ns |     465.39 ns |     388.62 ns |     3,000.0 ns |      88 B |
| AddSort_SimpleWithSort                   | InMemory   |    40,586.7 ns |   6,035.07 ns |   5,645.21 ns |    39,100.0 ns |    5608 B |
| AddSort_ComplexSort                      | InMemory   |    58,400.0 ns |   7,956.47 ns |   7,053.20 ns |    57,000.0 ns |   14608 B |
| AddSort_ComplexFull                      | InMemory   |    51,416.7 ns |   4,198.94 ns |   3,278.26 ns |    50,900.0 ns |   10816 B |
| AddSort_ComplexGraph                     | InMemory   |    44,864.3 ns |   4,070.39 ns |   3,608.29 ns |    45,400.0 ns |    6008 B |
| AddSort_NavLevel2                        | InMemory   |    87,693.3 ns |  22,278.69 ns |  20,839.50 ns |    87,400.0 ns |   10552 B |
| AddSort_NavLevel3                        | InMemory   |    58,453.3 ns |   5,588.10 ns |   5,227.11 ns |    56,500.0 ns |   10640 B |
| ExtractIncludes_SimpleGraph              | InMemory   |    26,930.0 ns |  18,306.43 ns |  17,123.84 ns |    16,450.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | InMemory   |    26,633.3 ns |   7,802.12 ns |   7,298.11 ns |    26,700.0 ns |    1240 B |
| GetMemberExpression_Simple               | InMemory   |     9,415.4 ns |   1,531.92 ns |   1,279.22 ns |     9,600.0 ns |     160 B |
| GetMemberExpression_Nested               | InMemory   |    13,650.0 ns |   2,703.65 ns |   2,396.71 ns |    12,950.0 ns |     376 B |
| GetMemberExpression_NavLevel2            | InMemory   |    10,506.7 ns |   2,352.44 ns |   2,200.48 ns |    10,300.0 ns |     376 B |
| GetMemberExpression_NavLevel3            | InMemory   |    15,760.0 ns |   6,603.10 ns |   6,176.55 ns |    12,600.0 ns |     512 B |
| ToCamelCase_Short                        | InMemory   |     2,750.0 ns |     851.96 ns |     755.24 ns |     2,500.0 ns |      32 B |
| ToCamelCase_Long                         | InMemory   |     2,613.3 ns |     502.84 ns |     470.36 ns |     2,600.0 ns |      56 B |
| ToType_Int                               | InMemory   |     2,050.0 ns |     397.60 ns |     352.46 ns |     2,050.0 ns |      24 B |
| ToType_Bool                              | InMemory   |       507.7 ns |     204.29 ns |     170.59 ns |       500.0 ns |      24 B |
| ToType_DateTime                          | InMemory   |     4,971.4 ns |   1,817.57 ns |   1,611.22 ns |     4,850.0 ns |      24 B |
| ToType_String                            | InMemory   |       138.5 ns |      91.96 ns |      76.79 ns |       200.0 ns |         - |
| Combined_Simple_Count                    | InMemory   |    63,135.7 ns |   8,483.61 ns |   7,520.50 ns |    63,800.0 ns |   15712 B |
| Combined_SimpleWithFilter_FilterAndCount | InMemory   |   128,926.7 ns |  33,464.88 ns |  31,303.07 ns |   130,900.0 ns |   19016 B |
| Combined_SimpleWithSort_SortAndCount     | InMemory   |   109,961.5 ns |  13,157.92 ns |  10,987.46 ns |   107,400.0 ns |   22520 B |
| Combined_ComplexFilter_FilterAndCount    | InMemory   |   138,985.7 ns |   9,482.34 ns |   8,405.85 ns |   138,300.0 ns |   28968 B |
| Combined_ComplexSort_SortAndCount        | InMemory   |   183,921.4 ns |  57,397.29 ns |  50,881.21 ns |   174,300.0 ns |   34032 B |
| Combined_ComplexFull_FilterSortAndCount  | InMemory   |   265,600.0 ns |  86,125.24 ns |  80,561.60 ns |   230,600.0 ns |   40304 B |
| Combined_ComplexGraph_FilterSortAndCount | InMemory   |   139,461.5 ns |  13,383.95 ns |  11,176.21 ns |   144,300.0 ns |   26536 B |
| **AddFilter_NoFilters**                      | **PostgreSQL** |     **5,235.7 ns** |     **937.15 ns** |     **830.76 ns** |     **5,150.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | PostgreSQL |    63,293.3 ns |  23,059.92 ns |  21,570.27 ns |    64,500.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | PostgreSQL |    76,885.7 ns |   4,862.44 ns |   4,310.43 ns |    76,800.0 ns |    5768 B |
| AddFilter_ComplexFull                    | PostgreSQL |    72,900.0 ns |   8,636.14 ns |   7,211.56 ns |    72,700.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | PostgreSQL |    88,806.7 ns |   6,924.23 ns |   6,476.93 ns |    88,200.0 ns |    1888 B |
| AddFilter_NavLevel2                      | PostgreSQL |    90,007.7 ns |   4,950.02 ns |   4,133.49 ns |    90,700.0 ns |    5568 B |
| AddFilter_NavLevel3                      | PostgreSQL |   113,278.6 ns |  29,687.22 ns |  26,316.95 ns |   112,800.0 ns |    5880 B |
| AddSort_NoSort                           | PostgreSQL |     3,016.7 ns |     340.34 ns |     265.72 ns |     3,050.0 ns |      88 B |
| AddSort_SimpleWithSort                   | PostgreSQL |    80,293.3 ns |  13,914.78 ns |  13,015.90 ns |    78,400.0 ns |    5440 B |
| AddSort_ComplexSort                      | PostgreSQL |   101,478.6 ns |  18,644.00 ns |  16,527.42 ns |   105,800.0 ns |   14480 B |
| AddSort_ComplexFull                      | PostgreSQL |    70,146.7 ns |  18,855.08 ns |  17,637.05 ns |    63,200.0 ns |   10816 B |
| AddSort_ComplexGraph                     | PostgreSQL |    69,273.3 ns |  29,221.74 ns |  27,334.03 ns |    62,800.0 ns |    6008 B |
| AddSort_NavLevel2                        | PostgreSQL |    93,761.5 ns |  13,108.42 ns |  10,946.12 ns |    96,500.0 ns |   10424 B |
| AddSort_NavLevel3                        | PostgreSQL |   141,673.3 ns |  48,637.25 ns |  45,495.31 ns |   138,100.0 ns |   10768 B |
| ExtractIncludes_SimpleGraph              | PostgreSQL |    30,092.9 ns |   4,204.44 ns |   3,727.13 ns |    30,800.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | PostgreSQL |    27,014.3 ns |   9,195.99 ns |   8,152.00 ns |    25,200.0 ns |    1240 B |
| GetMemberExpression_Simple               | PostgreSQL |    13,830.8 ns |   3,097.59 ns |   2,586.63 ns |    13,800.0 ns |     160 B |
| GetMemberExpression_Nested               | PostgreSQL |    13,373.3 ns |   3,055.04 ns |   2,857.69 ns |    12,800.0 ns |     376 B |
| GetMemberExpression_NavLevel2            | PostgreSQL |    12,600.0 ns |   3,357.40 ns |   3,140.52 ns |    11,800.0 ns |     376 B |
| GetMemberExpression_NavLevel3            | PostgreSQL |    12,786.7 ns |   3,071.67 ns |   2,873.24 ns |    12,000.0 ns |     512 B |
| ToCamelCase_Short                        | PostgreSQL |     2,811.5 ns |     956.21 ns |     798.48 ns |     2,850.0 ns |      32 B |
| ToCamelCase_Long                         | PostgreSQL |     3,576.9 ns |     637.73 ns |     532.53 ns |     3,700.0 ns |      56 B |
| ToType_Int                               | PostgreSQL |     2,314.3 ns |     380.25 ns |     337.09 ns |     2,400.0 ns |      24 B |
| ToType_Bool                              | PostgreSQL |       806.7 ns |     281.30 ns |     263.13 ns |       800.0 ns |      24 B |
| ToType_DateTime                          | PostgreSQL |     6,673.3 ns |   1,011.35 ns |     946.02 ns |     6,700.0 ns |      24 B |
| ToType_String                            | PostgreSQL |       307.1 ns |     136.12 ns |     120.67 ns |       300.0 ns |         - |
| Combined_Simple_Count                    | PostgreSQL |   773,769.2 ns |  82,583.63 ns |  68,961.07 ns |   787,700.0 ns |    6744 B |
| Combined_SimpleWithFilter_FilterAndCount | PostgreSQL |   854,561.5 ns |  90,580.84 ns |  75,639.11 ns |   866,200.0 ns |    9880 B |
| Combined_SimpleWithSort_SortAndCount     | PostgreSQL |   898,538.5 ns |  65,779.89 ns |  54,929.19 ns |   910,400.0 ns |   13368 B |
| Combined_ComplexFilter_FilterAndCount    | PostgreSQL |   974,569.2 ns |  83,474.53 ns |  69,705.01 ns |   997,600.0 ns |   19184 B |
| Combined_ComplexSort_SortAndCount        | PostgreSQL |   993,550.0 ns | 155,685.29 ns | 138,010.96 ns |   974,900.0 ns |   24344 B |
| Combined_ComplexFull_FilterSortAndCount  | PostgreSQL | 1,176,250.0 ns | 134,911.50 ns | 119,595.53 ns | 1,204,900.0 ns |   30144 B |
| Combined_ComplexGraph_FilterSortAndCount | PostgreSQL |   990,784.6 ns | 143,530.72 ns | 119,854.65 ns | 1,014,400.0 ns |   17216 B |
