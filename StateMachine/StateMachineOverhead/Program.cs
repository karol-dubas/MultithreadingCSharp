try
{
    // The async keyword "forces" to use a Task, which should be awaited.
    // It re-throws an exception, but it won't return any control to the caller.
    await TestAsync();
}
catch (Exception e)
{
    Console.WriteLine($"Async exception caught: {e.Message}");
}

try
{
    Test();
}
catch (Exception e)
{
    Console.WriteLine($"Exception caught: {e.Message}");
}

// There is no await inside a method, so it isn't an asynchronous method.
// The async keyword isn't needed here, it forces to return a Task, and spawns an unnecessary state machine.
async Task TestAsync()
{
    throw new Exception("Oops");
    Console.WriteLine("Inside a method");
}

void Test()
{
    throw new Exception("Oops");
    Console.WriteLine("Inside a method");
}