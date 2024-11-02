public class CellInfo
{
    public CellValue Value { get; set; } = CellValue.Blank;
    public string ImageName => Value.ToImagePath();
    public event EventHandler? ValueChanged;

    public bool IsWinningCell { get; set; } = false;
}
