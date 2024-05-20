var repo = new Repository();

var fooTask = repo.GetFooAsync(); // No await, continue
int barResult = await repo.GetBarAsync(); // Return control to the caller
int fooResult = await fooTask; // Return control to the caller

return barResult + fooResult;

class Repository
{
    public async Task<int> GetFooAsync()
    {
        // Return control to the caller (non-blocking),
        // it will return here once async operation is finished
        await Task.Delay(1000);
        
        return 1;
    }
    
    public async Task<int> GetBarAsync()
    {
        await Task.Delay(500); // Return control to the caller
        return 2;
    }
}