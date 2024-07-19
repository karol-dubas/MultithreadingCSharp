# Multithreading

- Program is run in a thread (main thread)
- Hyper-Threading/SMT allows CPU core to handle both threads at the same time
- CPU thread priorities are managed by OS, but it can be configured
- Program performance using multiple threads depends on what hardware it is running on.

![Concurrency vs Parallelism](assets/Concurrency_Parallelism.png)

# Concurrent programming

Concurrent programming made it possible to solve the problem of multitasking when the first OS were created, even with only 1 CPU core.
During concurrent programming, there is a **context switching**, that is imperceptible to humans (looks like parallelism).

# Asynchronous programming

Solves the problem of blocking threads (not concurrency or parallelism).

Used for I/O operations that are beyond the scope of the application and require processing time in an external program:
  - network resources
  - disk write & read
  - memory
  - database
  
After calling an I/O operation, we can wait for the result:
- synchronously, blocking the resources until the result is returned
- asynchronously, which doesn't block the resources

Debugging doesn't stop asynchronous operations.

## Asynchronous operations benefits example

### Synchronous (blocking) web server example

Synchronous web application with 1 CPU core during the request execution starts a new thread, if it performs synchronous operation, it will block the application.
If application is used by more than 1 user, concurrent programming with context switching is used to handle such requests (1 CPU core).
Web server has a thread pool with limited number of threads (that handle requests). By default, it's `(CPU physical core number) x (number of threads that can be run on each core)`, so if the CPU has 6 cores and 2 threads on each, there will be 12 threads in the thread pool available to use. When the number of available threads is exceeded, a thread throttling mechanism kicks in.
The synchronous approach makes the thread in such an approach wait most of the time for the result and during this time it could perform other operations.

![alt text](assets/image-1.png)

```cs
app.MapGet("/sync", () =>
{
    Thread.Sleep(1000); // It waits most of the time, blocking the thread
    return "Hello";
});
```

Result is returned after 1s and thread handling that request is blocked.

Load test results:

```
API sync load test scenario (5s timeout)
    - requests: 188
    - ok:       76      (p50 = 3022.85 ms, p75 = 3039.23 ms, p95 = 3993.6 ms, p99 = 4583.42 ms)
    - fail:     112     (min > 5s)
```

### Asynchronous web server example

Asynchronous programming can be implemented on 1 thread, it doesn't require more than 1 core or 1 thread, but it's welcomed to use multiple threads.

In the asynchronous version as in the synchronous version - one thread is taken from the **ThreadPool** to handle the request, but instead of blocking the thread, while waiting for the result, it is returned to the thread pool, and it can be reused by another request. After receiving the result continuation doesn't have to take place on the same thread on which it was started, **ThreadPool** can allocate another thread. Storing context execution is needed to continue code execution properly.

![alt text](assets/image-2.png)


```cs
app.MapGet("/async", async () =>
{
    // Doesn't block the thread, it's returned to the ThreadPool while waiting for the result
    await Task.Delay(1000);
    return "Hello";
});
```

Result is still returned after 1s, but it doesn't block the thread, so thread can be reused in another request. This means that more requests can be handled this way. 

Load test results:

```
API async load test scenario (5s timeout)
    - requests: 109497
    - ok:       109430  (p50 = 1218.56 ms, p75 = 1296.38 ms, p95 = 1704.96 ms, p99 = 1797.12 ms)
    - fail:     67      (min > 5s)
```

Asynchronous operations occurs in parallel, but it subscribes to when that operation completes.  

## C# asynchronous patterns history

In C#, there are several options for performing parallel operations, some of them are legacy, but can be adapted to the latest approach with `TaskCompletionSource `.

### APM - Asynchronous Programming Model

Also called the `IAsyncResult` pattern, which is the **legacy** model that uses the `IAsyncResult` interface to provide asynchronous behavior.
In this pattern, asynchronous operations require `Begin` and `End` methods.

```cs
string filePath = "example.txt";
byte[] buffer = new byte[1024];
FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, true);
fs.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCallback), fs);
// ...

void ReadCallback(IAsyncResult ar)
{
    FileStream fs = (FileStream)ar.AsyncState;
    int bytesRead = fs.EndRead(ar);
    Console.WriteLine("Bytes read: " + bytesRead);
    fs.Close();
}
```

