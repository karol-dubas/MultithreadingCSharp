Console.WriteLine("Before");

try
{
    // An exception is still thrown, now a Task object wraps & contains it,
    // without await it gets lost or garbage collected,
    // but at least app continues execution, and it doesn't crash the app.
    /*await*/ TaskMethod();
    
    // Unhandled exception, app crashes.
    // An exception is thrown, but it's "outside" (code execution continues) the try catch block.
    // Its fire and forget, once it's started operation can't be managed (can't use await).
    VoidMethod(); 
    
    // await can be used only on Task (GetAwaiter),
    // because it's a wrapper and a pointer to asynchronous operation.
    // async void method can't be awaited.
    //await VoidMethod();
    
    // async void should be used only for event handlers
}
catch (Exception)
{
    Console.WriteLine("Exception caught");
}

Console.WriteLine("After");
Console.ReadKey();

async void VoidMethod()
{
    await Task.Delay(100);
    throw new Exception();
}

async Task TaskMethod()
{
    await Task.Delay(100);
    throw new Exception();
}