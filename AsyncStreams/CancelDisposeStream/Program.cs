using System.Runtime.CompilerServices;

var cts = new CancellationTokenSource();
cts.CancelAfter(TimeSpan.FromSeconds(3));

Console.WriteLine("Started");

// Resources can be cleaned up asynchronously by implementing IAsyncDisposable and await using
await using var service = new Service();
var numbersEnumerator = service.GetNumbers(); // not passing a CancellationToken, it is passed by IAsyncEnumerable.WithCancellation

// https://stackoverflow.com/questions/59300561/whats-the-difference-between-returning-asyncenumerable-with-enumeratorcancellat
// EnumeratorCancellationAttribute is used with IAsyncEnumerable.WithCancellation
await foreach (int number in numbersEnumerator.WithCancellation(cts.Token))
{
    Console.WriteLine($"Received number: {number}");
}

Console.WriteLine("Finished");

public class Service : IAsyncDisposable
{
    public async IAsyncEnumerable<int> GetNumbers([EnumeratorCancellation] CancellationToken ct = default)
    {
        Console.WriteLine("Start sending numbers...");

        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(1_000);
            yield return Random.Shared.Next(10);
        }
        
        Console.WriteLine("Cancellation requested, stopped sending numbers");
    }

    public async ValueTask DisposeAsync()
    {
        Console.WriteLine("Disposal started...");
        await Task.Delay(1_000);
        Console.WriteLine("Service disposed");
    }
}