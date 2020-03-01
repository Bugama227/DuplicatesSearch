using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScaleTo16x16
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public string[] Paths;
        public string FolderPath;
        public Dictionary<string, string[]> LightHashes = new Dictionary<string, string[]>();
        public Dictionary<string, string[]> DarkHashes = new Dictionary<string, string[]>();
        /*public List<KeyValuePair<string, string>> Matches = new List<KeyValuePair<string, string>>();*/
        public Dictionary<string, string> Matches = new Dictionary<string, string>();
        public int DimensionScale = 32;
        public int ReducedDimensionScale = 16;

        private void ClearTemp()
        {
            LightHashes.Clear();
            DarkHashes.Clear();
            progressBar1.Value = 0;
            listView1.Items.Clear();
            Matches.Clear();
        }

        async private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ClearTemp();
                FolderPath = fbd.SelectedPath;
                FolderLabel.Text = await GetAllImagesPaths(fbd.SelectedPath);
                progressBar1.Minimum = 0;
                progressBar1.Maximum = Paths.Length;
            }
            fbd.Dispose();
        }

        async private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            Hash_label.Text = await SetFingerPrintsIntoDictionary();
            await Task.Run(() =>
             {
                 CompareLightFingerPrints();
                 CompareDarkFingerPrints();
             });
            
            SetMatchesIntoLV();
        }

        async private Task<string> GetAllImagesPaths(string folderName)
        {
            return await Task.Run(() =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                this.BeginInvoke((ThreadStart)delegate ()
                {
                    label8.Text = "Getting Images Paths";
                });
                var IEpaths = Directory.GetFiles(folderName, "*.*").AsParallel(). // search of typical image formats using lynq
                 Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                 s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                 s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                 s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase) ||
                 s.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase));
                Paths = IEpaths.ToArray();
                
                
                
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;

                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
                return elapsedTime.ToString();
            }).ConfigureAwait(false);
        }

        private Task<string> SetFingerPrintsIntoDictionary()
        {
            return Task.Run(() =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                bool isDark;
                string LittleTempHash = "";
                string MiddleTempHash = "";
                this.BeginInvoke((ThreadStart)delegate ()
                {
                    label8.Text = "Setting Image Hash Into Dictionary";
                });
                Parallel.For(0, Paths.Length, i =>
                {
                    (LittleTempHash , MiddleTempHash, isDark) = SetBlackAndWhite(Paths[i]);
                    
                    if (isDark)
                    {
                        DarkHashes.Add(Paths[i], new[] { LittleTempHash , MiddleTempHash});
                    }
                    else
                    {
                        LightHashes.Add(Paths[i], new[] { LittleTempHash, MiddleTempHash });
                    }
                    UpdateProgressBar();
                });

                stopWatch.Stop();
                this.BeginInvoke((ThreadStart)delegate ()
                {
                    progressBar1.Value = 0;
                    progressBar1.Maximum = ((DarkHashes.Count - 1) * DarkHashes.Count) / 2 +
                                           ((LightHashes.Count - 1) * LightHashes.Count) / 2;
                });

                TimeSpan ts = stopWatch.Elapsed;

                string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
                return elapsedTime.ToString();
            });
        }

        private void UpdateProgressBar()
        {
            this.BeginInvoke((ThreadStart)delegate ()
            {
                progressBar1.Value += 1;
            });
        }

        private (string, string, bool) SetBlackAndWhite(string path) 
        {
            Bitmap Temp = new Bitmap(path);
            Bitmap MiddleSizedImage = new Bitmap(Temp, new Size(DimensionScale, DimensionScale));
            bool isDark = false;
            int overallAvg = GetAvgImageColor(MiddleSizedImage);
            if(overallAvg < 128)
            {
                isDark = true;
            }
            Bitmap SmallerImage = ReduceImageScale(MiddleSizedImage);
            Temp.Dispose();
            Color pixel;
            
            string MiddleTempHash = "";
            string LittleTempHash = "";

            this.BeginInvoke((ThreadStart)delegate ()
            {
                label8.Text = "Setting Image Hash";
            });

            for (int y = 0; y < MiddleSizedImage.Height; y++)
            {
                for (int x = 0; x < MiddleSizedImage.Width; x++)
                {
                    
                    
                    pixel = MiddleSizedImage.GetPixel(x, y);

                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;
                    int avg = (r + g + b) / 3;

                    if (avg > overallAvg)
                    {
                        MiddleTempHash += "1";
                    }
                    else
                    {
                        MiddleTempHash += "0";
                    }

                }
            }
            MiddleSizedImage.Dispose();

            this.BeginInvoke((ThreadStart)delegate ()
            {
                label8.Text = "Setting Image Hash";
            });

            for (int y = 0; y < SmallerImage.Height; y++)
            {
                for(int x = 0; x < SmallerImage.Width; x++)
                {
                    
                    
                    pixel = SmallerImage.GetPixel(x, y);
                    
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;
                    int avg = (r + g + b) / 3;

                    if (avg > overallAvg)
                    {
                        LittleTempHash += "1";
                    }
                    else
                    {
                        LittleTempHash += "0";
                    }
                    
                }
            }
            SmallerImage.Dispose();
            return (LittleTempHash, MiddleTempHash, isDark);
        }

        private int GetAvgImageColor(Bitmap image)
        {
            int avg = 0;
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
            avg = tempList.Aggregate((i, acc) => i + acc) / tempList.Count;
            return avg;
        }

        private Bitmap ReduceImageScale(Bitmap MiddleSizedImage)
        {
            Bitmap newImage = new Bitmap(MiddleSizedImage);
            this.BeginInvoke((ThreadStart)delegate ()
            {
                label8.Text = "Reducing Image Size";
            });
            while (newImage.Width > ReducedDimensionScale)
            {
                
                
                newImage = SuperReduced(newImage);
            }
            return newImage;
        }

        private Bitmap SuperReduced(Bitmap newImage)
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
        private int GetPixelsAvg(Color pixel1, Color pixel2)
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

        async private void CompareLightFingerPrints()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int J = 1;

            this.BeginInvoke((ThreadStart)delegate ()
            {
                label8.Text = "Comparing Light Fingers";
            });

            for (int i = 0; i < LightHashes.Count - 1; i++)
            {
                for(int j = J; j < LightHashes.Count; j++)
                {
                    
                    
                    bool isSimilar = await FastCompareFunc(LightHashes.ElementAt(i).Value[0], LightHashes.ElementAt(j).Value[0]).ConfigureAwait(false);
                    if(isSimilar == true)
                    {
                        isSimilar = await SlowCompareFunc(LightHashes.ElementAt(i).Value[1], LightHashes.ElementAt(j).Value[1]).ConfigureAwait(false);
                    }
                    UpdateProgressBar();
                    if (isSimilar == true)
                    {
                        /*Matches.Add(new KeyValuePair<string, string>(Path.GetFileName(Hashes.ElementAt(i).Key), Path.GetFileName(Hashes.ElementAt(j).Key)));*/
                        try
                        {
                            Matches.Add(Path.GetFileName(LightHashes.ElementAt(i).Key), Path.GetFileName(LightHashes.ElementAt(j).Key));
                        }
                        catch
                        {
                            
                        }
                    }
                }
                J++;
            }
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            this.BeginInvoke((ThreadStart)delegate ()
            {
                label3.Text = elapsedTime;
            });
        }

        async private void CompareDarkFingerPrints()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            int J = 1;

            this.BeginInvoke((ThreadStart)delegate ()
            {
                label8.Text = "Comparing Dark Fingers";
            });

            for (int i = 0; i < DarkHashes.Count - 1; i++)
            {
                for (int j = J; j < DarkHashes.Count; j++)
                {
                    
                    

                    bool isSimilar = await FastCompareFunc(DarkHashes.ElementAt(i).Value[0], DarkHashes.ElementAt(j).Value[0]);
                    if (isSimilar == true)
                    {
                        isSimilar = await SlowCompareFunc(DarkHashes.ElementAt(i).Value[1], DarkHashes.ElementAt(j).Value[1]);
                    }

                    UpdateProgressBar();

                    if (isSimilar == true)
                    {
                        /*Matches.Add(new KeyValuePair<string, string>(Path.GetFileName(Hashes.ElementAt(i).Key), Path.GetFileName(Hashes.ElementAt(j).Key)));*/
                        try
                        {
                            Matches.Add(Path.GetFileName(DarkHashes.ElementAt(i).Key), Path.GetFileName(DarkHashes.ElementAt(j).Key));
                        }
                        catch
                        {

                        }
                    }
                }
                J++;
            }
            TimeSpan ts = stopWatch.Elapsed;
            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds,
                ts.Milliseconds / 10);
            label7.Text = elapsedTime;
        }

        private Task<bool> FastCompareFunc(string firstHash, string secondHash)
        {
            return Task.Run(() =>
            {
                int DiffCount = 0;
                return Parallel.For(0, ReducedDimensionScale * ReducedDimensionScale, (i, pls) =>
                {
                    if (firstHash[i] != secondHash[i])
                    {
                        ++DiffCount;
                        if (DiffCount > 2)
                        {
                            pls.Break();
                        }
                    }
                }).IsCompleted;
            });
        }

        private Task<bool> SlowCompareFunc(string firstHash, string secondHash)
        {
            return Task.Run(() =>
            {
                int DiffCount = 0;
                return Parallel.For(0, DimensionScale * DimensionScale, (i, pls) =>
                {
                    if (firstHash[i] != secondHash[i])
                    {
                        ++DiffCount;
                        if (DiffCount > 5)
                        {
                            pls.Break();
                        }
                    }
                }).IsCompleted;
            });
        }

        private void SetMatchesIntoLV()
        {
            Parallel.For(0, Matches.Count, i => 
            {
                this.BeginInvoke((ThreadStart)delegate ()
                {
                    listView1.Items.Add(new ListViewItem(new[] { Matches.ElementAt(i).Key, Matches.ElementAt(i).Value }));
                    label4.Text = listView1.Items.Count.ToString();
                });
            });
            
            /*for (int i = 0; i < Matches.Count; i++)
            {
                listView1.Items.Add(new ListViewItem(new[] { Matches.ElementAt(i).Key, Matches.ElementAt(i).Value }));
            }*/
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if(listView1.Items.Count > 0)
            {
                string LeftMatch = listView1.SelectedItems[0].SubItems[0].Text;
                string RightMatch = listView1.SelectedItems[0].SubItems[1].Text;

                if(pictureBox1.Image != null || pictureBox2.Image != null)
                {
                    pictureBox1.Image = null;
                    pictureBox2.Image = null;
                }
                pictureBox1.Image = Image.FromFile(FolderPath + "\\" + LeftMatch);
                pictureBox2.Image = Image.FromFile(FolderPath + "\\" + RightMatch);
            }
            
        }




        /*private Bitmap ReduceImageScale(Bitmap image)
        {
            Bitmap newImage;
            int X = 0, Y = 0;
            int tempAvg = 0;
            Color pixel;

            for(int x = 0; x < image.Width; x += 2)
            {
                for(int y = 0; y < image.Height; y += 2)
                {
                    getPixelsAvg(image.GetPixel(x, y), image.GetPixel(x + 1, y));
                    pixel = image.GetPixel(x, y);
                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;
                    tempAvg += (r + g + b) / 3;
                    pixel = image.GetPixel(x + 1, y);
                    r = pixel.R;
                    g = pixel.G;
                    b = pixel.B;
                    tempAvg += (r + g + b) / 3;
                    tempAvg /= 2;
                }
            }
            return image;
        }

        private int getPixelsAvg(Color pixel1, Color pixel2)
        {
            int avg = 0;
            int r1 = pixel1.R;
            int g1 = pixel1.G;
            int b1 = pixel1.B;

            int r2 = pixel2.R;
            int g2 = pixel2.G;
            int b2 = pixel2.B;

            avg = ((r1 + g1 + b1) / 3 + (r2 + g2 + b2) / 3) / 2;
            return avg;
        }*/
    }
}
