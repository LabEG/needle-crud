# Changelog
<a name="0.2.0"></a>
## [0.2.0](https://github.com/LabEG/needle-crud/compare/v0.2.0...v0.2.0) (2026-02-28)

### ### 🚀 Features

* Add benchmark tests and data generation for library system entities; refactor DbContext and repository ([73d0624](https://github.com/LabEG/needle-crud/commit/73d06248bbcfa135b239349a86f94e002ae77db2))
* Add benchmarking project for NeedleCrud with PagedListQuery benchmarks and update solution file ([389232b](https://github.com/LabEG/needle-crud/commit/389232b34d9690d19676c683d227cfbfd85799a6))
* Add benchmarks for GetPaged and GetPagedComponents methods; update README with benchmark details ([4902004](https://github.com/LabEG/needle-crud/commit/4902004b0a3f886e94cb4a4dd23d665765e990c1))
* Add benchmarks for PagedListQuery and PagedListQueryStruct, enhance exception handling in PagedListQueryFilter and PagedListQuerySort ([a88629d](https://github.com/LabEG/needle-crud/commit/a88629dc950170108b8835b10e9f04cbdf21ec48))
* Add changelog generation workflow and versioning configuration ([73a1697](https://github.com/LabEG/needle-crud/commit/73a169757222af6f1afe56a07666ce777103031a))
* Add comprehensive README documentation for NeedleCrud library and sample application ([08bcf09](https://github.com/LabEG/needle-crud/commit/08bcf09b3ab9282c781af32737c1b81a56f43134))
* Add Dockerfile and .dockerignore for containerization; remove obsolete HTTP file ([0818f28](https://github.com/LabEG/needle-crud/commit/0818f28347320dae6c39632047d635ebfd9a52bf))
* Add initial project structure with essential files including README, LICENSE, and security policy ([fdfccb5](https://github.com/LabEG/needle-crud/commit/fdfccb5c2928d01aecfba981a48b23f746f8a321))
* Add issue templates for bug reports and feature requests; enhance pull request and workflow configurations ([991a315](https://github.com/LabEG/needle-crud/commit/991a315938a94b9845c545dbe1da747aa4c9cb55))
* Add LibraryDbContext and entity classes for library system, including User, Author, Book, Category, Loan, and Review; implement test data generator and update project dependencies ([a4df436](https://github.com/LabEG/needle-crud/commit/a4df436976465171ce4d3f76ad34df85866cf6fd))
* Add NeedleCrud exception handler middleware and update JSON options for controllers ([b807eb1](https://github.com/LabEG/needle-crud/commit/b807eb1a888760b7d4de4857a4cbed3845908d9d))
* Add performance benchmarks for CrudDbRepository GetPaged method and update README for clarity ([d3c21cb](https://github.com/LabEG/needle-crud/commit/d3c21cb930d9685706827e73d382d875a3f0c187))
* Add unit tests for CrudDbRepository.ExtractIncludes method and enhance SQL-injection protection in repository methods ([5b1609e](https://github.com/LabEG/needle-crud/commit/5b1609ea45b4b7ffc1372cf6e39ad631b7701d13))
* Add unit tests for PagedListQuery and update solution file to include test project ([4ea32e1](https://github.com/LabEG/needle-crud/commit/4ea32e132af3b3dbd47dea898710d0acc8a6ab31))
* Enhance CrudDbRepository with performance optimizations and caching; improve ExtractIncludes and ToType methods ([1319c57](https://github.com/LabEG/needle-crud/commit/1319c57277f899526192f5c2b11983002ab07d68))
* Enhance pagination and filtering capabilities with detailed documentation and improved service methods ([6b2084b](https://github.com/LabEG/needle-crud/commit/6b2084b5cfffef27a1e6e072cfed406be040eb22))
* Enhance security documentation with rate limiting and authentication examples; update BooksController for authorization guidance ([edd61e5](https://github.com/LabEG/needle-crud/commit/edd61e541782372a09d3c0f01f99e5fd2d384dd3))
* Enhance User entity and add database context for library system ([71f5f64](https://github.com/LabEG/needle-crud/commit/71f5f6484f44f575350642c440958598b9dd1d43))
* Implement LibraryDbContext and entity classes for library system including User, Author, Book, Category, Loan, and Review ([d495db8](https://github.com/LabEG/needle-crud/commit/d495db8116e9ff499c5c968ff3a68dbd1a9668c8))
* Introduce NeedleCrudException for consistent error handling and update exceptions in CRUD operations ([ddb79bf](https://github.com/LabEG/needle-crud/commit/ddb79bfd6fa34aca9c434e3a4ddb4752ebf6e555))
* Introduce NeedleCrudSettings and enhance pagination with PagedListQuery and PagedListQuerySort ([f6ff870](https://github.com/LabEG/needle-crud/commit/f6ff87094fb52d1280a43a481dcb8b2c7955c0eb))
* Migrate from Newtonsoft.Json to System.Text.Json; update graph handling to use JsonObject ([bdd57b3](https://github.com/LabEG/needle-crud/commit/bdd57b392e4550a057dcccbda1928c8ba67d7715))
* Refactor controllers to accept NeedleCrudSettings via constructor for enhanced configuration ([014daac](https://github.com/LabEG/needle-crud/commit/014daac4ce0fd0ec601bfaba1506cc22e080794a))
* Refactor CRUD operations and improve code readability across controllers and repositories ([c7f446c](https://github.com/LabEG/needle-crud/commit/c7f446cb09c89d29b659a74dedd815b1f999de7e))
* Refactor graph handling to use JsonDocument instead of JsonObject across repositories and services ([29e7b30](https://github.com/LabEG/needle-crud/commit/29e7b30a57b3adcf1db1d6274e7d4b7bcaa3dfbf))
* Refactor PagedListQuery to use init-only properties and add parsing methods for filters and sorts ([e0f37f6](https://github.com/LabEG/needle-crud/commit/e0f37f63f47316267494b1e51a3d346f0f0bb289))
* Replace ObjectNotFoundException with ObjectNotFoundNeedleCrudException for improved error handling ([3b2c762](https://github.com/LabEG/needle-crud/commit/3b2c76296d2d1f2c89e46b2de555ff8de824459f))
* Update changelog configuration and remove deprecated versionize.json ([51b22be](https://github.com/LabEG/needle-crud/commit/51b22be30fcaca872c424f28bcee98e728f06cf0))
* Update entity classes to use Guid for Id properties; add benchmarks for CrudDbRepository methods and update README with usage instructions ([17bd64d](https://github.com/LabEG/needle-crud/commit/17bd64dcab118364c4224d19e625008eba547f8c))
* Update GetAll method signatures to return arrays instead of lists for improved performance ([8b88c6a](https://github.com/LabEG/needle-crud/commit/8b88c6ac2c9f6c419343a257d0ba674d2500070e))

### ### 🐛 Bug Fixes

* Correct tool name from dotnet-versionize to versionize in changelog workflow ([cd9d620](https://github.com/LabEG/needle-crud/commit/cd9d62030a08b73e7d410d67e695110ce434ed80))

### ### ♻️ Refactoring

* Remove FluentAssertions dependency and update assertions in PagedListQueryTests ([d01b045](https://github.com/LabEG/needle-crud/commit/d01b0455a1b6b1528579e3ee17803d38fff38ac3))
* Remove unused repository and service classes, update CRUD repository interface with detailed XML documentation ([53806b1](https://github.com/LabEG/needle-crud/commit/53806b1db61dd0fd22868620ee768f68386daa0b))
* Update CRUD controller methods to return simplified types and remove unnecessary validation checks ([ee4f514](https://github.com/LabEG/needle-crud/commit/ee4f514f9db3afa532c347e61256fad96defbb76))
* Update property name handling in pagination models to preserve case sensitivity ([94c3fd3](https://github.com/LabEG/needle-crud/commit/94c3fd3f3367b0e3eaea042750f5b4018b32c274))

<a name="0.1.0"></a>
## [0.1.0](https://github.com/LabEG/needle-crud/compare/v0.1.0...v0.1.0) (2026-02-24)

### ### 🚀 Features

* Add benchmark tests and data generation for library system entities; refactor DbContext and repository ([73d0624](https://github.com/LabEG/needle-crud/commit/73d06248bbcfa135b239349a86f94e002ae77db2))
* Add benchmarking project for NeedleCrud with PagedListQuery benchmarks and update solution file ([389232b](https://github.com/LabEG/needle-crud/commit/389232b34d9690d19676c683d227cfbfd85799a6))
* Add benchmarks for GetPaged and GetPagedComponents methods; update README with benchmark details ([4902004](https://github.com/LabEG/needle-crud/commit/4902004b0a3f886e94cb4a4dd23d665765e990c1))
* Add benchmarks for PagedListQuery and PagedListQueryStruct, enhance exception handling in PagedListQueryFilter and PagedListQuerySort ([a88629d](https://github.com/LabEG/needle-crud/commit/a88629dc950170108b8835b10e9f04cbdf21ec48))
* Add changelog generation workflow and versioning configuration ([73a1697](https://github.com/LabEG/needle-crud/commit/73a169757222af6f1afe56a07666ce777103031a))
* Add comprehensive README documentation for NeedleCrud library and sample application ([08bcf09](https://github.com/LabEG/needle-crud/commit/08bcf09b3ab9282c781af32737c1b81a56f43134))
* Add Dockerfile and .dockerignore for containerization; remove obsolete HTTP file ([0818f28](https://github.com/LabEG/needle-crud/commit/0818f28347320dae6c39632047d635ebfd9a52bf))
* Add initial project structure with essential files including README, LICENSE, and security policy ([fdfccb5](https://github.com/LabEG/needle-crud/commit/fdfccb5c2928d01aecfba981a48b23f746f8a321))
* Add issue templates for bug reports and feature requests; enhance pull request and workflow configurations ([991a315](https://github.com/LabEG/needle-crud/commit/991a315938a94b9845c545dbe1da747aa4c9cb55))
* Add LibraryDbContext and entity classes for library system, including User, Author, Book, Category, Loan, and Review; implement test data generator and update project dependencies ([a4df436](https://github.com/LabEG/needle-crud/commit/a4df436976465171ce4d3f76ad34df85866cf6fd))
* Add NeedleCrud exception handler middleware and update JSON options for controllers ([b807eb1](https://github.com/LabEG/needle-crud/commit/b807eb1a888760b7d4de4857a4cbed3845908d9d))
* Add unit tests for PagedListQuery and update solution file to include test project ([4ea32e1](https://github.com/LabEG/needle-crud/commit/4ea32e132af3b3dbd47dea898710d0acc8a6ab31))
* Enhance CrudDbRepository with performance optimizations and caching; improve ExtractIncludes and ToType methods ([1319c57](https://github.com/LabEG/needle-crud/commit/1319c57277f899526192f5c2b11983002ab07d68))
* Enhance pagination and filtering capabilities with detailed documentation and improved service methods ([6b2084b](https://github.com/LabEG/needle-crud/commit/6b2084b5cfffef27a1e6e072cfed406be040eb22))
* Enhance security documentation with rate limiting and authentication examples; update BooksController for authorization guidance ([edd61e5](https://github.com/LabEG/needle-crud/commit/edd61e541782372a09d3c0f01f99e5fd2d384dd3))
* Enhance User entity and add database context for library system ([71f5f64](https://github.com/LabEG/needle-crud/commit/71f5f6484f44f575350642c440958598b9dd1d43))
* Implement LibraryDbContext and entity classes for library system including User, Author, Book, Category, Loan, and Review ([d495db8](https://github.com/LabEG/needle-crud/commit/d495db8116e9ff499c5c968ff3a68dbd1a9668c8))
* Introduce NeedleCrudException for consistent error handling and update exceptions in CRUD operations ([ddb79bf](https://github.com/LabEG/needle-crud/commit/ddb79bfd6fa34aca9c434e3a4ddb4752ebf6e555))
* Migrate from Newtonsoft.Json to System.Text.Json; update graph handling to use JsonObject ([bdd57b3](https://github.com/LabEG/needle-crud/commit/bdd57b392e4550a057dcccbda1928c8ba67d7715))
* Refactor CRUD operations and improve code readability across controllers and repositories ([c7f446c](https://github.com/LabEG/needle-crud/commit/c7f446cb09c89d29b659a74dedd815b1f999de7e))
* Refactor PagedListQuery to use init-only properties and add parsing methods for filters and sorts ([e0f37f6](https://github.com/LabEG/needle-crud/commit/e0f37f63f47316267494b1e51a3d346f0f0bb289))
* Replace ObjectNotFoundException with ObjectNotFoundNeedleCrudException for improved error handling ([3b2c762](https://github.com/LabEG/needle-crud/commit/3b2c76296d2d1f2c89e46b2de555ff8de824459f))
* Update changelog configuration and remove deprecated versionize.json ([51b22be](https://github.com/LabEG/needle-crud/commit/51b22be30fcaca872c424f28bcee98e728f06cf0))
* Update entity classes to use Guid for Id properties; add benchmarks for CrudDbRepository methods and update README with usage instructions ([17bd64d](https://github.com/LabEG/needle-crud/commit/17bd64dcab118364c4224d19e625008eba547f8c))
* Update GetAll method signatures to return arrays instead of lists for improved performance ([8b88c6a](https://github.com/LabEG/needle-crud/commit/8b88c6ac2c9f6c419343a257d0ba674d2500070e))

### ### 🐛 Bug Fixes

* Correct tool name from dotnet-versionize to versionize in changelog workflow ([cd9d620](https://github.com/LabEG/needle-crud/commit/cd9d62030a08b73e7d410d67e695110ce434ed80))

### ### ♻️ Refactoring

* Remove FluentAssertions dependency and update assertions in PagedListQueryTests ([d01b045](https://github.com/LabEG/needle-crud/commit/d01b0455a1b6b1528579e3ee17803d38fff38ac3))
* Remove unused repository and service classes, update CRUD repository interface with detailed XML documentation ([53806b1](https://github.com/LabEG/needle-crud/commit/53806b1db61dd0fd22868620ee768f68386daa0b))
* Update CRUD controller methods to return simplified types and remove unnecessary validation checks ([ee4f514](https://github.com/LabEG/needle-crud/commit/ee4f514f9db3afa532c347e61256fad96defbb76))
* Update property name handling in pagination models to preserve case sensitivity ([94c3fd3](https://github.com/LabEG/needle-crud/commit/94c3fd3f3367b0e3eaea042750f5b4018b32c274))

