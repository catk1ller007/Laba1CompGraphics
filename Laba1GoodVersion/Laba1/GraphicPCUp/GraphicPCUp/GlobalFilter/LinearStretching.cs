﻿using System.ComponentModel;
using System.Drawing;
using Template;

namespace Lin
{
	class LinearStretching : MatrixFilter
	{
		protected Color globalCalculateNewPixelColor(Bitmap sourceImage, int x, int y, int colorMinR, int colorMaxR, int colorMinG, int colorMaxG, int colorMinB, int colorMaxB)
		{
			Color srcColor = sourceImage.GetPixel(x, y);

			int srcColorR = Clamp(255 * (srcColor.R - colorMinR) / (colorMaxR - colorMinR), 0, 255);
			int srcColorG = Clamp(255 * (srcColor.G - colorMinG) / (colorMaxG - colorMinG), 0, 255);
			int srcColorB = Clamp(255 * (srcColor.B - colorMinB) / (colorMaxB - colorMinB), 0, 255);

			return Color.FromArgb(srcColorR, srcColorG, srcColorB);
		}

		public override Bitmap ProccesImage(Bitmap sourceImage, BackgroundWorker worker)
		{
			Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

			int colorMaxR = 0;
			int colorMaxG = 0;
			int colorMaxB = 0;
			int colorMinR = 255;
			int colorMinG = 255;
			int colorMinB = 255;

			for (int i = 0; i < sourceImage.Width; i++)
			{
				worker.ReportProgress((int)((float)i / resultImage.Width * 50));
				if (worker.CancellationPending)
					return null;

				for (int j = 0; j < sourceImage.Height; j++)
				{
					if ((sourceImage.GetPixel(i, j).R) > colorMaxR)
						colorMaxR = sourceImage.GetPixel(i, j).R;
					if ((sourceImage.GetPixel(i, j).G) > colorMaxG)
						colorMaxG = sourceImage.GetPixel(i, j).G;
					if ((sourceImage.GetPixel(i, j).B) > colorMaxB)
						colorMaxB = sourceImage.GetPixel(i, j).B;

					if ((sourceImage.GetPixel(i, j).R) < colorMinR)
						colorMinR = sourceImage.GetPixel(i, j).R;
					if ((sourceImage.GetPixel(i, j).G) < colorMinG)
						colorMinG = sourceImage.GetPixel(i, j).G;
					if ((sourceImage.GetPixel(i, j).B) < colorMinB)
						colorMinB = sourceImage.GetPixel(i, j).B;
				}
			}

			for (int i = 0; i < sourceImage.Width; i++)
			{
				worker.ReportProgress((int)((float)i / resultImage.Width * 50) + 50);
				if (worker.CancellationPending)
					return null;

				for (int j = 0; j < sourceImage.Height; j++)
				{
					resultImage.SetPixel(i, j, globalCalculateNewPixelColor(sourceImage, i, j, colorMinR, colorMaxR, colorMinG, colorMaxG, colorMinB, colorMaxB));
				}
			}

			return resultImage;
		}
	}
    class PerfectReflectorFilter : Filtres
    {
        protected int maxR, maxG, maxB;

        public override Bitmap ProccesImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            GetMax(sourceImage, out maxR, out maxG, out maxB, worker, 50, 0);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((double)i / resultImage.Width * 50) + 50);
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(Clamp((int)((double)(sourceColor.R * 255 / maxR)), 0, 255),
                                  Clamp((int)((double)(sourceColor.G * 255 / maxG)), 0, 255),
                                  Clamp((int)((double)(sourceColor.B * 255 / maxB)), 0, 255));
        }
    }
    class GrayWorldFilter : Filtres
    {
        protected int Avg;
        protected int AvgR, AvgG, AvgB;

        public override Bitmap ProccesImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);

            GetAverageBrightness(sourceImage, out AvgR, out AvgG, out AvgB, worker, 50, 0);
            Avg = (AvgR + AvgG + AvgB) / 3;
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((double)i / resultImage.Width * 50) + 50);
                if (worker.CancellationPending)
                    return null;
                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            return resultImage;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            return Color.FromArgb(Clamp((int)((double)(sourceColor.R * Avg / AvgR)), 0, 255),
                                  Clamp((int)((double)(sourceColor.G * Avg / AvgG)), 0, 255),
                                  Clamp((int)((double)(sourceColor.B * Avg / AvgB)), 0, 255));
        }
    }
}
