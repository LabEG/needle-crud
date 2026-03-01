```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-NZVBQK : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=5  UnrollFactor=1  
WarmupCount=1  

```
| Method                                   | Provider   | Mean           | Error         | StdDev        | Median         | Allocated |
|----------------------------------------- |----------- |---------------:|--------------:|--------------:|---------------:|----------:|
| **AddFilter_NoFilters**                      | **InMemory**   |     **2,600.0 ns** |   **5,633.43 ns** |     **871.78 ns** |     **3,000.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | InMemory   |    24,050.0 ns |  23,456.81 ns |   3,629.97 ns |    23,700.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | InMemory   |    42,120.0 ns |  41,758.10 ns |  10,844.45 ns |    36,100.0 ns |    5768 B |
| AddFilter_ComplexFull                    | InMemory   |    54,540.0 ns |  79,278.67 ns |  20,588.42 ns |    43,600.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | InMemory   |    44,140.0 ns |  40,136.01 ns |  10,423.20 ns |    38,700.0 ns |    1888 B |
| AddSort_NoSort                           | InMemory   |     1,450.0 ns |   1,119.25 ns |     173.21 ns |     1,400.0 ns |      88 B |
| AddSort_SimpleWithSort                   | InMemory   |    63,260.0 ns |  31,079.01 ns |   8,071.12 ns |    59,100.0 ns |    5440 B |
| AddSort_ComplexSort                      | InMemory   |    83,600.0 ns |  92,617.79 ns |  24,052.55 ns |    78,800.0 ns |   14480 B |
| AddSort_ComplexFull                      | InMemory   |    57,160.0 ns |  94,367.33 ns |  24,506.90 ns |    41,300.0 ns |   10816 B |
| AddSort_ComplexGraph                     | InMemory   |    46,100.0 ns |  59,141.55 ns |  15,358.87 ns |    37,600.0 ns |    6008 B |
| ExtractIncludes_SimpleGraph              | InMemory   |    15,275.0 ns |  13,247.08 ns |   2,050.00 ns |    14,500.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | InMemory   |    29,960.0 ns |  53,428.71 ns |  13,875.27 ns |    29,800.0 ns |    1240 B |
| GetMemberExpression_Simple               | InMemory   |     7,175.0 ns |   3,707.44 ns |     573.73 ns |     7,050.0 ns |     160 B |
| GetMemberExpression_Nested               | InMemory   |     8,525.0 ns |   8,881.81 ns |   1,374.47 ns |     8,250.0 ns |     376 B |
| ToCamelCase_Short                        | InMemory   |     1,860.0 ns |   1,404.30 ns |     364.69 ns |     1,900.0 ns |      32 B |
| ToCamelCase_Long                         | InMemory   |     1,500.0 ns |     527.62 ns |      81.65 ns |     1,500.0 ns |      56 B |
| ToType_Int                               | InMemory   |     1,140.0 ns |     516.62 ns |     134.16 ns |     1,200.0 ns |      24 B |
| ToType_Bool                              | InMemory   |       600.0 ns |     608.84 ns |     158.11 ns |       600.0 ns |      24 B |
| ToType_DateTime                          | InMemory   |     4,475.0 ns |   7,393.73 ns |   1,144.19 ns |     4,350.0 ns |      24 B |
| ToType_String                            | InMemory   |       100.0 ns |       0.00 ns |       0.00 ns |       100.0 ns |         - |
| Combined_Simple_Count                    | InMemory   |   105,020.0 ns |  41,239.12 ns |  10,709.67 ns |   102,200.0 ns |   15712 B |
| Combined_SimpleWithFilter_FilterAndCount | InMemory   |   144,680.0 ns | 124,526.63 ns |  32,339.17 ns |   147,900.0 ns |   19016 B |
| Combined_SimpleWithSort_SortAndCount     | InMemory   |   144,480.0 ns | 124,711.35 ns |  32,387.14 ns |   152,300.0 ns |   22520 B |
| Combined_ComplexFilter_FilterAndCount    | InMemory   |   192,480.0 ns |  79,733.16 ns |  20,706.45 ns |   201,700.0 ns |   28968 B |
| Combined_ComplexSort_SortAndCount        | InMemory   |   136,000.0 ns | 109,770.37 ns |  28,507.02 ns |   134,500.0 ns |   33864 B |
| Combined_ComplexFull_FilterSortAndCount  | InMemory   |   236,680.0 ns | 221,064.42 ns |  57,409.73 ns |   238,000.0 ns |   39624 B |
| Combined_ComplexGraph_FilterSortAndCount | InMemory   |   160,440.0 ns | 177,886.75 ns |  46,196.62 ns |   151,000.0 ns |   26536 B |
| **AddFilter_NoFilters**                      | **PostgreSQL** |     **3,400.0 ns** |   **2,310.39 ns** |     **600.00 ns** |     **3,700.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | PostgreSQL |    44,500.0 ns |  48,036.54 ns |   7,433.71 ns |    44,450.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | PostgreSQL |    59,740.0 ns |  64,626.38 ns |  16,783.27 ns |    58,000.0 ns |    5768 B |
| AddFilter_ComplexFull                    | PostgreSQL |    59,360.0 ns |  72,189.36 ns |  18,747.35 ns |    66,000.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | PostgreSQL |    59,840.0 ns |  54,364.08 ns |  14,118.18 ns |    67,000.0 ns |    1888 B |
| AddSort_NoSort                           | PostgreSQL |     2,460.0 ns |   2,459.59 ns |     638.75 ns |     2,400.0 ns |      88 B |
| AddSort_SimpleWithSort                   | PostgreSQL |    81,240.0 ns |  70,663.57 ns |  18,351.10 ns |    74,200.0 ns |    5440 B |
| AddSort_ComplexSort                      | PostgreSQL |    76,240.0 ns |  54,222.07 ns |  14,081.30 ns |    78,300.0 ns |   14656 B |
| AddSort_ComplexFull                      | PostgreSQL |   124,525.0 ns |  55,769.06 ns |   8,630.32 ns |   126,600.0 ns |   10816 B |
| AddSort_ComplexGraph                     | PostgreSQL |    76,460.0 ns |  64,191.29 ns |  16,670.27 ns |    83,400.0 ns |    6008 B |
| ExtractIncludes_SimpleGraph              | PostgreSQL |    21,300.0 ns |  26,170.22 ns |   6,796.32 ns |    22,500.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | PostgreSQL |    31,220.0 ns |  38,002.95 ns |   9,869.25 ns |    31,200.0 ns |    1240 B |
| GetMemberExpression_Simple               | PostgreSQL |    15,520.0 ns |  15,947.43 ns |   4,141.50 ns |    16,100.0 ns |     160 B |
| GetMemberExpression_Nested               | PostgreSQL |    17,320.0 ns |  20,268.80 ns |   5,263.74 ns |    20,100.0 ns |     376 B |
| ToCamelCase_Short                        | PostgreSQL |     2,200.0 ns |   2,294.29 ns |     595.82 ns |     2,100.0 ns |      32 B |
| ToCamelCase_Long                         | PostgreSQL |     4,275.0 ns |     618.69 ns |      95.74 ns |     4,300.0 ns |      56 B |
| ToType_Int                               | PostgreSQL |     2,640.0 ns |   3,970.09 ns |   1,031.02 ns |     2,000.0 ns |      24 B |
| ToType_Bool                              | PostgreSQL |       650.0 ns |     527.62 ns |      81.65 ns |       650.0 ns |      24 B |
| ToType_DateTime                          | PostgreSQL |    10,860.0 ns |   5,868.92 ns |   1,524.14 ns |    10,200.0 ns |      24 B |
| ToType_String                            | PostgreSQL |       180.0 ns |     632.73 ns |     164.32 ns |       100.0 ns |         - |
| Combined_Simple_Count                    | PostgreSQL |   825,320.0 ns | 521,809.24 ns | 135,512.20 ns |   802,500.0 ns |    6744 B |
| Combined_SimpleWithFilter_FilterAndCount | PostgreSQL |   898,840.0 ns | 503,318.09 ns | 130,710.11 ns |   903,200.0 ns |    9880 B |
| Combined_SimpleWithSort_SortAndCount     | PostgreSQL |   894,070.0 ns | 587,777.20 ns | 152,643.87 ns |   900,650.0 ns |   13368 B |
| Combined_ComplexFilter_FilterAndCount    | PostgreSQL | 1,300,075.0 ns | 631,674.22 ns |  97,752.28 ns | 1,282,350.0 ns |   19296 B |
| Combined_ComplexSort_SortAndCount        | PostgreSQL |   992,760.0 ns | 517,417.34 ns | 134,371.64 ns | 1,056,900.0 ns |   24344 B |
| Combined_ComplexFull_FilterSortAndCount  | PostgreSQL | 1,043,130.0 ns | 698,585.84 ns | 181,420.53 ns | 1,067,050.0 ns |   30304 B |
| Combined_ComplexGraph_FilterSortAndCount | PostgreSQL |   981,380.0 ns | 438,063.97 ns | 113,763.82 ns |   978,500.0 ns |   17216 B |
