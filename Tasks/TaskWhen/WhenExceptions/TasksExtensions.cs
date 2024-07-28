public class TasksExtensions
{
    public static async Task WhenAll(IEnumerable<Task> tasks)
    {
        var result = Task.WhenAll(tasks);

        try
        {
            await result;
        }
        catch
        {
            throw result.Exception!;
        }
    }
}