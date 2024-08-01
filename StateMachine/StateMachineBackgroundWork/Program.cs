Test();

// This loop won't be executed when Test method has no await
while (true) 
{
    Thread.Sleep(500);
    Console.WriteLine("Main program");
}

async Task Test()
{
    // If a SynchronizationContext or specific TaskScheduler is present,
    // the state machine is resumed on the appropriate thread, such as the UI thread.
    // Otherwise, it resumes on a thread from the ThreadPool, possibly different from the original.
    
    // Method won't execute in the background until it hits the first await,
    // (work won't be executed on another thread), because control isn't returned to the caller.
    // This await will cause the rest of the method to be executed on another thread.
    await Task.Delay(1); // Comment/uncomment to see the difference
    
    // We can replace the Task.Delay(1) with the Task.Yield(), which was created for such purpose.
    //await Task.Yield(); // Comment/uncomment to see the difference
    
    while (true)
    {
        Thread.Sleep(500);
        Console.WriteLine("Inside a method");
    }
}

// TODO: Test() with Task.Yield vs Task.Run(Test) without Task.Yield etc.
