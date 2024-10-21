using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Game.Pages.Admin
{
    public partial class Email : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

         private void NavigateToSignup()
        {
            Navigation.NavigateTo("/Signup");
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