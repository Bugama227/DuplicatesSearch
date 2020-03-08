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
        public Dictionary<string, string> Matches = new Dictionary<string, string>();
        public Dictionary<string, string> TempOfRemoved = new Dictionary<string, string>();
        public int DimensionScale = 32;
        public int ReducedDimensionScale = 16;
        public int TempAmount = 3;
        public bool isChecked = false;
        public enum RemoveCases
        {
            Left = 0,
            Right = 1,
            Both = 10,
            FalsePositive = 11
        };
        private void ClearTemp()
        {
            MakeStuffButton.Enabled = true;
            ToggleButtons(false);

            DeleteTempFolder();
            FolderPath = "";
            isChecked = false;

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            listView1.Items.Clear();

            LightHashes.Clear();
            DarkHashes.Clear();
            Matches.Clear();

            FolderLabel.Text = "FolderLabel";
            Hash_label.Text = "Hash_label";
            LightLabel.Text = "LightLabel";
            DarkLabel.Text = "DarkLabel";
            ResultLabel.Text = "ResultLabel";
            DoubleLabel.Text = "DoubleLabel";
        }

        async private Task<string> CompareLightFingerPrints()
        {
            return await Task.Run(async () =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();

                int J = 1;
                for (int i = 0; i < LightHashes.Count - 1; i++)
                {
                    for (int j = J; j < LightHashes.Count; j++)
                    {
                        bool isSimilar = await FastCompareFunc(LightHashes.ElementAt(i).Value[0], LightHashes.ElementAt(j).Value[0]).ConfigureAwait(false);
                        if (isSimilar == true)
                        {
                            isSimilar = await SlowCompareFunc(LightHashes.ElementAt(i).Value[1], LightHashes.ElementAt(j).Value[1]).ConfigureAwait(false);
                        }

                        if (isSimilar == true)
                        {
                            try
                            {
                                Matches.Add(Path.GetFileName(LightHashes.ElementAt(i).Key), Path.GetFileName(LightHashes.ElementAt(j).Key));
                            }
                            catch { }
                        }
                    }
                    J++;
                }

                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                return elapsedTime;
            }).ConfigureAwait(false);
        }

        async private Task<string> CompareDarkFingerPrints()
        {
            return await Task.Run(async () =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                int J = 1;

                for (int i = 0; i < DarkHashes.Count - 1; i++)
                {
                    for (int j = J; j < DarkHashes.Count; j++)
                    {
                        bool isSimilar = await FastCompareFunc(DarkHashes.ElementAt(i).Value[0], DarkHashes.ElementAt(j).Value[0]);
                        if (isSimilar == true)
                        {
                            isSimilar = await SlowCompareFunc(DarkHashes.ElementAt(i).Value[1], DarkHashes.ElementAt(j).Value[1]);
                        }
                        if (isSimilar == true)
                        {
                            try
                            {
                                Matches.Add(Path.GetFileName(DarkHashes.ElementAt(i).Key), Path.GetFileName(DarkHashes.ElementAt(j).Key));
                            }
                            catch { }
                        }
                    }
                    J++;
                }
                TimeSpan ts = stopWatch.Elapsed;
                string elapsedTime = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10);
                return elapsedTime;
            }).ConfigureAwait(false);
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

        private Task<string> SetMatchesIntoLV()
        {
            return Task.Run(() =>
            {
                this.BeginInvoke((ThreadStart)delegate ()
                {
                    listView1.BeginUpdate();
                });

                Parallel.For(0, Matches.Count, i =>
                {
                    this.BeginInvoke((ThreadStart)delegate ()
                    {
                        listView1.Items.Add(new ListViewItem(new[] { Matches.ElementAt(i).Key, Matches.ElementAt(i).Value }));
                    });
                });

                this.BeginInvoke((ThreadStart)delegate ()
                {
                    listView1.EndUpdate();
                });
                    
                ToggleButtons(true);

                return listView1.Items.Count.ToString();
            });
        }

        

        private void CheckTemp()
        {
            if (!Directory.Exists($"{FolderPath}\\TempFolder"))
            {
                DirectoryInfo di = Directory.CreateDirectory($"{FolderPath}\\TempFolder");
                di.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            }

            if (TempOfRemoved.Count > TempAmount - 1)
            {
                File.Delete($"{FolderPath}\\TempFolder\\{TempOfRemoved.Keys.First()}");
                File.Delete($"{FolderPath}\\TempFolder\\{TempOfRemoved.Values.First()}");

                TempOfRemoved.Remove(TempOfRemoved.Keys.First());
            }
        }

        private void RemoveMatchFromLV(RemoveCases removeCase)
        {
            if (listView1.Items.Count == 0 || listView1.SelectedItems.Count == 0) return;

            CheckTemp();

            string leftMatch = listView1.SelectedItems[0].SubItems[0].Text;
            string rightMatch = listView1.SelectedItems[0].SubItems[1].Text;

            pictureBox1.Image = null;
            pictureBox2.Image = null;
            listView1.SelectedItems[0].Remove();

            try
            {
                switch (removeCase)
                {
                    case RemoveCases.Left:
                        File.Move($"{FolderPath}\\{leftMatch}", $"{FolderPath}\\TempFolder\\{leftMatch}");
                        break;

                    case RemoveCases.Right:
                        File.Move($"{FolderPath}\\{rightMatch}", $"{FolderPath}\\TempFolder\\{rightMatch}");
                        break;

                    case RemoveCases.Both:
                        File.Move($"{FolderPath}\\{leftMatch}", $"{FolderPath}\\TempFolder\\{leftMatch}");
                        File.Move($"{FolderPath}\\{rightMatch}", $"{FolderPath}\\TempFolder\\{rightMatch}");
                        break;

                    case RemoveCases.FalsePositive:
                        break;
                }
            }
            catch { }

            TempOfRemoved.Add(leftMatch, rightMatch);
        }

        private void ToggleButtons(bool state)
        {
            this.BeginInvoke((ThreadStart)delegate ()
            {
                RetrieveButton.Enabled = state;
                DeleteLeftButton.Enabled = state;
                DeleteRightButton.Enabled = state;
                DeleteBothButton.Enabled = state;
                FalsePositiveButton.Enabled = state;
            });
        }

        private void DeleteTempFolder()
        {
            if (FolderPath != null && Directory.Exists($"{FolderPath}\\TempFolder"))
                Directory.Delete($"{FolderPath}\\TempFolder");
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DeleteTempFolder();
        }

        async private void LoadButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ClearTemp();
                FolderPath = fbd.SelectedPath;
                this.Paths = await CompareHelpers.GetAllImagesPaths(fbd.SelectedPath);
            }
            fbd.Dispose();
        }

        async private void MakeStuffButton_Click(object sender, EventArgs e)
        {
            if (isChecked) return;

            MakeStuffButton.Enabled = false;
            isChecked = true;

            (DarkHashes, LightHashes) = await CompareHelpers.SetFingerPrintsIntoDictionary(Paths);
            this.Paths = null;

            LightLabel.Text = await CompareLightFingerPrints();
            DarkLabel.Text = await CompareDarkFingerPrints();

            this.BeginInvoke((ThreadStart)async delegate ()
            {
                ResultLabel.Text = await SetMatchesIntoLV();
            });
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (listView1.Items.Count == 0) return;

            string LeftMatch = listView1.SelectedItems[0].SubItems[0].Text;
            string RightMatch = listView1.SelectedItems[0].SubItems[1].Text;

            pictureBox1.Image = null;
            pictureBox2.Image = null;

            pictureBox1.ImageLocation = $"{FolderPath}\\{LeftMatch}";
            pictureBox2.ImageLocation = $"{FolderPath}\\{RightMatch}";
        }

        private void DeleteLeftButton_Click(object sender, EventArgs e)
        {
            RemoveMatchFromLV(RemoveCases.Left);
        }

        private void DeleteRightButton_Click(object sender, EventArgs e)
        {
            RemoveMatchFromLV(RemoveCases.Right);
        }

        private void DeleteBothButton_Click(object sender, EventArgs e)
        {
            RemoveMatchFromLV(RemoveCases.Both);
        }

        private void FalsePositiveButton_Click(object sender, EventArgs e)
        {
            RemoveMatchFromLV(RemoveCases.FalsePositive);
        }

        private void RetrieveButton_Click(object sender, EventArgs e)
        {
            if (TempOfRemoved.Count > 0)
            {
                string LeftMatch = TempOfRemoved.Keys.Last();
                string RightMatch = TempOfRemoved.Values.Last();

                listView1.Items.Add(new ListViewItem(new[] { LeftMatch, RightMatch }));
                if (File.Exists($"{FolderPath}\\TempFolder\\{LeftMatch}"))
                {
                    File.Move(
                        $"{FolderPath}\\TempFolder\\{LeftMatch}",
                        $"{FolderPath}\\{LeftMatch}"
                    );
                }
                if (File.Exists($"{FolderPath}\\TempFolder\\{RightMatch}"))
                {
                    File.Move(
                        $"{FolderPath}\\TempFolder\\{RightMatch}",
                        $"{FolderPath}\\{RightMatch}"
                    );
                }
                TempOfRemoved.Remove(TempOfRemoved.Keys.Last());
            }
        }
    }
}
