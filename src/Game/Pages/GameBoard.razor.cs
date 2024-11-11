using Microsoft.AspNetCore.Components;
using Logic;
using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace Game.Pages
{
    public partial class GameBoard : ComponentBase
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        [Inject]
        private NavigationManager NavigationManager { get; set; }

        private GameInfo game = new GameInfo();
        private bool ShowResultModal { get; set; } = false;
        private string ResultTitle { get; set; } = string.Empty;
        private string ResultMessage { get; set; } = string.Empty;

        private void NavigateToStartGame()
        {
            NavigationManager.NavigateTo("/StartGame");
        }

        protected override void OnInitialized()
        {
            game.WinnerChanged += async (sender, e) => await OnWinnerChanged(sender, e);
        }

        private async Task OnWinnerChanged(object sender, GameResult e)
        {
            ShowResultModal = true;

            if (e == GameResult.XWins || e == GameResult.OWins)
            {
                var winner = e == GameResult.XWins ? "X" : "O";
                ResultTitle = $"Player {winner} Wins!";
                ResultMessage = "Congratulations! Would you like to play again?";

                if (JSRuntime != null)
                {
                    await JSRuntime.InvokeVoidAsync("triggerConfetti", winner);
                }
            }
            else if (e == GameResult.Cat)
            {
                ResultTitle = "It's a Draw!";
                ResultMessage = "Good game! Would you like to play again?";
            }

            await InvokeAsync(StateHasChanged);
        }

        private async Task HandleCellClick(BoardIndex boardIndex, CellIndex cellIndex)
        {
            if (game.CanPlay(boardIndex, cellIndex))
            {
                game.Play(boardIndex, cellIndex);
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task ResetGame()
        {
            game = new GameInfo();
            game.WinnerChanged += async (sender, e) => await OnWinnerChanged(sender, e);
            ShowResultModal = false;
            await InvokeAsync(StateHasChanged);
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

            // Mark the sub-board as active if it's still in progress
            if (board.Winner == GameResult.InProgress)
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
    }
}
