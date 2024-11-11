using Microsoft.AspNetCore.Components;
using Logic;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Game.Pages
{
    public partial class GameBoard : ComponentBase
    {
        [Inject]
        private IJSRuntime? JSRuntime { get; set; }

        [Inject]
        private NavigationManager? NavigationManager { get; set; }

        private GameInfo game = new GameInfo();
        private bool ShowResultModal { get; set; } = false;
        private string ResultTitle { get; set; } = string.Empty;
        private string ResultMessage { get; set; } = string.Empty;

        private void NavigateToStartGame()
        {
            NavigationManager?.NavigateTo("/StartGame");
        }

        protected override void OnInitialized()
        {
            game.WinnerChanged += async (_, e) => await OnWinnerChanged(this, e);
        }

        private async Task OnWinnerChanged(object sender, GameResult e)
        {
            try
            {
                ShowResultModal = true;

                if (e == GameResult.XWins || e == GameResult.OWins)
                {
                    var winner = e == GameResult.XWins ? "X" : "O";
                    ResultTitle = $"Player {winner} Wins!";
                    ResultMessage = "Congratulations! Would you like to play again?";

                    if (JSRuntime != null)
                    {
                        try
                        {
                            await JSRuntime.InvokeVoidAsync("triggerConfetti", winner);
                        }
                        catch (JSException jsEx)
                        {
                            Console.WriteLine($"JavaScript function 'triggerConfetti' was not found: {jsEx.Message}");
                        }
                    }
                }
                else if (e == GameResult.Cat)
                {
                    ResultTitle = "It's a Draw!";
                    ResultMessage = "Good game! Would you like to play again?";
                }

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in OnWinnerChanged: {ex.Message}");
            }
        }

        private async Task HandleCellClick(BoardIndex boardIndex, CellIndex cellIndex)
        {
            try
            {
                if (game.CanPlay(boardIndex, cellIndex))
                {
                    game.Play(boardIndex, cellIndex);
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in HandleCellClick: {ex.Message}");
            }
        }

        private async Task ResetGame()
        {
            try
            {
                game = new GameInfo();
                game.WinnerChanged += async (_, e) => await OnWinnerChanged(this, e);
                ShowResultModal = false;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in ResetGame: {ex.Message}");
            }
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

            switch (board.Winner)
            {
                case GameResult.XWins:
                    classes.Add("won-x");
                    break;
                case GameResult.OWins:
                    classes.Add("won-o");
                    break;
                case GameResult.Cat:
                    classes.Add("draw");
                    break;
                case GameResult.InProgress:
                    classes.Add("active");
                    break;
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
