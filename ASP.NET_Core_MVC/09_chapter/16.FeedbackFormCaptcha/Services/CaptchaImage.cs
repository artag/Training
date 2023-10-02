using SkiaSharp;

namespace Feedback.Services;

public class CaptchaImage : ICaptchaImage
{
    private readonly string _text;
    private readonly int _width;
    private readonly int _height;
    private SKBitmap _image;

    /// <summary>
    /// Инициализирует экземпляр класса <see cref="CaptchaImage"/>.
    /// </summary>
    /// <param name="text">Текст капчи.</param>
    /// <param name="width">Ширина картинки.</param>
    /// <param name="height">Высота картинки.</param>
    public CaptchaImage(string text, int width, int height)
    {
        _text = text;
        _width = width;
        _height = height;

        GenerateImage();
    }

    public Stream Encode()
    {
        return _image.Encode(
                format: SKEncodedImageFormat.Jpeg,
                quality: 100)
            .AsStream();
    }

    public void Dispose()
    {
        _image?.Dispose();
    }

    private void GenerateImage()
    {
        _image = new SKBitmap(_width, _height);

        using var canvas = new SKCanvas(_image);
        canvas.Clear(SKColors.White);
        using var paint = new SKPaint();
        paint.TextSize = 32;
        paint.IsAntialias = true;
        paint.Typeface = SKTypeface.FromFamilyName(
            familyName: "Arial",
            weight: SKFontStyleWeight.Bold,
            width: SKFontStyleWidth.Normal,
            slant: SKFontStyleSlant.Upright);
        paint.Color = SKColors.Red;
        canvas.DrawText(
                text: _text,
                x: _image.Width / 4,
                y: 3 * _image.Height / 4,
                paint);
    }
}
