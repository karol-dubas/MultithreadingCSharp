using System.Text;
using NBomber.Contracts;
using NBomber.CSharp;
using NBomber.Http.CSharp;
using Xunit.Abstractions;

namespace AsyncApi.LoadTests;

public class ApiTest(ITestOutputHelper testOutputHelper)
{
    private readonly ITestOutputHelper _testOutputHelper = testOutputHelper;

    private const string Url = "http://localhost:5000";
    
    [Fact]
    public void Test_Sync_Endpoint()
    {
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(5);

        var scenario = Scenario.Create("API sync test scenario", async context =>
        {
            try
            {
                var request = Http.CreateRequest("GET", $"{Url}/sync");
                return await Http.Send(httpClient, request);
            }
            catch (Exception e)
            {
                return Response.Fail();
            }
        })
        .WithWarmUpDuration(TimeSpan.FromSeconds(5))
        .WithLoadSimulations(LoadSimulation.NewKeepConstant(150, TimeSpan.FromSeconds(5)));

        NBomberRunner.RegisterScenarios(scenario).Run();
    }
    
    [Fact]
    public void Test_Async_Endpoint()
    {
        using var httpClient = new HttpClient();
        httpClient.Timeout = TimeSpan.FromSeconds(5);

        var scenario = Scenario.Create("API async test scenario", async context =>
            {
                try
                {
                    var request = Http.CreateRequest("GET", $"{Url}/async");
                    return await Http.Send(httpClient, request);
                }
                catch (Exception e)
                {
                    return Response.Fail();
                }
            })
            .WithWarmUpDuration(TimeSpan.FromSeconds(5))
            .WithLoadSimulations(LoadSimulation.NewKeepConstant(1_500, TimeSpan.FromSeconds(5)));

        NBomberRunner.RegisterScenarios(scenario).Run();
    }
}