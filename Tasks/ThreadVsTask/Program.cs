using System.Diagnostics;
using ThreadVsTask;

// In modern .NET tasks are used instead of threads directly, but it may cause problems.

int count = 16;

var sw = Stopwatch.StartNew();
await new TaskExample().Run(count);
Console.WriteLine(sw.ElapsedMilliseconds);

sw.Reset();
new ThreadExample().Run(count);
Console.WriteLine(sw.ElapsedMilliseconds);
