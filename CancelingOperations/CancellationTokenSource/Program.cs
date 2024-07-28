// CancellationTokenSource provides cancellation request mechanism using CancellationToken.
var cts = new CancellationTokenSource();

// CancellationTokenSource signals to a CancellationToken that it should be canceled with Cancel / CancelAfter methods.
// Calling CancellationTokenSource.Cancel won't automatically cancel asynchronous operations,
// CancellationToken and its members are used for that purpose.
// CancellationTokenSource is created at the source of an operation/s that may need to be canceled.
cts.Cancel();
cts.CancelAfter(TimeSpan.FromSeconds(10));

// CancellationToken is obtained as a property from CancellationTokenSource.
// It indicates to a task that it's canceled, represents cancellation itself,
// which can be checked or received to handle cancellation request.
// CancellationToken can't trigger cancellation, it can be only observed,
// and it's passed to various methods that support cancellation, to observe and react to cancellation request
CancellationToken ct = cts.Token;

// Passing CancellationToken to a Task.Run method won't stop execution "inside",
// it just won't start Task.Run if CancellationToken is marked as canceled, to skip a scheduling process.
await Task.Run(() => Console.WriteLine("Not invoked"), ct)
    .ContinueWith(t => Console.WriteLine("Invoked"));
