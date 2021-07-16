using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HalconDotNet;

namespace get_width_height_click_region
{
    public partial class Form1 : Form
    {
        HObject _image;
        HObject _connectedRegions;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HOperatorSet.ReadImage(out _image, "letters");
            hSmartWindowControl1.HalconWindow.DispObj(_image);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            HOperatorSet.Threshold(_image, out HObject region, 0, 200);
            HOperatorSet.Connection(region, out _connectedRegions);
            hSmartWindowControl1.HalconWindow.SetColored(12);
            hSmartWindowControl1.HalconWindow.DispObj(_connectedRegions);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            hSmartWindowControl1.MouseWheel += hSmartWindowControl1.HSmartWindowControl_MouseWheel;
        }

        private void hSmartWindowControl1_HMouseMove(object sender, HMouseEventArgs e)
        {
            if (_image == null || _connectedRegions == null)
            {
                return;
            }

            getWidthAndHeight(_connectedRegions, e.X, e.Y, out HObject destRegions, out int width, out int height);

            if (width == 0 || height == 0)
            {
                return;
            }
            string text = "width:" + width.ToString() + " height:" + height.ToString();

            hSmartWindowControl1.HalconWindow.DispObj(_image);
            hSmartWindowControl1.HalconWindow.DispObj(_connectedRegions);
            hSmartWindowControl1.HalconWindow.DispText(text, "image", e.Y, e.X + 10, "black", null, null);
        }

        /// <summary>
        /// get width and height from clicked region
        /// </summary>
        /// <param name="regions">connected regions</param>
        /// <param name="x">clicked x</param>
        /// <param name="y">clicked y</param>
        /// <param name="destRegions">clicked region</param>
        /// <param name="width">clicked region width</param>
        /// <param name="height">clicked region height</param>
        private void getWidthAndHeight(HObject regions, double x, double y, out HObject destRegions, out int width, out int height)
        {
            destRegions = null;
            width = 0;
            height = 0;
            if (regions == null)
            {
                return;
            }
            HOperatorSet.SelectRegionPoint(regions, out destRegions, y, x);
            HOperatorSet.SmallestRectangle1(destRegions, out HTuple row1, out HTuple column1, out HTuple row2, out HTuple column2);
            if (row1.Length == 0 || column1.Length == 0 || row2.Length == 0 || column2.Length == 0)
            {
                return;
            }
            width = column2 - column1 + 1;
            height = row2 - row1 + 1;
        }
    }
}
