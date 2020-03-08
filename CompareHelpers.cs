using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleTo16x16
{
    class Constants
    {
        public const int DIMENSION_SCALE = 32;
        public const int REDUCED_IMAGE_SCALE = 16;
    }

    class CompareHelpers
    {
        async public static Task<string[]> GetAllImagesPaths(string folderName)
        {
            return await Task.Run(() =>
            {
                return Directory
                    .GetFiles(folderName, "*.*")
                    .AsParallel()
                    .Where(s =>
                        s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                        s.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase)
                    )
                    .ToArray();
            }).ConfigureAwait(false);
        }

        public static Task<(Dictionary<string, string[]>, Dictionary<string, string[]>)> SetFingerPrintsIntoDictionary(string[] paths)
        {
            return Task.Run(() =>
            {
                bool isDark;
                string LittleTempHash = "";
                string MiddleTempHash = "";

                var darkHashes = new Dictionary<string, string[]>();
                var lightHashes = new Dictionary<string, string[]>();
                Parallel.For(0, paths.Length, i =>
                {
                    (LittleTempHash, MiddleTempHash, isDark) = SetBlackAndWhite(paths[i]);

                    if (isDark)
                    {
                        darkHashes.Add(paths[i], new[] { LittleTempHash, MiddleTempHash });
                    }
                    else
                    {
                        lightHashes.Add(paths[i], new[] { LittleTempHash, MiddleTempHash });
                    }
                });

                return (darkHashes, lightHashes);
            });
        }

        private static (string, string, bool) SetBlackAndWhite(string path)
        {
            Bitmap Temp = new Bitmap(path);
            Bitmap MiddleSizedImage = new Bitmap(Temp, new Size(Constants.DIMENSION_SCALE, Constants.DIMENSION_SCALE));
            Bitmap SmallerImage = ReduceImageScale(MiddleSizedImage);

            Temp.Dispose();

            bool isDark = false;
            int overallAvg = GetAvgImageColor(MiddleSizedImage);

            if (overallAvg < 128)
            {
                isDark = true;
            }

            string MiddleTempHash = CreateHash(MiddleSizedImage, overallAvg);
            MiddleSizedImage.Dispose();

            string LittleTempHash = CreateHash(SmallerImage, overallAvg);
            SmallerImage.Dispose();

            return (LittleTempHash, MiddleTempHash, isDark);
        }

        private static string CreateHash(Bitmap image, int overallAvg)
        {
            string Hash = "";
            Color pixel;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    pixel = image.GetPixel(x, y);

                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;
                    int avg = (r + g + b) / 3;

                    if (avg > overallAvg)
                    {
                        Hash += "1";
                    }
                    else
                    {
                        Hash += "0";
                    }

                }
            }
            return Hash;
        }

        private static Bitmap ReduceImageScale(Bitmap MiddleSizedImage)
        {
            Bitmap newImage = new Bitmap(MiddleSizedImage);

            while (newImage.Width > Constants.REDUCED_IMAGE_SCALE)
                newImage = SuperReduced(newImage);

            return newImage;
        }

        private static Bitmap SuperReduced(Bitmap newImage)
        {
            Bitmap tempImage = new Bitmap(newImage, new Size(newImage.Width / 2, newImage.Height / 2));
            int X, Y = 0, tempAvg;
            for (int y = 0; y < newImage.Height; y += 2)
            {
                X = 0;
                for (int x = 0; x < newImage.Width; x += 2)
                {
                    tempAvg = GetPixelsAvg(newImage.GetPixel(x, y), newImage.GetPixel(x + 1, y));
                    tempImage.SetPixel(X++, Y, Color.FromArgb(255, tempAvg, tempAvg, tempAvg));
                }
                Y++;
            }
            return tempImage;
        }

        private static int GetPixelsAvg(Color pixel1, Color pixel2)
        {
            int avg;
            int r1 = pixel1.R;
            int g1 = pixel1.G;
            int b1 = pixel1.B;

            int r2 = pixel2.R;
            int g2 = pixel2.G;
            int b2 = pixel2.B;

            avg = ((r1 + g1 + b1) / 3 + (r2 + g2 + b2) / 3) / 2;
            return avg;
        }

        private static int GetAvgImageColor(Bitmap image)
        {
            List<int> tempList = new List<int>();

            Color pixel;
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    pixel = image.GetPixel(x, y);

                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    tempList.Add((r + g + b) / 3);
                }
            }

            return tempList.Aggregate((i, acc) => i + acc) / tempList.Count;
        }
    }
}
