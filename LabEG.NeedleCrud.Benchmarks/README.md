# LabEG.NeedleCrud.Benchmarks

Проект для измерения производительности компонентов библиотеки NeedleCrud с использованием [BenchmarkDotNet](https://benchmarkdotnet.org/).

## Запуск бенчмарков

### Запуск всех бенчмарков

```bash
dotnet run -c Release
```

### Запуск конкретного бенчмарка

```bash
dotnet run -c Release -- --filter *PagedListQuery*
dotnet run -c Release -- --filter *CrudDbRepository*
```

### Запуск CrudDbRepository бенчмарков

Для бенчмарков с InMemory:
```bash
dotnet run -c Release -- --filter *CrudDbRepository* --runtimes net8.0
```

Для бенчмарков с PostgreSQL (требуется запущенный контейнер):
```bash
# Запустить PostgreSQL
docker run -d --name needlecrud-postgres \
  -e POSTGRES_DB=needlecrud-test \
  -e POSTGRES_USER=needlecrud-test \
  -e POSTGRES_PASSWORD=needlecrud-test \
  -p 5432:5432 \
  postgres:latest

# Запустить бенчмарк
dotnet run -c Release -- --filter *CrudDbRepository*

# Остановить PostgreSQL
docker stop needlecrud-postgres
```

Или использовать готовый скрипт:
```bash
# Linux/macOS
./run-crud-benchmarks.sh postgres

# Windows PowerShell
.\run-crud-benchmarks.ps1 -postgres
```

### Запуск с дополнительными опциями

```bash
# Только бенчмарк ParseComplexFilter
dotnet run -c Release -- --filter *ParseComplexFilter

# С экспортом результатов в разные форматы
dotnet run -c Release -- --exporters json html

# Запуск с конкретным методом
dotnet run -c Release -- --filter *CrudDbRepository.Create*
```

## Доступные бенчмарки

### CrudDbRepositoryBenchmarks

Измеряет производительность основных CRUD операций с базой данных:

- `Create` - создание новой сущности
- `GetById` - получение сущности по ID
- `GetAll` - получение всех сущностей
- `Update` - обновление существующей сущности
- `Delete` - удаление сущности

**Параметры:**
- `Provider`: InMemory, PostgreSQL

**Особенности:**
- База данных создается один раз в GlobalSetup для каждого провайдера
- Между итерациями данные очищаются и пересоздаются (RemoveRange + AddRange)
- ChangeTracker очищается перед и после каждой итерации для избежания конфликтов
- Тестовые данные генерируются один раз и используются во всех итерациях
- Используется 100 книг, 20 авторов, 10 пользователей и другие связанные сущности
- Настроен SimpleJob с 1 warmup и 5 итерациями для стабильных результатов

**Примечание:** Для запуска бенчмарка с PostgreSQL необходимо запустить контейнер:
```bash
docker run -d -it --rm --name needlecrud-postgres \
  -e POSTGRES_DB=needlecrud-test \
  -e POSTGRES_USER=needlecrud-test \
  -e POSTGRES_PASSWORD=needlecrud-test \
  -p 5432:5432 \
  postgres:latest
```

### PagedListQueryBenchmarks

Измеряет производительность парсинга параметров запроса:

- `ParseSimpleFilter` - парсинг одного фильтра
- `ParseComplexFilter` - парсинг нескольких фильтров
- `ParseSimpleSort` - парсинг одной сортировки
- `ParseComplexSort` - парсинг нескольких сортировок
- `ParseSimpleGraph` - парсинг простого JSON графа
- `ParseComplexGraph` - парсинг вложенного JSON графа
- `ParseUrlEncodedFilter` - парсинг URL-encoded значений
- `ParseAllParameters` - парсинг всех параметров одновременно
- `ParseMinimal` - baseline (без параметров)

## Результаты

Результаты бенчмарков сохраняются в папке `BenchmarkDotNet.Artifacts/results/`.

## Добавление новых бенчмарков

Создайте новый класс с атрибутом `[MemoryDiagnoser]` и методами с атрибутом `[Benchmark]`:

```csharp
using BenchmarkDotNet.Attributes;

[MemoryDiagnoser]
public class MyBenchmarks
{
    [Benchmark]
    public void MyMethod()
    {
        // код для измерения
    }
}
```

## Полезные атрибуты

- `[MemoryDiagnoser]` - показывает выделение памяти
- `[SimpleJob]` - настройка количества прогонов
- `[Benchmark(Baseline = true)]` - сравнение с baseline
- `[Params(...)]` - параметризация бенчмарка
- `[GlobalSetup]` - выполняется один раз перед всеми итерациями
- `[IterationSetup]` - выполняется перед каждой итерацией
- `[IterationCleanup]` - выполняется после каждой итерации

## Документация

- [BenchmarkDotNet Documentation](https://benchmarkdotnet.org/articles/overview.html)
- [Best Practices](https://benchmarkdotnet.org/articles/guides/good-practices.html)
