namespace BierFroh.Modules.Logo;
public static class ColorConverter
{
    public static RgbData ToRgb(HsvData hsv)
    {
        var h = hsv.Hue / 60;
        var c = hsv.Brightness * hsv.Saturation;
        var m = hsv.Brightness - c;
        var x = c * (1 - Math.Abs(h % 2 - 1));

        var (r,g,b) = h switch
        {
            >= 0 and < 1 => (c, x, m),
            >= 1 and < 2 => (x, c, m),
            >= 2 and < 3 => (m, c, x),
            >= 3 and < 4 => (m, x, c),
            >= 4 and < 5 => (x, m, c),
            >= 5 and <= 6 => (c, m, x),
            _ => (0,0,0),
        };

        var red = Convert.ToInt32((r + m) * 255);
        var green = Convert.ToInt32((g + m) * 255);
        var blue = Convert.ToInt32((b + m) * 255);

        return new RgbData(red, green, blue);
    }

    public static string ToCss(RgbData rgb) => $"rgba({rgb.Red}, {rgb.Green}, {rgb.Blue}, 1)";
}
