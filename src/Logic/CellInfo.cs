namespace Logic;

public class CellInfo
{
    public CellValue Value { get; set; } = CellValue.Blank;
    public string ImageName => Value.ToImagePath();
    public event EventHandler? ValueChanged;
}
