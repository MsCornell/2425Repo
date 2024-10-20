using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Game.Pages.Admin
{
    public partial class Signup : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

         private void NavigateToStart1()
        {
            Navigation.NavigateTo("/Start1");
        }

        private void NavigateToLogin()
        {
            Navigation.NavigateTo("/Login");
        }

        private void NavigateToTerms()
        {
            Navigation.NavigateTo("/Terms");
        }

        private void NavigateToMain()
        {
            Navigation.NavigateTo("/Main");
        }
    }
}