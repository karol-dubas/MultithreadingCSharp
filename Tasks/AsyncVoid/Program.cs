Console.WriteLine("Before");

try
{
    Test(); // Unhandled exception, app crashes
    //Test2(); // Lost exception
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