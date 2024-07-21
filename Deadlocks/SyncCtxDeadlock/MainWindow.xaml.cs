using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SyncCtxDeadlock;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void Example1_Click(object sender, RoutedEventArgs e)
    {
        var task = Task.Run(() =>
        {
            Dispatcher.Invoke(() => { }); // Update UI, call the original thread, which is blocked and waits
        });

        // Block UI thread until all processing has completed,
        // but it can't complete, because Dispatcher can't communicate back.
        task.Wait();
    }
    
    private void Example2_Click(object sender, RoutedEventArgs e)
    {
        _ = GetNumbers().Result;
    }

    private async Task<IEnumerable<int>> GetNumbers()
    {
        // It tries to return to UI thread, due to the SynchronizationContext.
        // With ConfigureAwait(false) it won't return to the original (UI) thread. 
        await Task.Delay(500)/*.ConfigureAwait(false)*/;
        
        return Enumerable.Range(0, 10);
    }
}