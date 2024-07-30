CancellationTokenSource cts = new();
var ct1 = cts.Token;
Console.WriteLine(ct1.CanBeCanceled); // Properly created, can be canceled

CancellationToken ct2 = default; // or new() or CancellationToken.None, all create empty token
Console.WriteLine(ct2.CanBeCanceled); // Can't be canceled, it's just a placeholder

// CancellationToken is a struct (value type), so when passed to a method, its value is copied.
// It is useful for CancellationToken.Register method, as it calls the callback only for that specific instance.
void Method1(CancellationToken ct = default) => ct.Register(() => Console.WriteLine("I was canceled"));
void Method2(CancellationToken ct = default) => ct.Register(() => Console.WriteLine("Me too"));

// Internally CancellationToken is bound to the CancellationTokenSource, which is a class (reference type).
// CancellationTokenSource is canceled, and CancellationToken just points to that cancellation.