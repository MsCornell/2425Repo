using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Game.Services; // Assuming PlayerStateService is in the Game.Services namespace

namespace Game.Pages.Admin
{
    public partial class Login : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        [Inject]
        public PlayerStateService PlayerStateService { get; set; } 

        private LoginModel loginModel = new LoginModel();
        private bool showPassword = false;
        //private Logic.Player? currentPlayer;

        private void TogglePassword()
        {
            showPassword = !showPassword;
        }

        private async Task HandleLogin()
        {

            try
            {
                var playerRepository = new Logic.PlayerRepository("http://localhost:5000/api/Player");
                var player = await playerRepository.GetAsync(loginModel.Username, loginModel.Password);
                // check if player is null
                if (player == null)
                {
                    throw new Exception();
                }

                PlayerStateService.CurrentPlayer = player;
                Navigation.NavigateTo("/Home");
            }
            catch
            {
                JSRuntime.InvokeVoidAsync("alert", "You have entered an invalid username or password.");
            }
        }

         private void NavigateToLoginBox()
        {
            Navigation.NavigateTo("/LoginBox");
        }

        private void NavigateToReset()
        {
            Navigation.NavigateTo("/Reset");
        }

        private void NavigateToTerms()
        {
            Navigation.NavigateTo("/Terms");
        }

        /*
        private void NavigateToHome()
        {
            Navigation.NavigateTo("/Home");
        }
        */

        public class LoginModel
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
            public bool RememberMe { get; set; } = false;
        }
    }
}