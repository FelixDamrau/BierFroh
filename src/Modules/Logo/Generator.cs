using SixLabors.ImageSharp;
using SixLabors.ImageSharp.ColorSpaces;
using SixLabors.ImageSharp.ColorSpaces.Conversion;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace BierFroh.Modules.Logo;
public class Generator
{
    private readonly IReadOnlyList<Color> palette;
    private readonly float hue;
    private readonly Random random = new();

    public Generator(float hue)
    {
        this.hue = hue;
        palette =  CreatePalette();
    }

    public string Create()
    {
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
                AddSquare(image, squareSize, x, y);
            }
        }

        image.SaveAsPng(memoryStream);
        return "data:image/png;base64, " + Convert.ToBase64String(memoryStream.ToArray());
    }

    private void AddSquare(Image<Rgba32> image, int size, int x, int y)
    {
        var square = new Rectangle(x, y, size, size);
        image.Mutate(i => i.Fill(GetRandomTintedColor(), square));
    }

    private Color GetRandomTintedColor()
    {
        var rnd = random.Next(0, palette.Count);
        return palette[rnd];
    }

    private IReadOnlyList<Color> CreatePalette()
    {
        var saturation = 1;
        var brightness = 1;
        var converter = new ColorSpaceConverter();
        return new[]
        {
            GetRgb32(new Hsv(hue, saturation, brightness)),
            GetRgb32(new Hsv(hue, saturation, brightness - 0.25f)),
            GetRgb32(new Hsv(hue, saturation, brightness - 0.55f)),
            GetRgb32(new Hsv(hue - 25, saturation, brightness)),
            GetRgb32(new Hsv(hue - 25, saturation, brightness - 0.25f)),
            GetRgb32(new Hsv(hue - 25, saturation, brightness - 0.55f)),
        };

        Color GetRgb32(Hsv hsv)
        {
            var rgb = converter.ToRgb(hsv);
            var rgba32 = new Rgba32(rgb.R, rgb.G, rgb.B);
            return new Color(rgba32);
        }
    }
}
