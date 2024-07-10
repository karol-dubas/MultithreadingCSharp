try
{
    ToggleBulbState();
}
catch
{
    Console.WriteLine("Top level handled exception"); // Won't happen
}

Console.ReadKey();

async void ToggleBulbState() // async void method
{
    // When working with forced async void method, whole code must be in try, catch, finally blocks,
    // without any throw, so it makes sure no exception is thrown back to the caller (prevents app crash)
    
    try
    {
        var bulb = new LightBulb();
        var toggleTask = bulb.Toggle();
        await toggleTask; // Exception re-thrown with await
    }
    catch
    {
        // The return type is void, not a Task, exception can't be set on a void,
        // so it's thrown back to the caller and the app crashes.
        // Returning an exception in Task would be correct with async Task (compiler does it automatically).
        
        // Will crash the app
        //throw;
        
        Console.WriteLine($"{nameof(ToggleBulbState)} handled exception");
    }
}

class LightBulb
{
    public async Task Toggle()
    {
        await Task.Delay(1_000);
        throw new Exception("Random exception");
        Console.WriteLine("State toggled");
    }
}