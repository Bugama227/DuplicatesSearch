using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScaleTo16x16
{
    

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
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    Hash += GetPixelAvg(image.GetPixel(x, y)) > overallAvg
                        ? "1"
                        : "0";
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

        private static int GetPixelsAvg(Color p1, Color p2)
        {
            return (GetPixelAvg(p1) + GetPixelAvg(p2)) / 2;
        }

        private static int GetPixelAvg(Color p)
        {
            return (p.R + p.G + p.B) / 3;
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

                    tempList.Add((pixel.R + pixel.G + pixel.B) / 3);
                }
            }

            return tempList.Aggregate((i, acc) => i + acc) / tempList.Count;
        }

        /// <summary>
        ///     Dirty func
        /// </summary>
        async public static Task<Dictionary<string, string>> CompareFingerPrints(
            Dictionary<string, string[]> Hash,
            Dictionary<string, string> Matches
        )
        {
            return await Task.Run(async () =>
            {
                int J = 1;
                for (int i = 0; i < Hash.Count - 1; i++)
                {
                    for (int j = J; j < Hash.Count; j++)
                    {
                        bool isSimilar = await FastCompareFunc(Hash.ElementAt(i).Value[0], Hash.ElementAt(j).Value[0])
                            ? await SlowCompareFunc(Hash.ElementAt(i).Value[1], Hash.ElementAt(j).Value[1])
                            : false;

                        if (isSimilar)
                        {
                            try
                            {
                                Matches.Add(Path.GetFileName(Hash.ElementAt(i).Key), Path.GetFileName(Hash.ElementAt(j).Key));
                            }
                            catch { }
                        }
                    }
                    J++;
                }

                return Matches;
            });
        }

        private static Task<bool> FastCompareFunc(string firstHash, string secondHash)
        {
            return Task.Run(() =>
            {
                int DiffCount = 0;
                return Parallel.For(0, Constants.REDUCED_IMAGE_SCALE * Constants.REDUCED_IMAGE_SCALE, (i, pls) =>
                {
                    if (firstHash[i] != secondHash[i])
                    {
                        ++DiffCount;
                        if (DiffCount > 2) pls.Break();
                    }
                }).IsCompleted;
            });
        }

        private static Task<bool> SlowCompareFunc(string firstHash, string secondHash)
        {
            return Task.Run(() =>
            {
                int DiffCount = 0;
                return Parallel.For(0, Constants.DIMENSION_SCALE * Constants.DIMENSION_SCALE, (i, pls) =>
                {
                    if (firstHash[i] != secondHash[i])
                    {
                        ++DiffCount;
                        if (DiffCount > 5) pls.Break();
                    }
                }).IsCompleted;
            });
        }
    }
}