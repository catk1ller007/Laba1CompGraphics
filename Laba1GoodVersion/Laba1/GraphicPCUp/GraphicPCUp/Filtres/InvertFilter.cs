using System.Drawing;
using Template;

namespace Invert
{
    class InvertFilter : Filtres
    {
        protected override Color calculateNewPixelColor(Bitmap naitiImage, int x, int y)
        {
            Color naitiColor = naitiImage.GetPixel(x, y);

            Color resultColor = Color.FromArgb(255 - naitiColor.R, 255 - naitiColor.G, 255 - naitiColor.B);

            return resultColor;
        }
    }
}
