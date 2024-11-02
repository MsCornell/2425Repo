namespace Logic;

public static class UtilityExtensions
{
    // Constants for the image names
    private const string XImage = "/Images/x.png";
    private const string OImage = "/Images/o.png";
    private const string BlankImage = "/Images/-.png";
    private const string WinImage = "/Images/win.png"; // New winning mark image

    // Extension method to get the image path based on CellValue
    public static string ToImagePath(this CellValue value)
    {
        return value switch
        {
            CellValue.X => XImage,
            CellValue.O => OImage,
            CellValue.Blank => BlankImage,
            _ => BlankImage
        };
    }

    // New method to get winning mark image
    public static string GetWinImage()
    {
        return WinImage;
    }
}
