using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Json;

class FunctionTest
{
    static async Task Main(string[] args)
    {
        using (var client = new HttpClient())
        {
            var url = "http://localhost:7071/api/minimax";

            Dictionary<string, string> boardState = new Dictionary<string, string>
            {
                { "board", "XOXOXOXOX" },
                { "next", "O" }
            };

            string boardjsonString = JsonSerializer.Serialize(boardState);


            //var json = await JsonFileReader.ReadAsync<Item>(@"request.json");// Replace with your JSON payload
            // var jsonString = JsonSerializer.Serialize(json);
            // var content = new StringContent(jsonString, Encoding.UTF8, "application/json");
            var content = JsonContent.Create(boardjsonString);
            var response = await client.PostAsync(url, content);


            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseString);
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
    }

    public static class JsonFileReader
    {
        public static async Task<T> ReadAsync<T>(string filePath)
        {
            using FileStream stream = File.OpenRead(filePath);
            return await JsonSerializer.DeserializeAsync<T>(stream);
        }
    }

    public class Item
    {
        public string board;
        public string next;
    }

}