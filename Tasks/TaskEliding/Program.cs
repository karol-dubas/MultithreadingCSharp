string websiteUrl = "https://blog.stephencleary.com/2016/12/eliding-async-await.html";

string content1 = await GetWithKeywordsAsync(websiteUrl);
string content2 = await GetElidingKeywordsAsync(websiteUrl);

async Task<string> GetWithKeywordsAsync(string url)
{
    using var client = new HttpClient();
    return await client.GetStringAsync(url);
}

Task<string> GetElidingKeywordsAsync(string url)
{
    // Returns a task, but a client is disposed
    using var client = new HttpClient();
    return client.GetStringAsync(url);
}