```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-KQXXMH : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=15  UnrollFactor=1  
WarmupCount=3  

```
| Method                                   | Provider   | Mean           | Error        | StdDev        | Median         | Allocated |
|----------------------------------------- |----------- |---------------:|-------------:|--------------:|---------------:|----------:|
| **AddFilter_NoFilters**                      | **InMemory**   |     **1,700.0 ns** |     **309.7 ns** |     **274.56 ns** |     **1,600.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | InMemory   |    27,957.1 ns |   4,312.8 ns |   3,823.15 ns |    27,400.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | InMemory   |    43,586.7 ns |  11,784.1 ns |  11,022.89 ns |    40,400.0 ns |    5768 B |
| AddFilter_ComplexFull                    | InMemory   |    37,728.6 ns |  13,043.3 ns |  11,562.57 ns |    32,700.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | InMemory   |    40,166.7 ns |  10,078.7 ns |   9,427.59 ns |    40,200.0 ns |    1888 B |
| AddFilter_NavLevel2                      | InMemory   |    71,093.3 ns |  16,325.7 ns |  15,271.05 ns |    73,300.0 ns |    2016 B |
| AddFilter_NavLevel3                      | InMemory   |    38,806.7 ns |  12,195.0 ns |  11,407.17 ns |    32,900.0 ns |    2208 B |
| AddSort_NoSort                           | InMemory   |     1,728.6 ns |     469.2 ns |     415.89 ns |     1,650.0 ns |      88 B |
| AddSort_SimpleWithSort                   | InMemory   |    37,433.3 ns |   4,306.4 ns |   4,028.23 ns |    38,400.0 ns |    5440 B |
| AddSort_ComplexSort                      | InMemory   |    86,871.4 ns |  34,712.8 ns |  30,772.03 ns |    90,550.0 ns |   14480 B |
| AddSort_ComplexFull                      | InMemory   |    59,866.7 ns |  11,988.1 ns |  11,213.68 ns |    58,300.0 ns |   10984 B |
| AddSort_ComplexGraph                     | InMemory   |    52,393.3 ns |  15,344.8 ns |  14,353.57 ns |    50,100.0 ns |    6008 B |
| AddSort_NavLevel2                        | InMemory   |    65,926.7 ns |  13,841.4 ns |  12,947.28 ns |    62,300.0 ns |   10424 B |
| AddSort_NavLevel3                        | InMemory   |    55,907.1 ns |   6,837.3 ns |   6,061.06 ns |    53,750.0 ns |   10760 B |
| ExtractIncludes_SimpleGraph              | InMemory   |    18,353.3 ns |   3,580.0 ns |   3,348.75 ns |    17,900.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | InMemory   |    25,507.1 ns |   8,492.2 ns |   7,528.15 ns |    24,350.0 ns |    1240 B |
| GetMemberExpression_Simple               | InMemory   |     8,120.0 ns |   2,953.3 ns |   2,762.56 ns |     7,000.0 ns |     160 B |
| GetMemberExpression_Nested               | InMemory   |    17,306.7 ns |   6,338.9 ns |   5,929.40 ns |    17,300.0 ns |     376 B |
| GetMemberExpression_NavLevel2            | InMemory   |    10,371.4 ns |   3,231.9 ns |   2,865.02 ns |     9,400.0 ns |     376 B |
| GetMemberExpression_NavLevel3            | InMemory   |    16,406.7 ns |   5,344.2 ns |   4,998.92 ns |    16,300.0 ns |     512 B |
| ToCamelCase_Short                        | InMemory   |     1,746.7 ns |     663.8 ns |     620.91 ns |     1,500.0 ns |      32 B |
| ToCamelCase_Long                         | InMemory   |     2,486.7 ns |     684.4 ns |     640.16 ns |     2,600.0 ns |      56 B |
| ToType_Int                               | InMemory   |     1,580.0 ns |     361.9 ns |     338.48 ns |     1,500.0 ns |      24 B |
| ToType_Bool                              | InMemory   |       978.6 ns |     313.4 ns |     277.84 ns |       850.0 ns |      24 B |
| ToType_DateTime                          | InMemory   |     3,973.3 ns |     934.1 ns |     873.80 ns |     4,100.0 ns |      24 B |
| ToType_String                            | InMemory   |       292.3 ns |     103.3 ns |      86.23 ns |       300.0 ns |         - |
| Combined_Simple_Count                    | InMemory   |    62,800.0 ns |  10,830.4 ns |  10,130.72 ns |    61,700.0 ns |   15712 B |
| Combined_SimpleWithFilter_FilterAndCount | InMemory   |   116,146.7 ns |  16,622.5 ns |  15,548.72 ns |   110,000.0 ns |   19016 B |
| Combined_SimpleWithSort_SortAndCount     | InMemory   |   105,733.3 ns |  19,206.0 ns |  17,965.27 ns |   106,100.0 ns |   22520 B |
| Combined_ComplexFilter_FilterAndCount    | InMemory   |   150,714.3 ns |  27,437.0 ns |  24,322.22 ns |   147,750.0 ns |   28968 B |
| Combined_ComplexSort_SortAndCount        | InMemory   |   133,378.6 ns |  19,344.7 ns |  17,148.59 ns |   130,550.0 ns |   34032 B |
| Combined_ComplexFull_FilterSortAndCount  | InMemory   |   177,692.9 ns |  16,496.2 ns |  14,623.45 ns |   177,900.0 ns |   40480 B |
| Combined_ComplexGraph_FilterSortAndCount | InMemory   |   178,060.0 ns |  45,336.3 ns |  42,407.64 ns |   166,200.0 ns |   26536 B |
| **AddFilter_NoFilters**                      | **PostgreSQL** |     **2,613.3 ns** |     **559.7 ns** |     **523.54 ns** |     **2,500.0 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | PostgreSQL |    41,557.1 ns |  11,186.7 ns |   9,916.71 ns |    40,500.0 ns |    1528 B |
| AddFilter_ComplexFilter                  | PostgreSQL |    79,540.0 ns |  45,874.2 ns |  42,910.75 ns |    61,700.0 ns |    5768 B |
| AddFilter_ComplexFull                    | PostgreSQL |    51,926.7 ns |  10,761.7 ns |  10,066.46 ns |    53,300.0 ns |    4696 B |
| AddFilter_ComplexGraph                   | PostgreSQL |    61,440.0 ns |   6,839.1 ns |   6,397.30 ns |    62,900.0 ns |    1888 B |
| AddFilter_NavLevel2                      | PostgreSQL |    50,233.3 ns |  11,633.4 ns |  10,881.88 ns |    50,100.0 ns |    2016 B |
| AddFilter_NavLevel3                      | PostgreSQL |    55,373.1 ns |  15,703.2 ns |  13,112.85 ns |    55,050.0 ns |    2320 B |
| AddSort_NoSort                           | PostgreSQL |     2,730.0 ns |     503.4 ns |     470.87 ns |     2,650.0 ns |      88 B |
| AddSort_SimpleWithSort                   | PostgreSQL |    51,420.0 ns |  11,687.8 ns |  10,932.79 ns |    52,200.0 ns |    5440 B |
| AddSort_ComplexSort                      | PostgreSQL |    68,306.7 ns |  12,880.1 ns |  12,048.03 ns |    67,000.0 ns |   14648 B |
| AddSort_ComplexFull                      | PostgreSQL |    92,860.0 ns |  15,932.5 ns |  14,903.30 ns |    95,400.0 ns |   10816 B |
| AddSort_ComplexGraph                     | PostgreSQL |    61,560.0 ns |  14,894.6 ns |  13,932.38 ns |    62,300.0 ns |    6008 B |
| AddSort_NavLevel2                        | PostgreSQL |    99,092.9 ns |  13,946.8 ns |  12,363.44 ns |   103,150.0 ns |   10424 B |
| AddSort_NavLevel3                        | PostgreSQL |   134,860.0 ns |  27,248.3 ns |  25,488.03 ns |   141,700.0 ns |   10768 B |
| ExtractIncludes_SimpleGraph              | PostgreSQL |    23,433.3 ns |   5,769.6 ns |   5,396.91 ns |    23,900.0 ns |     752 B |
| ExtractIncludes_ComplexGraph             | PostgreSQL |    28,685.7 ns |   6,193.8 ns |   5,490.67 ns |    28,850.0 ns |    1240 B |
| GetMemberExpression_Simple               | PostgreSQL |    13,290.0 ns |   2,754.7 ns |   2,576.76 ns |    13,150.0 ns |     160 B |
| GetMemberExpression_Nested               | PostgreSQL |    13,500.0 ns |   4,117.3 ns |   3,851.34 ns |    13,200.0 ns |     376 B |
| GetMemberExpression_NavLevel2            | PostgreSQL |    18,700.0 ns |   5,002.0 ns |   4,434.13 ns |    18,650.0 ns |     376 B |
| GetMemberExpression_NavLevel3            | PostgreSQL |    24,121.4 ns |  16,070.9 ns |  14,246.41 ns |    21,000.0 ns |     512 B |
| ToCamelCase_Short                        | PostgreSQL |     3,100.0 ns |   1,057.3 ns |     937.26 ns |     2,800.0 ns |      32 B |
| ToCamelCase_Long                         | PostgreSQL |     2,993.3 ns |     986.8 ns |     923.09 ns |     2,600.0 ns |      56 B |
| ToType_Int                               | PostgreSQL |     2,107.7 ns |     490.0 ns |     409.19 ns |     2,100.0 ns |      24 B |
| ToType_Bool                              | PostgreSQL |     1,233.3 ns |     449.3 ns |     420.32 ns |     1,100.0 ns |      24 B |
| ToType_DateTime                          | PostgreSQL |     5,700.0 ns |   1,150.4 ns |   1,019.80 ns |     5,950.0 ns |      24 B |
| ToType_String                            | PostgreSQL |       935.7 ns |     195.9 ns |     173.68 ns |       900.0 ns |         - |
| Combined_Simple_Count                    | PostgreSQL |   804,023.1 ns | 106,666.7 ns |  89,071.49 ns |   799,700.0 ns |    6744 B |
| Combined_SimpleWithFilter_FilterAndCount | PostgreSQL |   834,230.8 ns | 111,698.7 ns |  93,273.45 ns |   797,600.0 ns |    9880 B |
| Combined_SimpleWithSort_SortAndCount     | PostgreSQL |   859,492.3 ns | 146,537.9 ns | 122,365.75 ns |   878,400.0 ns |   13368 B |
| Combined_ComplexFilter_FilterAndCount    | PostgreSQL |   943,153.8 ns | 129,340.2 ns | 108,004.91 ns |   935,500.0 ns |   19184 B |
| Combined_ComplexSort_SortAndCount        | PostgreSQL |   912,957.1 ns | 139,454.9 ns | 123,623.10 ns |   899,900.0 ns |   24664 B |
| Combined_ComplexFull_FilterSortAndCount  | PostgreSQL | 1,030,821.4 ns | 157,268.5 ns | 139,414.43 ns | 1,000,150.0 ns |   29584 B |
| Combined_ComplexGraph_FilterSortAndCount | PostgreSQL | 1,103,035.7 ns | 376,649.1 ns | 333,889.61 ns |   964,650.0 ns |   17216 B |
