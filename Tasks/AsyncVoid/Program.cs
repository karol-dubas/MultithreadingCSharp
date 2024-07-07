Console.WriteLine("Before");

try
{
    // Unhandled exception, app crashes.
    // An exception is thrown, but it's "outside" (code execution continues) the try catch block.
    // Its fire and forget, once it's started operation can't be managed (can't use await).
    Test(); 
    
    // await can be used only on Task, because it's a wrapper and a pointer to asynchronous operation
    //await Test();
    
    // An exception is still thrown, now a Task object wraps & contains it,
    // without await it gets lost, but at least it doesn't crash the app.
    Test2();
}
catch (Exception)
{
    Console.WriteLine("Exception caught");
}

Console.WriteLine("After");
Console.ReadKey();

async void Test()
{
    await Task.Delay(100);
    throw new Exception();
}

async Task Test2()
{
    await Task.Delay(100);
    throw new Exception();
}