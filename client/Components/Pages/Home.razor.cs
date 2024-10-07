using Microsoft.AspNetCore.Components;

namespace Client.Components.Pages
{
    public partial class Home : ComponentBase
    {
        public string text = string.Empty;

        public Logic.Class1 Information = new();

        protected override void OnInitialized()
        {
            text = Information.GetMessage();
        }
    }
}