// If Task.WhenAny or Task.WhenAll return task/s other tasks will try to complete

Console.WriteLine("WhenAll:");
var tasksAll = SampleTaskData.GetTasks();
try
{
    await Task.WhenAll(tasksAll); // Returns a generic type or void when awaited, re-throws an exception
}
catch (Exception ex) // Catches only the first exception
{
    Console.WriteLine($"Exception caught. {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("Better WhenAll:");
var tasksBetterAll = SampleTaskData.GetTasks();
try
{
    await TasksExtensions.WhenAll(tasksBetterAll);
}
catch (Exception ex) // Aggregate exception
{
    Console.WriteLine($"Exception caught. {ex.Message}");
}

Console.WriteLine();
Console.WriteLine("WhenAny:");
var tasksAny = SampleTaskData.GetTasks();
try
{
    var firstCompletedTask = await Task.WhenAny(tasksAny); // Returns first the completed task but doesn't re-throw an exception
    await firstCompletedTask; // It must be handled separately
}
catch (Exception ex)
{
    Console.WriteLine($"Exception caught. {ex.Message}");
}
