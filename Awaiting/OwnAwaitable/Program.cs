using System.Diagnostics;
using System.Runtime.CompilerServices;

var sw = Stopwatch.StartNew();
await "2 seconds ⌛";
Console.WriteLine($"Elapsed {sw.ElapsedMilliseconds}ms");

static class Extensions
{
    public static TaskAwaiter GetAwaiter(this string input)
    {
        int amount = (int)char.GetNumericValue(input.FirstOrDefault(char.IsDigit));
        return Task.Delay(TimeSpan.FromSeconds(amount)).GetAwaiter();
    }
}
