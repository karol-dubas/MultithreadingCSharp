using System.Runtime.CompilerServices;

// Start async state machine.
// After receiving a Task object, it looks as if the method is not executed from the beginning,
// but somewhere in the middle of the method (after `await`).
string value = await GetAsync();

Console.WriteLine(value);

// This is how a decompiled `async Task` method looks like.
// Keywords `async` and `await` are just a syntax sugar, they don't exist in low level C#/IL compiled code.
// `async Task` method automatically returns a `Task`, without explicit `return`. Compiler does it for us.
// Every async method will look like this, only different method builders are used.
Task<string> GetAsync()
{
    var stateMachine = new StateMachine
    {
        State = -1, // initial state
        MethodBuilder = AsyncTaskMethodBuilder<string>.Create()
    };
    
    stateMachine.MethodBuilder.Start(ref stateMachine); // calls StateMachine.MoveNext
    return stateMachine.MethodBuilder.Task; // Task.Status = WaitingForActivation
}

// struct only in Release mode, in Debug it's a sealed class
internal struct StateMachine : IAsyncStateMachine
{
    public int State;
    public AsyncTaskMethodBuilder<string> MethodBuilder;
    
    private TaskAwaiter _taskAwaiter;
    
    void IAsyncStateMachine.MoveNext()
    {
        try
        {
            if (State == -1) // not started yet
            {
                Console.WriteLine("Starting async operation...");

                // await = GetAwaiter, which returns TaskAwaiter, and awaits a result of this asynchronous operation.
                // When it returns a value, generic TaskAwaiter<T> is used instead.
                _taskAwaiter = Task.Delay(3_000).GetAwaiter();

                if (_taskAwaiter.IsCompleted) // lucky check
                {
                    Console.WriteLine("Async operation completed immediately");
                    State = 0;
                }
                else
                {
                    State = 0; // next time it enters MoveNext method, it will instantly take a result

                    // Schedule state machine to execute when async operation is completed.
                    // It saves the machine's state (stack -> heap) and returns control to the caller.
                    // Task Scheduler and OS are involved to resume code execution when a result is available.
                    MethodBuilder.AwaitUnsafeOnCompleted(ref _taskAwaiter, ref this);
                    Console.WriteLine("State machine state moved to heap");
                    return; // state saved, leave and wait for a result
                }
            }

            if (State == 0) // task completion
            {
                // TaskAwaiter.GetResult is a blocking operation, like Task.Result, when async operation isn't completed.
                // It returns void here, but it can return a generic type.
                _taskAwaiter.GetResult();

                Console.WriteLine("Async operation completed");
                MethodBuilder.SetResult("async result");
                State = -2; // finished
            }
        }
        catch (Exception e)
        {
            MethodBuilder.SetException(e);
            State = -2; // finished
        }
    }

    void IAsyncStateMachine.SetStateMachine(IAsyncStateMachine stateMachine)
    {
        MethodBuilder.SetStateMachine(stateMachine); // associate builder with state machine
    }
}