Console.WriteLine("Start");

await Task.Factory.StartNew(() => // Won't complete until all attached child tasks complete
{
    Task.Factory.StartNew(() => // Child task
    {
        Thread.Sleep(500);
        Console.WriteLine("Completed 1");
    }, TaskCreationOptions.AttachedToParent); // Attach to the parent, await will wait for this one
    
    Console.WriteLine("Waiting for child task to complete...");
});

Console.WriteLine("Press any key to continue...");
Console.ReadKey(true);

await Task.Factory.StartNew(() => 
{
    Task.Factory.StartNew(() =>
    {
        Thread.Sleep(500);
        Console.WriteLine("Completed 2");
    }, TaskCreationOptions.AttachedToParent);
    
}, TaskCreationOptions.DenyChildAttach); // Block attaching, won't wait for child tasks

Console.WriteLine("Press any key to continue...");
Console.ReadKey(true);

await Task.Run(() => // Task.Run by default doesn't allow to attach a child task
{
    Task.Factory.StartNew(() =>
    {
        Thread.Sleep(500);
        Console.WriteLine("Completed 3");
    }, TaskCreationOptions.AttachedToParent);
});

Console.WriteLine("Press any key to exit...");
Console.ReadKey(true);
