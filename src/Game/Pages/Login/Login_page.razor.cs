using Microsoft.AspNetCore.Components;

namespace Game.Pages.Login
{
    public partial class login_page : ComponentBase
    {
        private string username = "";
        private string password = "";
        private bool rememberMe = false;
        private bool isPasswordVisible = false;

        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        private void TogglePasswordVisibility()
        {
            isPasswordVisible = !isPasswordVisible;
        }
        private string passwordInputType => isPasswordVisible ? "text" : "password";

        private void NavigateToHandleLogin()
        {
        //todo
            Navigation.NavigateTo("/Login");
        }

    }
}

