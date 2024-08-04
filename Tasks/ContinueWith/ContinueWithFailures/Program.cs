for (int i = 0; i < 5; i++)
{
    var task = Task.Run(async () =>
    {
        await Task.Delay(Random.Shared.Next(500));
        return Random.Shared.Next(2) == 1 ? 1 : throw new Exception("Oops");
    });

    // This code below should be refactored to a try catch and Task completion status check
    
    task.ContinueWith(t =>
    {
        Console.WriteLine(
            $"Task faulted. Messages: " +
            $"[{string.Join(", ", t?.Exception?.InnerExceptions.Select(x => x.Message) ?? Array.Empty<string>())}]");
    }, TaskContinuationOptions.OnlyOnFaulted);

    task.ContinueWith(t =>
    {
        Console.WriteLine($"Result = {t.Result}");
    }, TaskContinuationOptions.OnlyOnRanToCompletion);
}

Console.WriteLine("Waiting for results...");
Console.ReadLine();