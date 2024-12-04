using Microsoft.AspNetCore.Components;
using Logic;
using Microsoft.JSInterop;
using Game.Services; // Assuming GameStateService is in the Game.Services namespace

namespace Game.Pages
{
    public partial class GameBoardAI : ComponentBase
    {
        /*
        [Parameter]
        public int gameId { get; set; }
        */

        [Inject]
        private IJSRuntime? JSRuntime { get; set; }

        [Inject]
        public GameStateService GameStateService { get; set; } 

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        [Inject]
        private GameRepository GameRepository { get; set; }

        private bool ShowResultModal { get; set; } = false;
        private string ResultTitle { get; set; } = string.Empty;
        private string ResultMessage { get; set; } = string.Empty;
        private bool IsRulesModalVisible { get; set; } = false;
        private Logic.Game? currentGame;
        private AIGameInfo game;

        private void NavigateToStartGame()
        {
            NavigationManager?.NavigateTo("/StartGame");
        }

        protected override async Task OnInitializedAsync()
        {
            currentGame = GameStateService.CurrentGame;
            game = new AIGameInfo(currentGame.GameMode);
            game.WinnerChanged += OnWinnerChanged;
            //currentGame.PlayerCharacter = game.NextPlayer == Players.X ? "X" : "O";
            await InvokeAsync(StateHasChanged);
        }

        private async void OnWinnerChanged(object? sender, GameResult e)
        {
            
            currentGame.Ended = DateTime.Now;
            currentGame.GameWinner = e == GameResult.XWins ? "X" : e == GameResult.OWins ? "O" : "-";
            if (currentGame.GameWinner == "-")
            {
                currentGame.GameScore = currentGame.GameMode == "easy" ? 5 : currentGame.GameMode == "medium" ? 10 : 20;
            }
            else if (currentGame.GameWinner == currentGame.PlayerCharacter)
            {
                currentGame.GameScore = currentGame.GameMode == "easy" ? 10 : currentGame.GameMode == "medium" ? 20 : 40;
            }
            else
            {
                currentGame.GameScore = 0;
            }
            
            

            if (e == GameResult.XWins || e == GameResult.OWins)
            {
                ShowResultModal = true;
                var winner = e == GameResult.XWins ? "X" : "O";
                ResultTitle = $"Player {winner} Wins!";
                ResultMessage = "Congratulations! Would you like to play again?";
            }
            else if (e == GameResult.Cat)
            {
                ShowResultModal = true;
                ResultTitle = "It's a Draw!";
                ResultMessage = "Good game! Would you like to play again?";
                await InvokeAsync(StateHasChanged);
            }
            await GameRepository.CreateGameAsync(currentGame);
        }

        private async void HandleCellClick(BoardIndex boardIndex, CellIndex cellIndex)
        {
            if (game.CanPlay(boardIndex, cellIndex))
            {
                game.Play(boardIndex, cellIndex);
                await InvokeAsync(StateHasChanged);
            }
            await game.PlayAIAsync(boardIndex);
            await InvokeAsync(StateHasChanged);
        }

        private void ResetGame()
        {
            /*
            game = new AIGameInfo(game.CurrentGameMode);
            game.WinnerChanged += OnWinnerChanged;
            ShowResultModal = false;
            InvokeAsync(StateHasChanged);
            */
            NavigationManager?.NavigateTo("/startgame");
        }

        private string GetCellContent(BoardIndex boardIndex, CellIndex cellIndex)
        {
            var board = game.GetBoard(boardIndex);
            var cellInfo = board.GetCell(cellIndex);
            var cellValue = cellInfo.Value;
            return cellValue == CellValue.Blank ? string.Empty : cellValue.ToString();
        }

        private string GetCellClasses(BoardIndex boardIndex, CellIndex cellIndex)
        {
            var board = game.GetBoard(boardIndex);
            var cellInfo = board.GetCell(cellIndex);
            var cellValue = cellInfo.Value;

            var classes = new List<string> { "cell" };

            if (cellValue != CellValue.Blank)
            {
                classes.Add(cellValue == CellValue.X ? "x" : "o");
            }

            return string.Join(" ", classes);
        }

        private string GetSubBoardClasses(BoardIndex boardIndex)
        {
            var classes = new List<string> { "sub-board" };

            var board = game.GetBoard(boardIndex);

            if (game.NextBoards.Contains(boardIndex))
            {
                classes.Add("active");
            }

            if (board.Winner == GameResult.XWins)
            {
                classes.Add("won-x");
            }
            else if (board.Winner == GameResult.OWins)
            {
                classes.Add("won-o");
            }
            else if (board.Winner == GameResult.Cat)
            {
                classes.Add("draw");
            }

            return string.Join(" ", classes);
        }

        private IEnumerable<BoardIndex> GetAllBoardIndices()
        {
            return Enum.GetValues(typeof(BoardIndex)).Cast<BoardIndex>();
        }

        private IEnumerable<CellIndex> GetAllCellIndices()
        {
            return Enum.GetValues(typeof(CellIndex)).Cast<CellIndex>();
        }

        private void ShowRulesModal()
        {
            IsRulesModalVisible = true;
        }

        private void CloseRulesModal()
        {
            IsRulesModalVisible = false;
        }
    }
}
