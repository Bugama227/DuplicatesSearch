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

        private FoldingManager FolderManager;
        public Dictionary<string, string[]> LightHashes = new Dictionary<string, string[]>();
        public Dictionary<string, string[]> DarkHashes = new Dictionary<string, string[]>();
        public Dictionary<string, string> Matches = new Dictionary<string, string>();
        private bool isChecked = false;

        private void ClearTemp()
        {
            this.MakeStuffButton.Enabled = true;
            this.ToggleButtons(false);

            this.FolderManager.DeleteTempFolder();
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

            this.FolderManager = null;
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

            this.FolderManager.CheckTemp();

            string leftMatch = listView1.SelectedItems[0].SubItems[0].Text;
            string rightMatch = listView1.SelectedItems[0].SubItems[1].Text;

            this.pictureBox1.Image = null;
            this.pictureBox2.Image = null;
            this.listView1.SelectedItems[0].Remove();

            this.FolderManager.RemoveSelectedFiles(removeCase);
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
            this.FolderManager.DeleteTempFolder();
        }

        async private void LoadButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ClearTemp();
                this.FolderManager = new FoldingManager(fbd.SelectedPath);
            }

            fbd.Dispose();
        }

        async private void MakeStuffButton_Click(object sender, EventArgs e)
        {
            if (this.isChecked) return;

            this.MakeStuffButton.Enabled = false;
            this.isChecked = true;

            (this.DarkHashes, this.LightHashes) = await CompareHelpers.SetFingerPrintsIntoDictionary(this.FolderManager.GetPaths());

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

            string leftMatch = this.listView1.SelectedItems[0].SubItems[0].Text;
            string rightMatch = this.listView1.SelectedItems[0].SubItems[1].Text;

            this.pictureBox1.Image = null;
            this.pictureBox2.Image = null;

            var folderPath = this.FolderManager.GetFolderPath();

            this.pictureBox1.ImageLocation = $"{folderPath}\\{leftMatch}";
            this.pictureBox2.ImageLocation = $"{folderPath}\\{rightMatch}";
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
            var matches = this.FolderManager.Retrieve();
            if (matches == null) return;

            this.listView1.Items.Add(new ListViewItem(new[] { matches[0], matches[1] }));
        }
    }
}
