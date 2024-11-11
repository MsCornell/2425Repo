using Microsoft.AspNetCore.Components;

namespace Game.Pages
{
    public partial class StartGame : ComponentBase
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private void NavigateToGameBoard()
        {
            NavigationManager?.NavigateTo("/GameBoard");
        }

        private bool isComingSoonVisible = false;

        private void ShowComingSoonMessage()
        {
            isComingSoonVisible = true;
        }
    }
}
