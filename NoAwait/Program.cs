using System.Diagnostics;

var stopwatch = Stopwatch.StartNew();

Console.WriteLine($"1. {stopwatch.ElapsedMilliseconds}"); // ~0ms
var delayTask = Task.Delay(1500);
Console.WriteLine($"2. {stopwatch.ElapsedMilliseconds}"); // ~0ms
await Task.Delay(500);
Console.WriteLine($"3. {stopwatch.ElapsedMilliseconds}"); // ~500ms
await delayTask;
Console.WriteLine($"4. {stopwatch.ElapsedMilliseconds}"); // ~1500ms elapsed
