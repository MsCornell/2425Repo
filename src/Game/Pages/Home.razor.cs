using Logic;

using Microsoft.AspNetCore.Components;

namespace Game.Pages
{
    public partial class Home : ComponentBase
    {
        private readonly GameInfo game;

        public Home()
        {
            game = new();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private bool IsBoardPlayable(BoardIndex boardIndex)
        {
            return game.NextBoards.Contains(boardIndex);
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
