using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Fade_Pictures
{
    public partial class Form1 : Form
        
    {
    private List<Bitmap> _bitmaps = new List<Bitmap>();
    private Random _random = new Random();


    public Form1()
        {
            InitializeComponent();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
           if (openFileDialog1.ShowDialog() == DialogResult.OK) 
            {
                menuStrip1.Enabled = false;
                trackBar1.Enabled = false;
                pictureBox1.Image = null; 
                _bitmaps.Clear();
                var bitmap = new Bitmap(openFileDialog1.FileName);
                await Task.Run(() => { RunProcessing(bitmap); });
                menuStrip1.Enabled=true;
                trackBar1.Enabled=true;
            }
        }

        private void RunProcessing(Bitmap bitmap) 
        { 
            var pixels = GetPixels(bitmap);
            var pixelsInStep = (bitmap.Width*bitmap.Height)/ trackBar1.Maximum;
            String size = " Widh  " + bitmap.Width.ToString() + " Height  " + bitmap.Height.ToString();
            _bitmaps.Add(bitmap);
            pictureBox1.Image = bitmap;
            for (int i = 0; i < trackBar1.Maximum; i++) 
            {
                for (int j = 0; j < pixelsInStep; j++) 
                {
                    var index = _random.Next(pixels.Count);
                    pixels.RemoveAt(index);
                }
                var currentBitmap = new Bitmap(bitmap.Width, bitmap.Height);
                foreach (var pixel in pixels)     
                currentBitmap.SetPixel(pixel.Point.X, pixel.Point.Y, pixel.Color);
                _bitmaps.Add(currentBitmap);

                pictureBox1.Image = currentBitmap;
                Text = $"Progress = {i + 1}%" + size;
                //this.Invoke(new Action(() =>
                //{ Text = $"Progress = {i + 1}%" + size; }));

            }

            Text = trackBar1.Value.ToString();
            pictureBox1.Image = _bitmaps[100-trackBar1.Value];
        }

        private List<Pixel> GetPixels(Bitmap bitmap) 
        {
            var pixels = new List<Pixel>(bitmap.Width*bitmap.Height);
            for (int y = 0; y < bitmap.Height; y++) 
            {
                for (int x = 0; x < bitmap.Width; x++) 
                {
                    pixels.Add(new Pixel()
                    { 
                        Color = bitmap.GetPixel(x, y),
                        Point = new Point() { X = x, Y = y },
                    });
                }
            }
            return pixels;
        
        }


        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Text = trackBar1.Value.ToString();
            if (_bitmaps == null || _bitmaps.Count == 0)
            return;
            pictureBox1.Image = _bitmaps[100-trackBar1.Value];
        }
    }
}
