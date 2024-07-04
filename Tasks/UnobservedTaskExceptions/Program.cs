﻿class Program
{
    public static void Main()
    {
        TaskScheduler.UnobservedTaskException += (_, eventArgs) =>
        {
            eventArgs.Exception.Handle(ex =>
            {
                Console.WriteLine("UnobservedTaskExceptionType: {0}", ex.GetType());
                return true;
            });
            eventArgs.SetObserved();
        };

        StartTasks();

        // Must be collected by Garbage Collector
        Thread.Sleep(100);
        GC.Collect();
        GC.WaitForPendingFinalizers();

        Console.WriteLine("Done");
        Console.ReadKey();
    }

    private static void StartTasks()
    {
        Task.Run(() => throw new Exception());
    }
}