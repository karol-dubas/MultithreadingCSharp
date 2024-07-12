const int length = 10;
var timeout = TimeSpan.FromSeconds(1);

CancellationTokenSource cts1 = new();
cts1.CancelAfter(timeout);

int[] result1 = GetData(length, cts1.Token);
Console.WriteLine($"Result 1: [{string.Join(", ", result1)}]");

CancellationTokenSource cts2 = new();
cts2.CancelAfter(timeout);
int[] result2 = [];
try
{
    result2 = GetDataException(length, cts2.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Operation was canceled");
}

Console.WriteLine($"Result 2: [{string.Join(", ", result2)}]");
return;

// Throws an exception on cancellation
int[] GetDataException(int count, CancellationToken ct = default)
{
    int[] result = new int[count];

    for (int i = 0; i < count; i++)
    {
        if (ct.IsCancellationRequested)
            ct.ThrowIfCancellationRequested();
        
        Thread.Sleep(200);
        result[i] = Random.Shared.Next(100);
    }

    return result;
}

// Returns partial data on cancellation
int[] GetData(int count, CancellationToken ct = default)
{
    int[] result = new int[count];
    int i = 0;

    while (i < count && !ct.IsCancellationRequested)
    {
        Thread.Sleep(200);
        result[i] = Random.Shared.Next(100);
        i++;
    }

    return result;
}