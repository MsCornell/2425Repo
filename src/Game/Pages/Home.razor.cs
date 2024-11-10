using Microsoft.AspNetCore.Components;

namespace Game.Pages
{
    public partial class Home : ComponentBase
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private void NavigateToStartGame()
        {
            NavigationManager?.NavigateTo("/StartGame");
        }
    }
}
