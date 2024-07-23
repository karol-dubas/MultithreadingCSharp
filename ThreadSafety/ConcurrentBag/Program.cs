using System.Collections.Concurrent;

const int taskCount = 10;
const int itemsToAdd = 10;

List<int> list = [];
ConcurrentBag<int> concurrentBag = [];
List<Task> tasks = [];

for (int i = 0; i < taskCount; i++)
{
    var task = Task.Run(() =>
    {
        for (int j = 0; j < itemsToAdd; j++)
        {
            list.Add(1);
            concurrentBag.Add(1);
        }
    });

    tasks.Add(task);
}

await Task.WhenAll(tasks);

Console.WriteLine($"List: {list.Count}/{taskCount * itemsToAdd}");
Console.WriteLine($"ConcurrentBag: {concurrentBag.Count}/{taskCount * itemsToAdd}");