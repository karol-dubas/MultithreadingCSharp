//Test(); // TODO: but does it allow to run in the background, even though there is no await keyword?
// TODO: Test() with Task.Yield vs Task.Run(Test) without Task.Yield

// There is no await, so it isn't an asynchronous method.
// The async keyword isn't needed here, it forces to return a Task, and spawns a state machine.
async Task<int> Test()
{
    Console.WriteLine("Inside a method");
    return 1;
}