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

        private string userName = string.Empty;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
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
            else if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName))
            {
                await JSRuntime.InvokeVoidAsync("alert", "Please enter a valid name.");
                return;
            }
            else
            {
                try
                {
                    // create new player dataset
                    var newPlayer = new Logic.Player
                    {
                        Name = firstName + " " + lastName,
                       Username = userName,
                       Created = DateTime.Now,
                      Password = userPassword
                    };

                     // load to database
                    var playerRepository = new Logic.PlayerRepository("http://localhost:5000/api/Player");
                    await playerRepository.CreateAsync(newPlayer);
                    await JSRuntime.InvokeVoidAsync("alert", "Email and user information saved successfully!");
                    }
                    catch
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