using Helpers;

int sum = 0;

// Consuming a stream:
var numbersEnumerator = GetNumbers(); // It can't be transformed to another collection (it can be infinite)
await foreach (int number in numbersEnumerator) // Asynchronously retrieve the data 
{
    // Just calling the IAsyncEnumerable method won't "start" it,
    // it has to be consumed with await foreach loop to start sending data.
    
    // The whole enumerator can't be awaited.
    // Each item in the enumeration is awaited instead.
    // The foreach body is a continuation of each enumeration.
    
    ThreadExtensions.PrintCurrentThread(2);
    Console.WriteLine($"Current sum = {sum += number}");
}

// Producing a stream:
// IAsyncEnumerable<T> is used like a Task, and allows for asynchronous retrieval of each item as it arrives by 
// exposing an enumerator that provides asynchronous iteration.
async IAsyncEnumerable<int> GetNumbers()
{
    for (int i = 0; i < 5; i++)
    {
        ThreadExtensions.PrintCurrentThread(1);
        await Task.Delay(100);
        
        // Method must yield return, it signals to the iterator using this enumerator
        // that it has an item to process.
        yield return Random.Shared.Next(10);
    }
}
