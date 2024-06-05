Console.WriteLine("start");

Task.Run(() =>
{
    Thread.Sleep(500);
    Console.WriteLine("1");
});

await Task.Run(() =>
{
    Thread.Sleep(1000);
    Console.WriteLine("2");
});

Task.Run(async () =>
{
    await Task.Delay(1500);
    Console.WriteLine("3");
});

Console.WriteLine("end");
Console.ReadKey();