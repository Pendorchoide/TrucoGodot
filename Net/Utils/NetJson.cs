using System.Text.Json;

namespace TrucoProject.Net.Utils
{
    public static class NetJson
    {
        public static string Stringify(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        public static T Parse<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
