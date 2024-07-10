namespace Helpers;

public static class ThreadExtensions
{
    public static void PrintInfo(this Thread thread)
    {
        string info = $"""
                       -----------------------------------
                       Name: {thread.Name}
                       ManagedThreadId: {thread.ManagedThreadId}
                       IsBackground: {thread.IsBackground}
                       IsThreadPoolThread: {thread.IsThreadPoolThread}
                       ThreadState: {thread.ThreadState}
                       -----------------------------------
                       """;

        Console.WriteLine(info);
    }

    public static void PrintShortInfo(this Thread thread)
    {
        string info = $"Id: {thread.ManagedThreadId}, ThreadState: {thread.ThreadState}";
        Console.WriteLine(info);
    }
    
    public static void PrintCurrentThread(double number) 
        => Console.WriteLine($"[{number}] Thread: " + Environment.CurrentManagedThreadId);
}