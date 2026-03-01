```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-NZVBQK : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=5  UnrollFactor=1  
WarmupCount=1  

```
| Method                                   | Provider   | Mean            | Error         | StdDev        | Median         | Allocated |
|----------------------------------------- |----------- |----------------:|--------------:|--------------:|---------------:|----------:|
| **AddFilter_NoFilters**                      | **InMemory**   |     **2,760.00 ns** |   **2,811.23 ns** |     **730.07 ns** |     **2,500.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | InMemory   |    50,760.00 ns |  58,563.09 ns |  15,208.65 ns |    52,000.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | InMemory   |    88,100.00 ns |  10,552.38 ns |   1,632.99 ns |    87,500.0 ns |    5768 B |
| AddFilter_ComplexFull                    | InMemory   |    69,820.00 ns |  71,670.30 ns |  18,612.55 ns |    65,800.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | InMemory   |    44,290.00 ns |  59,883.77 ns |  15,551.62 ns |    40,950.0 ns |    1888 B |
| AddSort_NoSort                           | InMemory   |     1,875.00 ns |   1,103.59 ns |     170.78 ns |     1,850.0 ns |      88 B |
| AddSort_SimpleWithSort                   | InMemory   |    78,130.00 ns |  98,103.57 ns |  25,477.19 ns |    78,350.0 ns |    5440 B |
| AddSort_ComplexSort                      | InMemory   |    69,780.00 ns | 120,199.75 ns |  31,215.49 ns |    49,700.0 ns |   14480 B |
| AddSort_ComplexFull                      | InMemory   |    68,620.00 ns |  74,958.30 ns |  19,466.43 ns |    64,400.0 ns |   10816 B |
| AddSort_ComplexGraph                     | InMemory   |    63,140.00 ns |  41,123.18 ns |  10,679.56 ns |    56,800.0 ns |    6008 B |
| ExtractIncludes_SimpleGraph              | InMemory   |    19,790.00 ns |  16,212.99 ns |   4,210.46 ns |    19,450.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | InMemory   |    32,120.00 ns |  62,233.72 ns |  16,161.90 ns |    28,400.0 ns |    1240 B |
| GetMemberExpression_Simple               | InMemory   |    11,125.00 ns |  19,417.36 ns |   3,004.86 ns |    11,200.0 ns |     160 B |
| GetMemberExpression_Nested               | InMemory   |    11,250.00 ns |  20,775.64 ns |   5,395.37 ns |     9,250.0 ns |     376 B |
| ToCamelCase_Short                        | InMemory   |     4,125.00 ns |   3,393.83 ns |     525.20 ns |     4,250.0 ns |      32 B |
| ToCamelCase_Long                         | InMemory   |     2,420.00 ns |   1,573.59 ns |     408.66 ns |     2,500.0 ns |      56 B |
| ToType_Int                               | InMemory   |     1,300.00 ns |     385.06 ns |     100.00 ns |     1,300.0 ns |      24 B |
| ToType_Bool                              | InMemory   |       760.00 ns |     210.91 ns |      54.77 ns |       800.0 ns |      24 B |
| ToType_DateTime                          | InMemory   |     4,120.00 ns |   5,118.61 ns |   1,329.29 ns |     4,200.0 ns |      24 B |
| ToType_String                            | InMemory   |        60.00 ns |     210.91 ns |      54.77 ns |       100.0 ns |         - |
| Combined_Simple_Count                    | InMemory   |    72,420.00 ns |  86,921.58 ns |  22,573.26 ns |    71,400.0 ns |   15712 B |
| Combined_SimpleWithFilter_FilterAndCount | InMemory   |   123,620.00 ns | 123,000.02 ns |  31,942.71 ns |   131,200.0 ns |   19016 B |
| Combined_SimpleWithSort_SortAndCount     | InMemory   |   141,540.00 ns | 167,766.29 ns |  43,568.37 ns |   154,300.0 ns |   22520 B |
| Combined_ComplexFilter_FilterAndCount    | InMemory   |   186,700.00 ns | 156,108.59 ns |  40,540.91 ns |   184,900.0 ns |   29080 B |
| Combined_ComplexSort_SortAndCount        | InMemory   |   159,020.00 ns | 171,478.95 ns |  44,532.54 ns |   173,500.0 ns |   33976 B |
| Combined_ComplexFull_FilterSortAndCount  | InMemory   |   184,625.00 ns | 162,378.51 ns |  25,128.25 ns |   191,500.0 ns |   39672 B |
| Combined_ComplexGraph_FilterSortAndCount | InMemory   |   172,970.00 ns | 179,799.38 ns |  46,693.33 ns |   153,350.0 ns |   26536 B |
| **AddFilter_NoFilters**                      | **PostgreSQL** |     **2,860.00 ns** |   **2,620.14 ns** |     **680.44 ns** |     **3,000.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | PostgreSQL |   102,120.00 ns |  16,473.38 ns |   4,278.08 ns |   100,500.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | PostgreSQL |    58,900.00 ns | 108,231.41 ns |  16,748.93 ns |    66,550.0 ns |    5768 B |
| AddFilter_ComplexFull                    | PostgreSQL |    45,650.00 ns |  36,575.47 ns |   5,660.09 ns |    47,600.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | PostgreSQL |    57,400.00 ns |  51,796.53 ns |  13,451.39 ns |    61,400.0 ns |    1888 B |
| AddSort_NoSort                           | PostgreSQL |     4,300.00 ns |   3,518.65 ns |     913.78 ns |     4,700.0 ns |      88 B |
| AddSort_SimpleWithSort                   | PostgreSQL |    63,450.00 ns |  71,282.31 ns |  11,031.02 ns |    66,450.0 ns |    5440 B |
| AddSort_ComplexSort                      | PostgreSQL |   105,020.00 ns |  32,252.91 ns |   8,375.98 ns |   104,400.0 ns |   14480 B |
| AddSort_ComplexFull                      | PostgreSQL |    77,920.00 ns |  86,158.71 ns |  22,375.14 ns |    82,000.0 ns |   10816 B |
| AddSort_ComplexGraph                     | PostgreSQL |    80,520.00 ns |  55,768.17 ns |  14,482.82 ns |    84,100.0 ns |    6008 B |
| ExtractIncludes_SimpleGraph              | PostgreSQL |    28,560.00 ns |  37,411.34 ns |   9,715.61 ns |    32,200.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | PostgreSQL |    27,780.00 ns |  39,089.63 ns |  10,151.45 ns |    30,600.0 ns |    1240 B |
| GetMemberExpression_Simple               | PostgreSQL |    17,160.00 ns |  22,122.95 ns |   5,745.26 ns |    16,600.0 ns |     160 B |
| GetMemberExpression_Nested               | PostgreSQL |    15,640.00 ns |  18,757.02 ns |   4,871.14 ns |    17,500.0 ns |     376 B |
| ToCamelCase_Short                        | PostgreSQL |     2,860.00 ns |   1,554.63 ns |     403.73 ns |     2,800.0 ns |      32 B |
| ToCamelCase_Long                         | PostgreSQL |     4,070.00 ns |   3,921.23 ns |   1,018.33 ns |     4,050.0 ns |      56 B |
| ToType_Int                               | PostgreSQL |     3,060.00 ns |     583.98 ns |     151.66 ns |     3,100.0 ns |      24 B |
| ToType_Bool                              | PostgreSQL |       760.00 ns |     798.49 ns |     207.36 ns |       800.0 ns |      24 B |
| ToType_DateTime                          | PostgreSQL |     5,880.00 ns |   6,422.62 ns |   1,667.93 ns |     5,900.0 ns |      24 B |
| ToType_String                            | PostgreSQL |       180.00 ns |     502.06 ns |     130.38 ns |       200.0 ns |         - |
| Combined_Simple_Count                    | PostgreSQL |   889,780.00 ns | 628,561.18 ns | 163,235.34 ns |   910,600.0 ns |    6744 B |
| Combined_SimpleWithFilter_FilterAndCount | PostgreSQL | 1,026,200.00 ns | 714,609.64 ns | 185,581.86 ns | 1,087,800.0 ns |    9880 B |
| Combined_SimpleWithSort_SortAndCount     | PostgreSQL |   936,650.00 ns | 389,788.74 ns | 101,226.90 ns |   916,450.0 ns |   13368 B |
| Combined_ComplexFilter_FilterAndCount    | PostgreSQL | 1,043,780.00 ns | 575,677.73 ns | 149,501.68 ns |   954,200.0 ns |   19488 B |
| Combined_ComplexSort_SortAndCount        | PostgreSQL | 1,047,480.00 ns | 495,649.40 ns | 128,718.58 ns | 1,064,700.0 ns |   24344 B |
| Combined_ComplexFull_FilterSortAndCount  | PostgreSQL | 1,088,700.00 ns | 697,611.34 ns | 181,167.45 ns | 1,029,700.0 ns |   29584 B |
| Combined_ComplexGraph_FilterSortAndCount | PostgreSQL | 1,004,440.00 ns | 747,161.15 ns | 194,035.38 ns |   994,100.0 ns |   17216 B |
