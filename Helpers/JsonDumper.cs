using System.Text.Json;

namespace Helpers;

public static class JsonDumper
{
    public static void Dump(this object value) 
        => Console.WriteLine(JsonSerializer.Serialize(value));
}