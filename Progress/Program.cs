int count = 30;

var progress = new Progress<int>(p =>
{
    Console.Clear();

     for (int j = 0; j < p; j++)
         Console.Write("█");
    
     for (int j = p; j < count; j++)
         Console.Write("▒");

     Console.WriteLine();
});

await RunLongProcess(progress);

async Task RunLongProcess(IProgress<int> progress)
{
    for (int i = 1; i <= count; i++)
    {
        await Task.Delay(Random.Shared.Next(1, 100));
        progress.Report(i);
    }
}