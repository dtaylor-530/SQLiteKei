using SQLite.Service.Repository;
using Utility.Chart;
using Utility.Database;

namespace SQLite.Service.Service;

public class ColumnSeriesService
{
    private readonly SeriesRepository repository;
    private Dictionary<TableKey, List<Series>> collection = new();

    public ColumnSeriesService(SeriesRepository repository)
    {
        this.repository = repository;
    }

    public IReadOnlyCollection<Series> Get(TableKey key)
    {
        var aa = collection.GetValueOrDefault(key) ?? repository.Load(key);
        return collection[key] = aa.ToList();

    }
    public void Set(TableKey key, List<Series> value)
    {
        collection[key] = collection.GetValueOrDefault(key) ?? new List<Series>();
        collection[key].Clear();
        foreach (var item in value)
            collection[key].Add(item);
        //tableSeriesPairs.OnNext(new(key.TableName, collection[key]));
        repository.Save(key, value);
    }
}
