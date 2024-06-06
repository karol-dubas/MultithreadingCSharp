Console.WriteLine("start");

new Task(() => Console.WriteLine("test1")).Start();

Task.Run(() => Console.WriteLine("test2"));

await new Task(() => Console.WriteLine("test3")); // mixing approaches - crashes the app

Console.WriteLine("end");
Console.ReadKey();