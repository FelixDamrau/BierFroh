using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BierFroh.Modules.Logo;
public class Generator
{
    private readonly Random random = new();

    public string Create(float hue, float hueRotation, float shade)
    {
        var palette = CreatePalette(hue, hueRotation, shade);

        using var memoryStream = new MemoryStream();
        using var image = new Image<Rgba32>(128, 128);
        image.Mutate(x => x.BackgroundColor(Color.Black));

        var borderSize = 8;
        var squareSize = 22;
        for (var column = 0; column <= 3; ++column)
        {
            for (var row = 0; row <= 3; ++row)
            {
                var x = (column + 1) * borderSize + column * squareSize;
                var y = (row + 1) * borderSize + row * squareSize;
                var square = new Rectangle(x, y, squareSize, squareSize);
                DrawSquare(image, square, GetRandomPaletteColor(palette));
            }
        }

        image.SaveAsPng(memoryStream);
        return "data:image/png;base64, " + Convert.ToBase64String(memoryStream.ToArray());
    }

    public IReadOnlyList<HsvData> GetPaletteColors(float hue, float hueRotation, float shade)
    {
        var saturation = 1;
        var brightness = 1;
        var rotatedHue = Mod(hue + hueRotation, 360);
        return new[]
        {
            new HsvData(hue, saturation, brightness),
            new HsvData(hue, saturation, brightness - 0.4f*shade),
            new HsvData(hue, saturation, brightness - shade),
            new HsvData(rotatedHue, saturation, brightness),
            new HsvData(rotatedHue, saturation, brightness - 0.4f*shade),
            new HsvData(rotatedHue, saturation, brightness - shade),
        };
    }

    private static void DrawSquare(Image<Rgba32> image, Rectangle square, Color fillColor) => image.Mutate(i => i.Fill(fillColor, square));

    private Color GetRandomPaletteColor(IReadOnlyList<Color> palette)
    {
        var rnd = random.Next(0, palette.Count);
        return palette[rnd];
    }

    private IReadOnlyList<Color> CreatePalette(float hue, float hueRotation, float shade)
    {
        var colors = GetPaletteColors(hue, hueRotation, shade);
        return colors
            .Select(c => new Hsv(c.Hue, c.Saturation, c.Brightness))
            .Select(c => GetRgb32(c))
            .ToList();

        static Color GetRgb32(Hsv hsv)
        {
            var rgb = ColorSpaceConverter.ToRgb(hsv);
            var rgba32 = new Rgba32(rgb.R, rgb.G, rgb.B);
            return new Color(rgba32);
        }
    }

    private static float Mod(float x, float y) => (x %= y) < 0 ? x + y : x;
}
