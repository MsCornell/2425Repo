using Microsoft.AspNetCore.Components;
using Game.Services;

namespace Game.Pages
{
    public partial class StartGame : ComponentBase
    {
        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        //private Logic.GameRepository gameRepository = new Logic.GameRepository("http://localhost:5000/api/Game");

        [Inject]
        private Services.GameStateService GameStateService { get; set; }

        [Inject]
        public PlayerStateService PlayerStateService { get; set; } 

        private async Task NavigateToGameBoard()
        {
            
            var newGame = new Logic.Game
            {
                Id = 1,  // TODO:change
                AiCharacter = false,  
                GameMode = "Local", // bond to botton
                Started = DateTime.Now,
                Ended = DateTime.Now,
                PlayerId = PlayerStateService.CurrentPlayer.Id,  // TODO
                PlayerCharacter = "X",  // bond to botton
                GameWinner = "X",  // bond to botton
                GameScore = 0  // bond to botton
            };

            // using api to save data
            GameStateService.CurrentGame = newGame;
            
            //NavigationManager?.NavigateTo($"/GameBoard/{newGame.Id}");
            NavigationManager?.NavigateTo("/GameBoard");
        }

        private async Task NavigateToAIEasy()
        {
            
            var newGame = new Logic.Game
            {
                Id = 1,  // TODO:change
                AiCharacter = true,  
                GameMode = "easy", // bond to botton
                Started = DateTime.Now,
                Ended = DateTime.Now,
                PlayerId = PlayerStateService.CurrentPlayer.Id,  // TODO
                PlayerCharacter = "X",  // bond to botton
                GameWinner = "X",  // bond to botton
                GameScore = 0  // bond to botton
            };

            // using api to save data
            GameStateService.CurrentGame = newGame;
            
            //NavigationManager?.NavigateTo($"/GameBoard/{newGame.Id}");
            NavigationManager?.NavigateTo("/GameBoardAI");
        }

        private async Task NavigateToAIMed()
        {
            
            var newGame = new Logic.Game
            {
                Id = 1,  // TODO:change
                AiCharacter = true,  
                GameMode = "medium", // bond to botton
                Started = DateTime.Now,
                Ended = DateTime.Now,
                PlayerId = PlayerStateService.CurrentPlayer.Id,  // TODO
                PlayerCharacter = "X",  // bond to botton
                GameWinner = "X",  // bond to botton
                GameScore = 0  // bond to botton
            };

            // using api to save data
            GameStateService.CurrentGame = newGame;
            
            //NavigationManager?.NavigateTo($"/GameBoard/{newGame.Id}");
            NavigationManager?.NavigateTo("/GameBoardAI");
        }
        

        private async Task NavigateToAIHard()
        {
            
            var newGame = new Logic.Game
            {
                Id = 1,  // TODO:change
                AiCharacter = true,  
                GameMode = "hard", // bond to botton
                Started = DateTime.Now,
                Ended = DateTime.Now,
                PlayerId = PlayerStateService.CurrentPlayer.Id,  // TODO
                PlayerCharacter = "X",  // bond to botton
                GameWinner = "X",  // bond to botton
                GameScore = 0  // bond to botton
            };

            // using api to save data
            GameStateService.CurrentGame = newGame;
            
            //NavigationManager?.NavigateTo($"/GameBoard/{newGame.Id}");
            NavigationManager?.NavigateTo("/GameBoardAI");        
        }
            
        
        private bool isComingSoonVisible = false;

        private void ShowComingSoonMessage()
        {
            isComingSoonVisible = true;
        }
    }
}
