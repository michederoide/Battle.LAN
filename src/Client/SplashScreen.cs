using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class SplashScreen : Form
    {
        public SplashScreen()
        {
            InitializeComponent();
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            // Save the splash image in a local variable
            Bitmap splashImage = Client.Properties.Resources.Splash;
            // Set the size of the screen to the splash image size
            this.ClientSize = new Size(splashImage.Width, splashImage.Height);
            // Set the splash image as background
            this.BackgroundImage = splashImage;
        }
    }
}
