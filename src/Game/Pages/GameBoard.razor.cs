using Logic;

using Microsoft.AspNetCore.Components;

using Microsoft.JSInterop;

using System.Threading.Tasks;

namespace Game.Pages
{
    public partial class GameBoard : ComponentBase
    {
        private readonly GameInfo game;

        [Inject]
        private PlayerRepository PlayerRepository { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        public GameBoard()
        {
            game = new();
        }

        protected override void OnInitialized()
        {
            // todo
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(100);
        }

        private bool IsBoardPlayable(BoardIndex boardIndex)
        {
            if (HasOverallWinner())
            {
                return false;
            }
            return game.NextBoards.Contains(boardIndex); //logic
        }

        private string GetCellImage(BoardIndex boardIndex, CellIndex cellIndex)
        {
            return game.GetBoard(boardIndex).GetCell(cellIndex).ImageName;
        }

        private void Play(BoardIndex boardIndex, CellIndex cellIndex)
        {
            if (game.CanPlay(boardIndex, cellIndex))
            {
                game.Play(boardIndex, cellIndex);
                StateHasChanged();
            }
        }

        private async Task ConfirmAndGoToHomePage()
        {
            bool confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to return to the home page?");
            if (confirmed)
            {
                Navigation.NavigateTo("/");
            }
        }

        private bool IsWinningLine(BoardIndex boardIndex)
        {
            // check the winning line
            return game.GetBoardWinner(boardIndex) != GameResult.InProgress;
        }

        private string GetCellStyle(BoardIndex boardIndex, CellIndex cellIndex)
        {
            var cell = game.GetBoard(boardIndex).GetCell(cellIndex);
            if (cell.Value == CellValue.X)
            {
                return "x-cell";
            }
            else if (cell.Value == CellValue.O)
            {
                return "o-cell";
            }
            return string.Empty;
        }

        private string GetBoardStyle(BoardIndex boardIndex)
        {
            var winnerPlayer = game.GetBoardWinnerPlayer(boardIndex);
            if (winnerPlayer == Players.X)
            {
                return "winning-line-x";
            }
            else if (winnerPlayer == Players.O)
            {
                return "winning-line-o";
            }
            return "no-winner";
        }

        private string GetOverallWinner()
        {
            var winner = game.Winner;
            if (winner == GameResult.XWins)
            {
                return "X is the winner!";
            }
            else if (winner == GameResult.OWins)
            {
                return "O is the winner!";
            }
            else if (winner == GameResult.Cat)
            {
                return "It's a draw!";
            }
            return string.Empty;
        }

        private bool HasOverallWinner()
        {
            return game.Winner != GameResult.InProgress;
        }
    }
}