### EAP - Event-based Asynchronous Pattern

Event-based **legacy model** for providing asynchronous behavior.
We can subscribe to events that indicate when the operations are completed.
It requires one or more events, event handler delegate types, and `EventArg`-derived types.

```cs
var worker = new BackgroundWorker();

worker.DoWork += (sender, e) =>
{
    // Runs on a different thread
    Dispatcher.Invoke(() => Notes.Text += $"Worker DoWork\n");
};

worker.RunWorkerCompleted += (sender, e) =>
{
    // Triggered when work is done  
    Notes.Text += $"Worker completed";
};

worker.RunWorkerAsync();
```

### TAP - Task-based Asynchronous Programming

It's the **current** and recommended approach to asynchronous programming in .NET with the `async` and `await` keywords in C#.

#### `async`
Allows to use `await` keyword and spawns a **state machine**

#### `await`
- Pauses execution of the method until a result is available, without blocking the calling thread. 
- Waits for the operation to be completed, then continues execution
- Guarantees that the code after it won't be executed until the asynchronous operation is completed
- Retrieves result when available, unwrapping `Task<T>` result
- Makes sure that there were no exceptions with awaited task or re-throws exceptions that occured inside the `Task`, if task failed
- Introduces continuation that allows to get back to the original context (thread). Code after `await` will run once task has completed, and it will run on the same thread that spawned asynchronous operation
- Keyword `await` can be used on any *awaitable* type, which can be created by implementing `GetAwaiter` method

#### `Task`
- Is a representation of asynchronous operation that can return a result
- Object returned from an asynchronous method is a reference to operation/result/error
- Allows to:
  - execute work on a different thread
  - get the result from asynchronous operation
  - subscribe to when operation is done + continuation
  - handle exceptions

#### TaskCompletionSource

`TaskCompletionSource` is used to create a `Task` that can be manually controlled.
It allows you to complete the task explicitly by calling `SetResult`, `SetException`, or `SetCanceled`.
It is often used to bridge older asynchronous patterns (like APM or EAP) with the newer `async` & `await` pattern.

#### ASP.NET & Console app `SynchronizationContext`

In ASP.NET Core `SynchronizationContext` was removed so using `Task.ConfigureAwait(false)` doesn't work.
In console app there is no `SynchronizationContext` as well.
It works in other/older .NET applications, and should be used in libraries, because the library can be used by any type of application.

# Parallel Programming
- Used in the CPU bound scenarios to maximize performance.
- Split and solve small pieces independently, use as much computer resources as possible
- CPU cores can perform operations independently, so with their use we can program in parallel
- Allows for both asynchronous and parallel programming

# Race conditions

Occurs when multiple things performing work on the same shared resource. It can be solved with thread synchronization mechanisms.

--------------------------------------------------------------------------------------

# 4.2 - Knowing When All or Any Task Completes

```cs
foreach (string identifier in identifiers)
{
    // Each call wii be awaited one by one (await)
    var loadTask = await service.GetStockPricesFor(identifier, cancellationTokenSource.Token);
}
```

- Data can be loaded in parallel by performing multiple asynchronous operations at the same time
- `Task.WhenAll` accepts a task collection, it creates and returns a `Task`.
  Returned task status is completed only when all the tasks passed to the method are marked as completed.

  ```cs
  var loadingTasks = new List<Task<IEnumerable<int>>>();

  foreach (string id in ids)
  {
      var loadTask = service.GetAsync(id, cts.Token);
      loadingTasks.Add(loadTask);
  }
  
  var allResults = await Task.WhenAll(loadingTasks);
  ```

- With no `await` it isn't executed, it's a representation of loading all the stocks
  TODO: check " With no `await` it isn't executed"

  ```cs
  var allResults = Task.WhenAll(loadingTasks);
  ```

- `Task.WhenAny` returns `Task` after the completion of any first task.
  It can be used to create a timeout:

