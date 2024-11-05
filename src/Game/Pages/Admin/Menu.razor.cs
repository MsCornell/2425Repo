using Microsoft.AspNetCore.Components;

namespace Game.Pages
{
    public partial class Menu : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private void NavigateToGameBoard()
        {
            Navigation.NavigateTo("/GameBoard");
        }
    }
}