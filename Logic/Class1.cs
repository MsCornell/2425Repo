namespace Logic
{
    public class Class1
    {
        public static string GetMessage1()
        {
            return "Hello from Logic!";
        }

        public async Task<string> GetMessage2Async(HttpClient httpClient)
        {
            return await httpClient.GetStringAsync("/api/Function1");
        }

        public async Task<string> GetMessage3Async(HttpClient httpClient)
        {
            return await httpClient.GetStringAsync("/api/Function1");
        }
    }
}
