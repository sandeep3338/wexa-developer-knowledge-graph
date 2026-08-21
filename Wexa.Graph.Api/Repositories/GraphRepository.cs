using Neo4j.Driver;

namespace Wexa.Graph.Api.Repositories;

public class GraphRepository
{
    private readonly IDriver _driver;

    public GraphRepository(IDriver driver)
    {
        _driver = driver;
    }

    public async Task<IReadOnlyList<T>> ExecuteAsync<T>(
        string query,
        Func<IRecord, T> mapper,
        object? parameters = null)
    {
        await using var session = _driver.AsyncSession();

        var result = await session.RunAsync(
            query,
            parameters);

        var records = await result.ToListAsync();

        return records
            .Select(mapper)
            .ToList();
    }
}