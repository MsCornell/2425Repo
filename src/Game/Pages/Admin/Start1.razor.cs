using Microsoft.AspNetCore.Components;

namespace Game.Pages.Admin
{
    public partial class Start1 : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private void NavigateToLogin()
        {
            Navigation.NavigateTo("/Login");
        }

        private void NavigateToSignup()
        {
            Navigation.NavigateTo("/Signup");
        }

        private void NavigateToStart()
        {
            Navigation.NavigateTo("/");
        }
    }
}

