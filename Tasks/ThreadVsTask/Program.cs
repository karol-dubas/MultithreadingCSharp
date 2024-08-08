using System.Diagnostics;
using ThreadVsTask;

// https://youtu.be/nka_qn6bDWU?si=iX_DafKR5xq3Uocd

// In modern .NET tasks are used instead of threads directly, but it may cause problems.

int count = 16;

var sw = Stopwatch.StartNew();
await new TaskExample().Run(count);
Console.WriteLine(sw.ElapsedMilliseconds);

sw.Reset();
new ThreadExample().Run(count);
Console.WriteLine(sw.ElapsedMilliseconds);
