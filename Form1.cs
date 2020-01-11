using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if(ofd.ShowDialog() == DialogResult.OK)
            {
                Bitmap OrigImage = new Bitmap(ofd.FileName);
                Orig_pb.Image = OrigImage;
                Orig_pb.BackColor = Color.Black;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(Orig_pb.Image != null)
            {
                Bitmap DownScaledImage = new Bitmap(Orig_pb.Image, new Size(16, 16));
                DownScaled_pb.Image = DownScaledImage;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if(DownScaled_pb.Image != null)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                if(sfd.ShowDialog() == DialogResult.OK)
                {
                    Bitmap saveds = new Bitmap(DownScaled_pb.Image);
                    saveds.Save(sfd.FileName, ImageFormat.Jpeg);
                }
            }
        }
    }
}
