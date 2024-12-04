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

        [Inject]
        private Logic.PlayerRepository playerRepository { get; set; } = default!;

        private string userName = string.Empty;
        private string userEmail = string.Empty;
        private string userPassword = string.Empty;
        private string confirmPassword = string.Empty;

         private void NavigateToSignup()
        {
            Navigation.NavigateTo("/Signup");
        }

        private async Task NavigateToLogin()
        {
            if (string.IsNullOrEmpty(userPassword) || string.IsNullOrEmpty(confirmPassword))
            {
                await JSRuntime.InvokeVoidAsync("alert", "Please enter a password.");
                return;
            }
            else if (userPassword != confirmPassword)
            {
                await JSRuntime.InvokeVoidAsync("alert", "Passwords do not match.");
                return;
            }
            else if (string.IsNullOrEmpty(userName))
            {
                await JSRuntime.InvokeVoidAsync("alert", "Please enter a username.");
                return;
            }
            else
            {
                try
                {
                    // create new player dataset
                    var newPlayer = new Logic.Player
                    {
                       Name = userName,
                       Email = userEmail,
                       Created = DateTime.Now,
                      Password = userPassword
                    };

                     // load to database
                    //var playerRepository = new Logic.PlayerRepository("https://icy-sea-07449320f.5.azurestaticapps.net/data-api/api/Player");
                    await playerRepository.CreateAsync(newPlayer);
                    await JSRuntime.InvokeVoidAsync("alert", "Email and username saved successfully!");
                    }
                    catch(Exception ex)
                    {
                        await JSRuntime.InvokeVoidAsync("alert", "Something went wrong. Please try again.");
                        return;
                    }
                
            }
            Navigation.NavigateTo("/Login");
        }

        private void NavigateToTerms()
        {
            Navigation.NavigateTo("/Terms");
        }
    }
}