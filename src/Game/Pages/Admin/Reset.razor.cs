using Microsoft.AspNetCore.Components;

namespace Game.Pages.Admin
{
    public partial class Reset : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private void NavigateToLogin()
        {
            Navigation.NavigateTo("/Login");
        }
    }
}