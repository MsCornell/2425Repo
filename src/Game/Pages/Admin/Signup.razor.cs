using Microsoft.AspNetCore.Components;

namespace Game.Pages.Admin
{
    public partial class Signup : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

         private void NavigateToLoginBox()
        {
            Navigation.NavigateTo("/LoginBox");
        }

        private void NavigateToEmail()
        {
            Navigation.NavigateTo("/Email");
        }

        private void NavigateToLogin()
        {
            Navigation.NavigateTo("/Login");
        }

        private void NavigateToTerms()
        {
            Navigation.NavigateTo("/Terms");
        }
    }
}