```cs
// ...
var timeoutTask = Task.Delay(2000);
var loadAllStocksAtOnceTask = Task.WhenAll(loadingTasks);

 // Timeout after 2s
var firstCompletedTask = await Task.WhenAny(loadAllStocksAtOnceTask, timeoutTask);

if (firstCompletedTask == timeoutTask)
    throw new OperationCanceledException("Loading timeout");

return loadAllStocksAtOnceTask.Result;
```

but it's easier to use `cancellationTokenSource.CancelAfter` method to achieve a timeout

- When `Task.WhenAll/WhenAny` are awaited it ensures that if any task failed within method, the exception will be propagated back to the calling context

TODO: `Task.WhenAll/WhenAny` exceptions

# 4.4 - Process Tasks as They Complete

- Standard .NET collections aren't thread-safe.
  Thread-safe collections should be used when working with collections on multiple threads
- Data can be processed on the fly as subsequent tasks are completed

```cs
var loadingTasks = new List<Task>();
var stocks = new ConcurrentBag<StockPrice>();

foreach (string id in ids)
{
    var loadTask = service.GetStockPricesFor(id, cts.Token)
        .ContinueWith(completedTask =>
        {
            foreach (var stock in completedTask.Result)
                stocks.Add(stock);
            
            UpdateStocksUi(stocks);
        }, 
        TaskContinuationOptions.OnlyOnRanToCompletion);

    loadingTasks.Add(loadTask);
}

await Task.WhenAll(loadingTasks);
```

# 5.5 - Deadlocking

A deadlock occurs if 2 threads depend on each other and one of them is blocked.
`Task` has a method `Wait` which will block the current thread until the data for the task is available.

```cs
private void Search_Click(...)
{
    var task = Task.Run(() =>
    {
        // Update UI (communicate with the original thread)
        Dispatcher.Invoke(() => { });
    });
    
    // Wait = block UI thread until all processing has completed,
    // but it can't completed, because it can't communicate back
    task.Wait();
}
```
UI thread waits for a thread to complete and this thread cannot complete unless it can communicate back to the UI thread.

Another deadlock with `Wait`:

```cs
private void Search_Click(...)
{
    LoadStocks().Wait(); // Deadlock, no await
}

private async Task LoadStocks()
{
    // Load data...
    Stocks.ItemsSource = data.SelectMany(x => x);
}
```

Deadlock with `Result`:

```cs
private void Search_Click(...)
{
    Stocks.ItemsSource = LoadStocks().Result; // Deadlock, no await
}

private async Task<IEnumerable<StockPrice>> LoadStocks()
{
    // Load data...
    return data.SelectMany(x => x);
}
```

The state machine with the code inside runs on the same thread (UI in this case) and it can't be executed, because this thread is blocked.
Asynchronous operation can't communicate to the state machine when it completes.

# 6.4 - Working with Attached and Detached Tasks

```cs
Task.Run(() => // parent task
{
    Task.Run(() => { }); // child task
    Task.Run(() => { }); // child task
});
```

- `Task.Run` doesn't have `TaskContinuationOptions`, it's a shortcut of using the `Task.Factory`

```cs
Console.WriteLine("Starting");

await Task.Factory.StartNew(() =>
    {
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("Completed 1");
        });
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(2000);
            Console.WriteLine("Completed 2");
        });
        
        // Parent tasks immediately starts child tasks and it's marked as completed
    });

Console.WriteLine("Completed");
```

Result:
```
Starting
Completed
Completed 1
Completed 2
```

Using `TaskCreationOptions.AttachedToParent`

```cs
Console.WriteLine("Starting");

await Task.Factory.StartNew(() =>
    {
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("Completed 1");
        }, TaskCreationOptions.AttachedToParent); // Attach to parent
        
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(2000);
            Console.WriteLine("Completed 2");
        });
    });

Console.WriteLine("Completed");
```

Result:
```
Starting
Completed 1
Completed
Completed 2
```

Using `TaskCreationOptions.DenyChildAttach`, which is default for `Task.Run`.
All child tasks would work as detached tasks.

```cs
Console.WriteLine("Starting");

await Task.Factory.StartNew(() =>
    {
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(1000);
            Console.WriteLine("Completed 1");
        }, TaskCreationOptions.AttachedToParent);
        
        Task.Factory.StartNew(() =>
        {
            Thread.Sleep(2000);
            Console.WriteLine("Completed 2");
        });
    }, TaskCreationOptions.DenyChildAttach);

Console.WriteLine("Completed");
Console.ReadKey();
```

