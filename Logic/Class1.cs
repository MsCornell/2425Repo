namespace Logic
{
    public class Class1
    {
        public static string GetMessage1()
        {
            return "Hello from Logic!";
        }

        public async Task<string> GetFunctionSample(HttpClient httpClient)
        {
            return "";// await httpClient.GetStringAsync("/Function1");
        }

        public async Task<string> GetDatabaseSample(HttpClient httpClient)
        {
            return "";//await httpClient.GetStringAsync("/Function1");
        }
    }
}
