Console.WriteLine("start");

await Task.Run(() => Console.WriteLine("test1"));

var task = new Task(() => Console.WriteLine("test2"));
task.Start();
await task;

Console.WriteLine("end");
Console.ReadKey();