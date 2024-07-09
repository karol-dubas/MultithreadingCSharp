using System.Windows;

namespace WpfSyncCtx;

public partial class MainWindow
{
    public MainWindow()
    {
        InitializeComponent();
        
        // SynchronizationContext was created to solve the problem of
        // synchronizing asynchronous code with the context in which it was called.
        // In GUI applications (like WPF), UI-related operations must be performed on the main thread of the application.
        // SynchronizationContext allows preserving this context after asynchronous operations are performed.

        // SynchronizationContext saves the current context (e.g., the main UI thread).
        // When an asynchronous operation finishes, SynchronizationContext uses the saved context
        // to continue executing the code on the appropriate thread.
        
        var sc = SynchronizationContext.Current;
        Console.WriteLine($"SynchronizationContext is: {sc!.GetType()}"); // exists
    }

    private async void SyncButton_OnClick(object sender, RoutedEventArgs e)
    {
        PrintCurrentThread(1); // Thread #1

        await Task.Run(() =>
        {
            PrintCurrentThread(2); // Thread #2
        });

        PrintCurrentThread(3); // Thread #1

        Console.WriteLine();
    }
    
    private async void NoSyncButton_OnClick(object sender, RoutedEventArgs e)
    {
        PrintCurrentThread(1); // Thread #1

        await Task.Run(() =>
        {
            PrintCurrentThread(2); // Thread #2
        }).ConfigureAwait(false);

        PrintCurrentThread(3); // Thread #2

        Console.WriteLine();
    }
    
    private void PrintCurrentThread(int i) 
        => Console.WriteLine($"[{i}] Thread: " + Environment.CurrentManagedThreadId);
}