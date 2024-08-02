using Helpers;

await Test();

async Task Test(int param = 1)
{
    int i = 1; 
    
    ThreadExtensions.PrintCurrentThread(1);
    
    // Task is returned and this runs in parallel on another thread
    // Copies whole stack: local variables, method params, "this" variables and contexts
    await Task.Delay(1_000);
    
    ThreadExtensions.PrintCurrentThread(2);

    // Whole stack is restored after awaiting
    i = 2; 
    param = 2;
}