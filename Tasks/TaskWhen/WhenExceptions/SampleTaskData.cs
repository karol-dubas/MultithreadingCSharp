public class SampleTaskData
{
    public static IEnumerable<Task> GetTasks()
    {
        return new[]
        {
            OkTask("1st", 500, 1000),
            FaultyTask("2nd", 0, 1), // Lowest delay, so should complete first
            FaultyTask("3rd", 500, 1000),
            OkTask("4th", 500, 1000),
        };

        async Task FaultyTask(string s, int minDelayMs, int maxDelayMs)
        {
            await Task.Delay(Random.Shared.Next(minDelayMs, maxDelayMs));
            throw new Exception($"{s} failed");
        }

        async Task OkTask(string s, int minDelayMs, int maxDelayMs)
        {
            await Task.Delay(Random.Shared.Next(minDelayMs, maxDelayMs));
            Console.WriteLine($"{s} succeeded");
        }
    }
}