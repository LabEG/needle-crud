```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.7922/25H2/2025Update/HudsonValley2)
AMD Ryzen 7 5800X 3.80GHz, 1 CPU, 16 logical and 8 physical cores
.NET SDK 10.0.103
  [Host]     : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3
  Job-NZVBQK : .NET 10.0.3 (10.0.3, 10.0.326.7603), X64 RyuJIT x86-64-v3

InvocationCount=1  IterationCount=5  UnrollFactor=1  
WarmupCount=1  

```
| Method                                   | Provider   | Mean            | Error            | StdDev         | Allocated |
|----------------------------------------- |----------- |----------------:|-----------------:|---------------:|----------:|
| **AddFilter_NoFilters**                      | **InMemory**   |     **5,170.00 ns** |     **4,820.231 ns** |   **1,251.799 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | InMemory   |    60,450.00 ns |    36,342.586 ns |   5,624.055 ns |    1528 B |
| AddFilter_ComplexFilter                  | InMemory   |    90,980.00 ns |    13,764.100 ns |   3,574.493 ns |    5880 B |
| AddFilter_ComplexFull                    | InMemory   |    82,840.00 ns |    98,022.753 ns |  25,456.198 ns |    4776 B |
| AddFilter_ComplexGraph                   | InMemory   |    91,980.00 ns |    60,759.330 ns |  15,779.005 ns |    2504 B |
| AddSort_NoSort                           | InMemory   |     4,675.00 ns |     4,200.293 ns |     650.000 ns |      88 B |
| AddSort_SimpleWithSort                   | InMemory   |    95,075.00 ns |   132,878.756 ns |  20,563.134 ns |    5440 B |
| AddSort_ComplexSort                      | InMemory   |    61,600.00 ns |    52,933.120 ns |   8,191.459 ns |   14480 B |
| AddSort_ComplexFull                      | InMemory   |    68,130.00 ns |    34,801.386 ns |   9,037.809 ns |   10944 B |
| AddSort_ComplexGraph                     | InMemory   |   108,860.00 ns |    57,878.009 ns |  15,030.735 ns |    6624 B |
| ExtractIncludes_SimpleGraph              | InMemory   |    23,360.00 ns |    18,070.560 ns |   4,692.867 ns |    1112 B |
| ExtractIncludes_ComplexGraph             | InMemory   |    31,240.00 ns |    21,686.340 ns |   5,631.874 ns |    1672 B |
| GetMemberExpression_Simple               | InMemory   |    17,750.00 ns |    22,474.969 ns |   3,478.026 ns |     160 B |
| GetMemberExpression_Nested               | InMemory   |    15,040.00 ns |     9,849.673 ns |   2,557.929 ns |     376 B |
| ToCamelCase_Short                        | InMemory   |     2,180.00 ns |     3,667.220 ns |     952.365 ns |      32 B |
| ToCamelCase_Long                         | InMemory   |     3,010.00 ns |     2,351.732 ns |     610.737 ns |      56 B |
| ToType_Int                               | InMemory   |     2,000.00 ns |     2,937.659 ns |     454.606 ns |      24 B |
| ToType_Bool                              | InMemory   |       725.00 ns |       323.099 ns |      50.000 ns |      24 B |
| ToType_DateTime                          | InMemory   |     5,820.00 ns |     4,703.463 ns |   1,221.475 ns |      24 B |
| ToType_String                            | InMemory   |        50.00 ns |         0.000 ns |       0.000 ns |         - |
| Combined_Simple_Count                    | InMemory   |    84,450.00 ns |    93,520.414 ns |  14,472.388 ns |   16680 B |
| Combined_SimpleWithFilter_FilterAndCount | InMemory   |   112,700.00 ns |   111,154.982 ns |  17,201.357 ns |   19984 B |
| Combined_SimpleWithSort_SortAndCount     | InMemory   |   145,560.00 ns |   114,577.396 ns |  29,755.386 ns |   23656 B |
| Combined_ComplexFilter_FilterAndCount    | InMemory   |   205,190.00 ns |   178,267.056 ns |  46,295.389 ns |   29936 B |
| Combined_ComplexSort_SortAndCount        | InMemory   |   163,480.00 ns |    81,512.502 ns |  21,168.538 ns |   34832 B |
| Combined_ComplexFull_FilterSortAndCount  | InMemory   |   228,975.00 ns |   177,870.585 ns |  27,525.670 ns |   40472 B |
| Combined_ComplexGraph_FilterSortAndCount | InMemory   |   189,060.00 ns |   138,539.351 ns |  35,978.230 ns |   28272 B |
| **AddFilter_NoFilters**                      | **PostgreSQL** |     **3,320.00 ns** |     **2,185.050 ns** |     **567.450 ns** |      **88 B** |
| AddFilter_SimpleWithFilter               | PostgreSQL |    56,875.00 ns |    80,584.876 ns |  12,470.599 ns |    1528 B |
| AddFilter_ComplexFilter                  | PostgreSQL |    81,500.00 ns |   103,889.417 ns |  16,077.002 ns |    5768 B |
| AddFilter_ComplexFull                    | PostgreSQL |    79,450.00 ns |    58,597.810 ns |   9,068.076 ns |    4696 B |
| AddFilter_ComplexGraph                   | PostgreSQL |    66,760.00 ns |    68,074.005 ns |  17,678.603 ns |    2504 B |
| AddSort_NoSort                           | PostgreSQL |     3,775.00 ns |     3,310.784 ns |     512.348 ns |      88 B |
| AddSort_SimpleWithSort                   | PostgreSQL |    70,200.00 ns |    29,647.787 ns |   4,588.028 ns |    5440 B |
| AddSort_ComplexSort                      | PostgreSQL |   116,580.00 ns |    45,888.465 ns |  11,917.089 ns |   14600 B |
| AddSort_ComplexFull                      | PostgreSQL |   100,860.00 ns |    46,560.639 ns |  12,091.650 ns |   10816 B |
| AddSort_ComplexGraph                     | PostgreSQL |   177,540.00 ns |    69,246.634 ns |  17,983.131 ns |    6624 B |
| ExtractIncludes_SimpleGraph              | PostgreSQL |    34,100.00 ns |    12,351.249 ns |   1,911.369 ns |    1112 B |
| ExtractIncludes_ComplexGraph             | PostgreSQL |    37,425.00 ns |    46,455.591 ns |   7,189.054 ns |    1672 B |
| GetMemberExpression_Simple               | PostgreSQL |    23,380.00 ns |    19,723.794 ns |   5,122.207 ns |     160 B |
| GetMemberExpression_Nested               | PostgreSQL |    24,840.00 ns |     9,090.293 ns |   2,360.720 ns |     376 B |
| ToCamelCase_Short                        | PostgreSQL |     3,500.00 ns |     2,791.898 ns |     432.049 ns |      32 B |
| ToCamelCase_Long                         | PostgreSQL |     4,175.00 ns |     5,390.373 ns |     834.166 ns |      56 B |
| ToType_Int                               | PostgreSQL |     2,940.00 ns |     2,533.829 ns |     658.027 ns |      24 B |
| ToType_Bool                              | PostgreSQL |       925.00 ns |     2,709.673 ns |     419.325 ns |      24 B |
| ToType_DateTime                          | PostgreSQL |     9,050.00 ns |     7,691.306 ns |   1,190.238 ns |      24 B |
| ToType_String                            | PostgreSQL |       250.00 ns |       646.199 ns |     100.000 ns |         - |
| Combined_Simple_Count                    | PostgreSQL | 1,494,875.00 ns | 1,077,481.820 ns | 166,741.504 ns |    7408 B |
| Combined_SimpleWithFilter_FilterAndCount | PostgreSQL | 1,164,225.00 ns |   408,653.390 ns |  63,239.564 ns |   10544 B |
| Combined_SimpleWithSort_SortAndCount     | PostgreSQL | 1,269,820.00 ns |   620,957.040 ns | 161,260.572 ns |   14032 B |
| Combined_ComplexFilter_FilterAndCount    | PostgreSQL | 1,458,240.00 ns |   670,396.697 ns | 174,099.894 ns |   19848 B |
| Combined_ComplexSort_SortAndCount        | PostgreSQL | 1,348,420.00 ns | 1,446,470.220 ns | 375,643.723 ns |   25176 B |
| Combined_ComplexFull_FilterSortAndCount  | PostgreSQL | 1,158,250.00 ns |   355,936.360 ns |  55,081.546 ns |   30248 B |
| Combined_ComplexGraph_FilterSortAndCount | PostgreSQL | 1,167,325.00 ns |   462,074.072 ns |  71,506.474 ns |   18656 B |
