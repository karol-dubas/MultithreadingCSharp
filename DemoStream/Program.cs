using Helpers;

int sum = 0;

// Consuming a stream:
var numbersEnumerator = /*await*/ GetNumbers(); // Can't be awaited, because it's an enumeration
await foreach (int number in numbersEnumerator) // Asynchronously retrieve the data 
{
    // It awaits each item in the enumeration.
    // Foreach body is a continuation of each enumeration.
    
    //ThreadExtensions.PrintCurrentThread(1);
    Console.WriteLine($"Current sum = {sum += number}");
}

// Producing a stream:
// IAsyncEnumerable<T> is used like a Task, and allows for asynchronous retrieval of each item as it arrives by 
// exposing an enumerator that provides asynchronous iteration.
async IAsyncEnumerable<int> GetNumbers()
{
    for (int i = 0; i < 10; i++)
    {
        //ThreadExtensions.PrintCurrentThread(2);
        await Task.Delay(100);
        
        // Method must yield return, it signals to the iterator using this enumerator
        // that it has an item to process.
        yield return Random.Shared.Next(10);
    }
}
