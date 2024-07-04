var task = new Task(() => Console.WriteLine("Task.Start")); // Legacy
task.Start();
await task;

await Task.Factory.StartNew(() => 
    Console.WriteLine("Task.Factory.StartNew")); // Older, more configurable Task.Run option, rarely used

await Task.Run(() => Console.WriteLine("Task.Run")); // Currently used with async/await
