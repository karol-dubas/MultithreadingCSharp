// ValueTask is a structure for representing asynchronous operations, that can complete successfully or with an error.
// It is optimized for performance, which is useful when the operation might complete quickly,
// allowing for reduced memory allocations compared to a Task.

var cache = new Cache();

Console.WriteLine($"Result: {await cache.GetValueAsync()}");
Console.WriteLine($"Result: {await cache.GetValueAsync()}");
Console.WriteLine($"Result: {await cache.GetValueAsync()}");

public class Cache
{
    private int? _cachedValue;

    public async ValueTask<int> GetValueAsync()
    {
        // Hot path doesn't call await.
        // The result is already available, and the operation is synchronous,
        // memory allocation overhead can be reduced with a ValueTask.
        if (_cachedValue.HasValue)
            return _cachedValue.Value;

        await Task.Delay(500); // async operation
        _cachedValue = Random.Shared.Next(100);
        return _cachedValue.Value;
    }
}
