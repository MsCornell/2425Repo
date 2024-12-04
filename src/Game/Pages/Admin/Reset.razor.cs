using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Game.Pages.Admin
{
    public partial class Reset : ComponentBase
    {
        [Inject]
        private NavigationManager Navigation { get; set; } = default!;

        [Inject]
        private Logic.PlayerRepository playerRepository { get; set; } = default!;

        [Inject]
        private IJSRuntime JSRuntime { get; set; } = default!;

        private string? userEmail { get; set; }
        private string? userPassword { get; set; }
        private string? confirmedPassword { get; set; }

        private async Task NavigateToLogin()
        {
            // load to database
            //var playerRepository = new Logic.PlayerRepository("https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Player");
            if (string.IsNullOrEmpty(userEmail))
            {
                await JSRuntime.InvokeVoidAsync("alert", "Please fill in an email.");
                return;
            }
            
            var player = await playerRepository.GetAsync(userEmail);
            // check if player is null
            if (player == null)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Email does not exist.");
                return;
            }
        
            if (userPassword != confirmedPassword)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Passwords do not match.");
                return;
            }

            if (string.IsNullOrEmpty(userPassword))
            {
                await JSRuntime.InvokeVoidAsync("alert", "Please enter a password.");
                return;
            }

            if(userPassword.Length < 8)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Password must be at least 8 characters long.");
                return;
            }

            // update password
            player.Password = userPassword;
            try 
            {
                player = await playerRepository.UpdateAsync(player);
            }
            catch
            {
                await JSRuntime.InvokeVoidAsync("alert", "An error occurred. Please try again.");
                return;
            }

            Navigation.NavigateTo("/Login");
        }

        private async Task NavigateBackToLogin()
        {
            Navigation.NavigateTo("/Login");
        }
    }
}