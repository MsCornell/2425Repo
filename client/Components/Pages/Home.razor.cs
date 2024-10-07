using Microsoft.AspNetCore.Components;

namespace Client.Components.Pages
{
    public partial class Home : ComponentBase
    {
        public string text1 = string.Empty;
        public string text2 = string.Empty;
        public string text3 = string.Empty;

        [Inject]
        public required IHttpClientFactory HttpClientFactory { get; set; }

        protected override void OnInitialized()
        {
            text1 = Logic.Class1.GetMessage1();
        }

        protected override async Task OnInitializedAsync()
        {
            var logic = new Logic.Class1();

            var functionClient = HttpClientFactory.CreateClient("FunctionClient");
            text2 = await logic.GetFunctionSample(functionClient);

            var databaseClient = HttpClientFactory.CreateClient("DatabaseClient");
            text3 = await logic.GetDatabaseSample(databaseClient);
        }
    }
}