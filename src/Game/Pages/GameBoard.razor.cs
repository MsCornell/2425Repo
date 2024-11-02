using Logic;
using Microsoft.AspNetCore.Components;

namespace Game.Pages
{
    public partial class GameBoard : ComponentBase
    {
        private readonly GameInfo game;

        public GameBoard()
        {
            game = new();
        }

        protected override void OnInitialized()
        {
            // Optional initialization logic
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(100);
        }

        private bool IsBoardPlayable(BoardIndex boardIndex)
        {
            return game.NextBoards.Contains(boardIndex);
        }

        private string GetCellImage(BoardIndex boardIndex, CellIndex cellIndex)
        {
            // If the board has a winner, show the winning mark for all cells on this board
            var board = game.GetBoard(boardIndex);
            if (board.Winner != GameResult.InProgress)
            {
                return board.WinningMark;
            }

            // Otherwise, return the normal cell image
            return board.GetCell(cellIndex).ImageName;
        }

        private void Play(BoardIndex boardIndex, CellIndex cellIndex)
        {
            if (game.CanPlay(boardIndex, cellIndex))
            {
                game.Play(boardIndex, cellIndex);
                StateHasChanged();
            }
        }

        private string GetCellStyle(BoardIndex boardIndex, CellIndex cellIndex)
        {
            var board = game.GetBoard(boardIndex);
            var cell = board.GetCell(cellIndex);

            // Apply a different style if the cell is part of the winning combination
            return cell.IsWinningCell ? "winning-cell" : "normal-cell";
        }
    }
}
