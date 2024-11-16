using Microsoft.AspNetCore.Components;

namespace Game.Pages.Shared
{
    public partial class NavMenu : ComponentBase
    {

        private bool isNavActive = false;

        private void ToggleNav()
        {
            isNavActive = !isNavActive;
        }

        private void NavigateToHome()
        {
            NavigationManager.NavigateTo("/Home");
        }

        private void NavigateToStartGame()
        {
            NavigationManager.NavigateTo("/StartGame");
        }

        private void NavigateToLeaderboard()
        {
            NavigationManager.NavigateTo("/Leaderboard");
        }

        private void NavigateToProfile()
        {
            NavigationManager.NavigateTo("/Profile");
        }

        // Method to determine if the current page is active
        private bool IsActive(string path)
        {
            // Get the current URI without the base URL
            var currentPath = NavigationManager.Uri.Replace(NavigationManager.BaseUri, "/");
            return currentPath.Equals(path, StringComparison.OrdinalIgnoreCase);
        }
    }
}
