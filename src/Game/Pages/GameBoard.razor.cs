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
            // todo
        }

        protected override async Task OnInitializedAsync()
        {
            await Task.Delay(100);
        }

        private bool IsBoardPlayable(BoardIndex boardIndex)
        {
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
    }
}
