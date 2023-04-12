using System.Drawing;
using Template;


namespace EvYya
{
    class YvelichenitYarkosti : Filtres
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int k = 50;
            //int resR = sourceColor.R + k;
            //int resG = sourceColor.B + k;
            //int resB = sourceColor.G + k;
            Color resultColor = Color.FromArgb(Clamp(sourceColor.R + k, 0, 255),
                                               Clamp(sourceColor.G + k, 0, 255),
                                               Clamp(sourceColor.B + k, 0, 255));
            return resultColor;
        }
    }
}
