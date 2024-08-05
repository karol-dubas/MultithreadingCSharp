using System.Collections.Concurrent;
using Helpers;

// https://youtu.be/E98p-sABx2I?si=FV4_s4-uhMyUBMVl

var threadsUsed = new ConcurrentDictionary<int, List<int>>();

var results = Enumerable.Range(0, 10)
    .AsParallel() // try to parallelize, alternatively use ParallelEnumerable
    .WithDegreeOfParallelism(2)
    .Select(HeavyComputation);

results.ForAll(number =>
{
    Console.WriteLine($"Executing '{number}' on a thread '{Environment.CurrentManagedThreadId}'");
    
    threadsUsed.AddOrUpdate(
        key: Environment.CurrentManagedThreadId,
        addValue: [number],
        updateValueFactory: (_, values) => [..values, number]);
});

// Order isn't preserved, because it's executed in parallel, to force it use .AsOrdered()
results.Dump();
threadsUsed.Dump();

int HeavyComputation(int n)
{
    for (long i = 0; i < 100_000_000; i++)
    {
        n++;
        n--;
    }

    return n;
}