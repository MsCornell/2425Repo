using Microsoft.AspNetCore.Components;

namespace Game.Pages.Admin
{
    public partial class Start : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private void NavigateToStart1()
        {
            Navigation.NavigateTo("/Start1");
        }
    }
}