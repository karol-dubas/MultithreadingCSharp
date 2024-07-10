using Helpers;

var sc = SynchronizationContext.Current;
Console.WriteLine($"Is SynchronizationContext null: {sc is null}"); // doesn't exists

await RunOne();
//await RunTwo();
Console.WriteLine();
return;

async Task RunOne()
{
    ThreadExtensions.PrintCurrentThread(1);

    await Task.Run(async () =>
    {
        ThreadExtensions.PrintCurrentThread(2);

        await Task.Run(() =>
        {
            ThreadExtensions.PrintCurrentThread(3);
        });
    
        ThreadExtensions.PrintCurrentThread(4); // continues execution on the latest used thread
    });

    ThreadExtensions.PrintCurrentThread(5); // continues execution on the latest used thread
}

async Task RunTwo()
{
    ThreadExtensions.PrintCurrentThread(1);

    await Task.Run(async () =>
    {
        ThreadExtensions.PrintCurrentThread(2);

        await Task.Run(() =>
        {
            ThreadExtensions.PrintCurrentThread(3);
        }).ConfigureAwait(false); // no effect
    
        ThreadExtensions.PrintCurrentThread(4);
    }).ConfigureAwait(false); // no effect

    ThreadExtensions.PrintCurrentThread(5);
}

