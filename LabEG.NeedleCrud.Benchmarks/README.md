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
```

### Запуск с дополнительными опциями

```bash
# Только бенчмарк ParseComplexFilter
dotnet run -c Release -- --filter *ParseComplexFilter

# С экспортом результатов в разные форматы
dotnet run -c Release -- --exporters json html
```

## Доступные бенчмарки

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

## Документация

- [BenchmarkDotNet Documentation](https://benchmarkdotnet.org/articles/overview.html)
- [Best Practices](https://benchmarkdotnet.org/articles/guides/good-practices.html)
