using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace RGBImageViewer
{
    public partial class Form1 : Form
    {
        RGBImage image;
        public Form1()
        {
            InitializeComponent();
            image = new RGBImage();
        }

        private void 열기ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(ofDlg.ShowDialog() == DialogResult.OK)
            {
                image.OpenImage(ofDlg.FileName);
                pictureBox.Image = image.GetImage(1920);
            }
        }
    }

    public class RGBImage
    {
        public string FileName { get; set; }
        public byte[] RedChannel { get; private set; }
        public byte[] GreenChannel { get; private set; }
        public byte[] BlueChannel { get; private set; }
        public int FileSize { get; private set; }
        public int ImageWidth { get; private set; }
        public int ImageHeight { get; private set; }

        public void OpenImage(string fileName)
        {
            FileName = fileName;
            using (FileStream fs = File.OpenRead(fileName))
            {
                int idx = 0;
                FileSize = Convert.ToInt32(fs.Length);
                int channelLength = FileSize / 3;
                RedChannel = new byte[channelLength];
                GreenChannel = new byte[channelLength];
                BlueChannel = new byte[channelLength];
                byte b;
                for(idx = 0; idx < FileSize; idx++)
                {
                    b = Convert.ToByte(fs.ReadByte());
                    switch(idx % 3)
                    {
                        case 0:
                            RedChannel[idx / 3] = b;
                            break;
                        case 1:
                            GreenChannel[idx / 3] = b;
                            break;
                        case 2:
                            BlueChannel[idx / 3] = b;
                            break;
                    }
                }
            }
            ImageWidth = ImageHeight = 0;
        }

        public Image GetImage(int width)
        {
            ImageWidth = width;
            ImageHeight = FileSize / ImageWidth / 3;
            Image image = new Bitmap(ImageWidth, ImageHeight);
            Pen p;
            Color c;
            using (Graphics g = Graphics.FromImage(image))
            {
                for(int i = 0; i < ImageHeight; i++)
                {
                    for(int j = 0; j < ImageWidth; j++)
                    {
                        c = Color.FromArgb(RedChannel[j + i * ImageWidth], GreenChannel[j + i * ImageWidth], BlueChannel[j + i * ImageWidth]);
                        p = new Pen(c);
                        g.DrawRectangle(p, j, i, 1, 1);
                        p.Dispose();
                    }
                }
            }

            return image;
        }
    }
}
