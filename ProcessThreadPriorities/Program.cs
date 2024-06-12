using System.Diagnostics;

Process p = Process.GetCurrentProcess();
p.PriorityClass = ProcessPriorityClass.High;

Thread t = Thread.CurrentThread;
t.Priority = ThreadPriority.Highest;

Console.WriteLine();