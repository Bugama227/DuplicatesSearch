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
        public string[] Paths = { };
        public Dictionary<string, string> Hashes = new Dictionary<string, string>();
        public int DimensionScale = 32;
        private void button2_Click(object sender, EventArgs e)
        {
            /*OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                List<string> Temp = Paths.ToList();
                Temp.Add(ofd.FileName);
                Paths = Temp.ToArray();
                Hash_label.Text = Paths[i];
                Hashes.Add(Paths[i], "");
                Bitmap OrigImage = new Bitmap(ofd.FileName);
                Orig_pb.Image = OrigImage;
                Orig_pb.BackColor = Color.Black;
            }*/

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                Hashes.Clear();
                Paths = GetAllImagesPaths(fbd.SelectedPath);
            }
        }

        private string[] GetAllImagesPaths(string folderName)
        {
            var IEpaths = Directory.GetFiles(folderName, "*.*"). // search of typical image formats using lynq
                Where(s => s.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".gif", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".png", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".tiff", StringComparison.OrdinalIgnoreCase) ||
                s.EndsWith(".bmp", StringComparison.OrdinalIgnoreCase));
            string[] paths = IEpaths.ToArray();
            return paths;
        }

        private void SetFingerPrintsIntoDictionary()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            string TempHash = "";
            /*for (int i = 0; i < Paths.Length; i++)
            {
                TempHash = SetBlackAndWhite(Paths[i]);
                Hashes.Add(Paths[i], TempHash);
            }*/

            Parallel.For(0, Paths.Length, i =>
            {
                TempHash = SetBlackAndWhite(Paths[i]);
                Hashes.Add(Paths[i], TempHash);
            });

            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:00}",
            ts.Hours, ts.Minutes, ts.Seconds,
            ts.Milliseconds / 10);
            Hash_label.Text = elapsedTime;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SetFingerPrintsIntoDictionary();
            CompareFingerPrints();
        }

        private string SetBlackAndWhite(string path) 
        {
            Bitmap Temp = new Bitmap(path);
            Bitmap BlackAndWhiteImage = new Bitmap(Temp, new Size(DimensionScale, DimensionScale));
            Temp.Dispose();
            Color pixel;
            
            string TempHash = "";
            
            for (int y = 0; y < BlackAndWhiteImage.Width; y++)
            {
                for(int x = 0; x < BlackAndWhiteImage.Height; x++)
                {
                    pixel = BlackAndWhiteImage.GetPixel(x, y);

                    int a = pixel.A;

                    int r = pixel.R;
                    int g = pixel.G;
                    int b = pixel.B;

                    int avg = (r + g + b) / 3;
                    if (avg > 170)
                    {
                        TempHash += "1";
                    }
                    else
                    {
                        TempHash += "0";
                    }
                }
            }
            
            return TempHash;
        }

        private void CompareFingerPrints()
        {
            /*int i = 0, j = 1;
            while(i < Hashes.Count - 1)
            {
                while(j < Hashes.Count)
                {
                    j++
                }
                i++;
            }*/
            int J = 1;
            for(int i = 0; i < Hashes.Count - 1; i++)
            {
                for(int j = J; j < Hashes.Count; j++)
                {
                    bool isSimilar = AnotherCompareFunc(Hashes.ElementAt(i).Value, Hashes.ElementAt(j).Value);
                    
                    if(isSimilar == true)
                    {
                        string[] TemplviItem = { Path.GetFileName(Hashes.ElementAt(i).Key), Path.GetFileName(Hashes.ElementAt(j).Key) };
                        ListViewItem lvi = new ListViewItem(TemplviItem);
                        listView1.Items.Add(lvi);
                    }
                }
                J++;
            }
        }

        private bool AnotherCompareFunc(string firstHash, string secondHash)
        {
            int DiffCount = 0;
            for (int i = 0; i < DimensionScale * DimensionScale; i++)
            {
                if(DiffCount > 50)
                {
                    return false;
                }
                if(firstHash[i] != secondHash[i])
                {
                    DiffCount++;
                }
            }
            return true;
        }
    }
}
