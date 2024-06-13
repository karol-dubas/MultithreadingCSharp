// Multiple things performing work on the same shared resource

int number = 0;

List<Task> tasks = [];

for (int i = 0; i < 1000; i++)
{
    var task = Task.Run(() => { number++; }); // Race condition
    tasks.Add(task);
}

await Task.WhenAll(tasks);

Console.WriteLine(number);