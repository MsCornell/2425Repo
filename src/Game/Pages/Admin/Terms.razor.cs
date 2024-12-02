using Microsoft.AspNetCore.Components;

namespace Game.Pages.Admin
{
    public partial class Terms : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private void NavigateToStart()
        {
            Navigation.NavigateTo("/");
        }
    }
}

