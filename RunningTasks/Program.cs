new Task(() => Console.WriteLine("Task.Start")).Start(); // Legacy

await Task.Factory.StartNew(() => 
    Console.WriteLine("Task.Factory.StartNew")); // Older, more extended Task.Run option, rarely used

await Task.Run(() => Console.WriteLine("Task.Run")); // Currently used with async/await
