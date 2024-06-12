var task = new Task(() => { Thread.Sleep(10); });

Console.WriteLine(task.Status);

task.Start();

while (true)
{
    Console.WriteLine(task.Status);

    if (task.IsCompleted)
    {
        Console.WriteLine(task.Status);
        break;
        
    }
    
    Thread.Sleep(1);
}

