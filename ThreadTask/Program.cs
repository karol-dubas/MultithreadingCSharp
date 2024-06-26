// In modern .NET tasks are used instead of threads directly.

// ThreadPool manages creating appropriate amount of threads needed to run created tasks. Threads run the tasks.
// Threads can be described as an opportunity to execute a code when placed on the CPU, which actually runs the code.
// Task -> TaskScheduler -> ThreadPool -> Thread -> CPU.

// Blocking the Task blocks the Thread, blocked Thread can't pick up any other Task.
// CPU can switch threads, but ThreadPool can't do the same with tasks, once it's attached it can't be detached.
// The CPU will try to perform operations on the Thread that is blocked and in effect it won't do anything.

// TODO: TaskScheduler.Current vs TaskScheduler.Default (ThreadPool)?

// TODO: add an example

Console.WriteLine("bop");
