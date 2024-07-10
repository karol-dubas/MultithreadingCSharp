// Each async method will generate state machine code,
// but there is no continuation in these methods,
// so to keep amount of generated code to a minimum, we can skip async & await. 
// If the caller has the opportunity to await Task, then it should do so at the top level.
string result = await Method1();
Console.WriteLine(result);

// But there are also implications, like using & dispose pitfall, or misleading exception stack trace.
string websiteUrl = "https://blog.stephencleary.com/2016/12/eliding-async-await.html";
string content1 = await Get(websiteUrl);
string content2 = await GetButWrong(websiteUrl);
return;

Task<string> Method1() => Method2();
Task<string> Method2() => Task.Run(() => "Hello");

async Task<string> Get(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}

Task<string> GetButWrong(string url)
{
    // Returns a task, but the client is disposed
    using var client = new HttpClient();
    return client.GetStringAsync(url);
}