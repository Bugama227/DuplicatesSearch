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
    

    enum RemoveCase
    {
        Left = 0,
        Right = 1,
        Both = 10,
        FalsePositive = 11
    };

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
        private bool isChecked = false;
        

        private void ClearTemp()
        {
            this.MakeStuffButton.Enabled = true;
            this.ToggleButtons(false);

            FoldingHelpers.DeleteTempFolder(FolderPath);
            this.FolderPath = "";
            this.isChecked = false;

            this.pictureBox1.Image = null;
            this.pictureBox2.Image = null;
            this.listView1.Items.Clear();

            this.LightHashes.Clear();
            this.DarkHashes.Clear();
            this.Matches.Clear();

            this.FolderLabel.Text = "FolderLabel";
            this.Hash_label.Text = "Hash_label";
            this.LightLabel.Text = "LightLabel";
            this.DarkLabel.Text = "DarkLabel";
            this.ResultLabel.Text = "ResultLabel";
            this.DoubleLabel.Text = "DoubleLabel";
        }

        private Task<string> SetMatchesIntoLV()
        {
            return Task.Run(() =>
            {
                this.BeginInvoke((ThreadStart)delegate ()
                {
                    this.listView1.BeginUpdate();
                });

                Parallel.For(0, Matches.Count, i =>
                {
                    this.BeginInvoke((ThreadStart)delegate ()
                    {
                        this.listView1.Items.Add(new ListViewItem(new[] { Matches.ElementAt(i).Key, Matches.ElementAt(i).Value }));
                    });
                });

                this.BeginInvoke((ThreadStart)delegate ()
                {
                    this.listView1.EndUpdate();
                });

                ToggleButtons(true);

                return this.listView1.Items.Count.ToString();
            });
        }

        private void RemoveMatchFromLV(RemoveCase removeCase)
        {
            if (this.listView1.Items.Count == 0 || this.listView1.SelectedItems.Count == 0) return;

            FoldingHelpers.CheckTemp(this.FolderPath, this.TempOfRemoved);

            string leftMatch = listView1.SelectedItems[0].SubItems[0].Text;
            string rightMatch = listView1.SelectedItems[0].SubItems[1].Text;

            this.pictureBox1.Image = null;
            this.pictureBox2.Image = null;
            this.listView1.SelectedItems[0].Remove();

            FoldingHelpers.RemoveSelectedFiles(removeCase, this.FolderPath, leftMatch, rightMatch);

            this.TempOfRemoved.Add(leftMatch, rightMatch);
        }

        private void ToggleButtons(bool state)
        {
            this.BeginInvoke((ThreadStart)delegate ()
            {
                this.RetrieveButton.Enabled = state;
                this.DeleteLeftButton.Enabled = state;
                this.DeleteRightButton.Enabled = state;
                this.DeleteBothButton.Enabled = state;
                this.FalsePositiveButton.Enabled = state;
            });
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            FoldingHelpers.DeleteTempFolder(this.FolderPath);
        }

        async private void LoadButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ClearTemp();
                this.FolderPath = fbd.SelectedPath;
                this.Paths = await CompareHelpers.GetAllImagesPaths(fbd.SelectedPath);
            }
            fbd.Dispose();
        }

        async private void MakeStuffButton_Click(object sender, EventArgs e)
        {
            if (this.isChecked) return;

            this.MakeStuffButton.Enabled = false;
            this.isChecked = true;

            (this.DarkHashes, this.LightHashes) = await CompareHelpers.SetFingerPrintsIntoDictionary(Paths);
            this.Paths = null;

            await CompareHelpers.CompareFingerPrints(this.LightHashes, this.Matches);
            await CompareHelpers.CompareFingerPrints(this.DarkHashes, this.Matches);

            this.BeginInvoke((ThreadStart)async delegate ()
            {
                this.ResultLabel.Text = await this.SetMatchesIntoLV();
            });
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (this.listView1.Items.Count == 0) return;

            string LeftMatch = this.listView1.SelectedItems[0].SubItems[0].Text;
            string RightMatch = this.listView1.SelectedItems[0].SubItems[1].Text;

            this.pictureBox1.Image = null;
            this.pictureBox2.Image = null;

            this.pictureBox1.ImageLocation = $"{this.FolderPath}\\{LeftMatch}";
            this.pictureBox2.ImageLocation = $"{this.FolderPath}\\{RightMatch}";
        }

        private void DeleteLeftButton_Click(object sender, EventArgs e)
        {
            this.RemoveMatchFromLV(RemoveCase.Left);
        }

        private void DeleteRightButton_Click(object sender, EventArgs e)
        {
            this.RemoveMatchFromLV(RemoveCase.Right);
        }

        private void DeleteBothButton_Click(object sender, EventArgs e)
        {
            this.RemoveMatchFromLV(RemoveCase.Both);
        }

        private void FalsePositiveButton_Click(object sender, EventArgs e)
        {
            this.RemoveMatchFromLV(RemoveCase.FalsePositive);
        }

        private void RetrieveButton_Click(object sender, EventArgs e)
        {
            if (this.TempOfRemoved.Count > 0)
            {
                string leftMatch = this.TempOfRemoved.Keys.Last();
                string rightMatch = this.TempOfRemoved.Values.Last();

                this.listView1.Items.Add(new ListViewItem(new[] { leftMatch, rightMatch }));

                FoldingHelpers.MoveIfExists($"{this.FolderPath}\\TempFolder\\{leftMatch}", $"{this.FolderPath}\\{leftMatch}");
                FoldingHelpers.MoveIfExists($"{this.FolderPath}\\TempFolder\\{rightMatch}", $"{this.FolderPath}\\{rightMatch}");

                this.TempOfRemoved.Remove(this.TempOfRemoved.Keys.Last());
            }
        }
    }

    class Constants
    {
        public const int DIMENSION_SCALE = 32;
        public const int REDUCED_IMAGE_SCALE = 16;
        public const int TEMP_AMOUNT = 3;

    }
}
