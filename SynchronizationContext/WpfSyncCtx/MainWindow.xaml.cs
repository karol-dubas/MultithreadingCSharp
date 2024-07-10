using System.Windows;
using Helpers;

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
        ThreadExtensions.PrintCurrentThread(1); // Thread #1

        await Task.Run(() =>
        {
            ThreadExtensions.PrintCurrentThread(2); // Thread #2
        });

        ThreadExtensions.PrintCurrentThread(3); // Thread #1

        Console.WriteLine();
    }
    
    private async void NoSyncButton_OnClick(object sender, RoutedEventArgs e)
    {
        ThreadExtensions.PrintCurrentThread(1); // Thread #1

        // ConfigureAwait configures how the continuation should execute.
        // ConfigureAwait(false) configures to continue on the new thread,
        // It skips SynchronizationContext, and it's quicker than waiting for another thread to be available.
        await Task.Run(() =>
        {
            ThreadExtensions.PrintCurrentThread(2); // Thread #2
        }).ConfigureAwait(false); // No continuation enqueue on a ThreadPool
        
        // TODO:
        // Each method marked with `async` has its own asynchronous context,
        // therefore it only affects the continuation in the method that you are operating in.

        ThreadExtensions.PrintCurrentThread(3); // Thread #2 (continue on new thread)

        Console.WriteLine();
    }
    

}