Result looks like default `Task.Factory.StartNew` call with no options:
```
Starting
Completed
Completed 1
Completed 2
```

--------------------------------------------------------------------------------------

# Questions / TODO

1. What is the difference?
    
    ```cs
    Task.Run(() => GetStocks());
    Task.Run(async () => await GetStocks());

    async Task GetStocks() { /* ... */ }
    ```

1. When `Task.Run` is executed? What is the difference?

    ```cs
    var loadLinesTask = SearchForStocks();

    Task<List<string>> SearchForStocks()
    {
        return Task.Run(async () =>
        {
            // ...
            return lines;
        });
    }

    // ...

    await loadLinesTask;
    ```

    vs

    ```cs
    var loadLinesTask = await SearchForStocks();

    async Task<List<string>> SearchForStocks()
    {
        return await Task.Run(async () =>
        {
            // ...
            return lines;
        });
    }
    ```

    1. What is the point of that?
    
    ```cs
    async Task AsyncTaskMethod()
    {
        await TaskMethod();
    }

    Task TaskMethod() // no async
    {
        // ...
    }
    ```

1. Can I spawn too many threads and overflow a thread pool?

1. When is the method executed? What if an exception is thrown?

    ```cs
    void Search_Click(...)
    {
        try
        {
            var loadLinesTask = Task.Run(() => File.ReadAllLines("StockPrices_Small.csv"));
            var processStocksTask = loadLinesTask.ContinueWith(completedTask => { /* continuation */ });
        }
        catch (Exception ex)
        {
            Notes.Text = ex.Message;
        }
    }
    ```

1. Try different `ContinueWith` chain executions, `OnlyOnFaulted` etc.

1. Multiple awaits and exception.
   If `responseTask` throws an exception (on execution), then it will be re-thrown (await) and caught, but what about `getStocksTask`?

    ```cs
    async void Search_Click(...)
    {
        var getStocksTask = GetStocks();
        await getStocksTask;
        AfterLoadingStockData();
    }

    async Task GetStocks()
    {
        try
        {
            var store = new DataStore();
            Task responseTask = store.GetStockPrices();
            Stocks.ItemsSource = await responseTask;
        }
        catch (Exception ex)
        {
            Notes.Text = ex.Message;
        }
    }
    ```

1. Check `CancellationTokenSource.Dispose`

1. Standard .NET collections aren't thread-safe, try to break few and check what happens

1. What happens if `Task.WhenAny` returns the first completed task and the next task throws an exception or `Task.WhenAll`?

1. `Task.WhenAll` exceptions and `Parallel` [link](https://code-maze.com/csharp-execute-multiple-tasks-asynchronously/)

1. Explore the benefits of `ConfigureAwait(false)`

1. Is async method with no await started like Task.Run?

    ```cs
    using var cts = new CancellationTokenSource();

    // Is thread pool involved here? How does it work? Is it a different thread?
    var backgroundTask = StartBackgroundService(cts.Token);

    // ...

    cts.Cancel();

    // And what is the task status before and after this?
    // App can't close properly without this, why is that?
    await backgroundTask;

    async Task StartBackgroundService(CancellationToken ct)
    {
        try
        {
            while (!ct.IsCancellationRequested) { /* ... */ }
        }
        catch (TaskCanceledException) { } // Is this catch needed? (ct doesn't throw)
    }
    ```

1. Are all continuations running on the same new thread?

   ~~~~ ```cs
    await foreach (var stock in enumerator) {}
    ```

1. `Task.Yield`

1. `ValueTask`

1. Program, Process, Thread  
   ![Program, Process, Thread](assets/Program_Process_Thread.png)  
   [source](https://www.youtube.com/channel/UCZgt6AzoyjslHTC9dz0UoTw/community?lb=UgkxC7h3_WHiaeRFkHvbBzmlJudh-7q3W1Cj)

1. Own awaitable type with `GetAwaiter`

1. Background processing with channels
   [link](https://code-maze.com/aspnetcore-long-running-tasks-monolith-app/)