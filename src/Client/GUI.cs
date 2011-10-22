using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Security.Cryptography;

using ClientLib;


namespace Client
{
    public sealed partial class GUI : Form
    {
        public GUI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Represents the time in milliseconds the splash screen is shown
        /// </summary>
        private const int c_showSplashTime = 5000;

        /// <summary>
        /// This important variable is sent to the server to indicate this is a valid client.
        /// THIS MUST BE 0xD2AB5803 OR 0xD8BC2022, OTHERWISE THE SERVER WILL KICK THE PLAYER/MANAGER!!!
        /// NOTE: 0xD2AB5803 is a player client & 0xD8BC2022 is a manager/admin client
        /// </summary>
        private const uint c_clientHash = 0xD2AB5803;

        /// <summary>
        /// This variable represents the number of updates per second for our client
        /// </summary>
        private const int c_ticks = 100;

        /// <summary>
        /// This variable holds a thread which updates the client all (1000 / c_ticks) milliseconds
        /// </summary>
        private Thread _Updater;

        /// <summary>
        /// This property holds an object of ClientState which includes all important attributes for the client
        /// </summary>
        public ClientState ClientProperty { get; private set; }

        /// <summary>
        /// Returns if the client is a valid player client
        /// </summary>
        public bool IsValidPlayerClient { get { return c_clientHash == 0xD2AB5803; } }

        /// <summary>
        /// Returns if the client is a valid manager client (to manage/administrate a server)
        /// </summary>
        public bool IsValidManagerClient { get { return c_clientHash == 0xD8BC2022; } }

        /// <summary>
        /// This variable indicates if the client is logged in to a server or not
        /// </summary>
        public bool IsLoggedIn { get; private set; }

        /// <summary>
        /// Indicates if the client wants to shutdown
        /// </summary>
        private bool _RequestShutdown;

        private void GUI_Load(object sender, EventArgs e)
        {
            // Don't show our GUI before we did all loading jobs
            Visible = false;

            // Allow Cross-Thread-Calls (CTC)
            CheckForIllegalCrossThreadCalls = false;

            // Sets some important attributes for our GUI
            MaximumSize = MinimumSize = Size;
            MaximizeBox = false;
            IsLoggedIn = false;
            _RequestShutdown = false;

            // Startup a thread for the splash screen
            Thread t = new Thread(new ThreadStart(SplashThread));
            t.Start();

            // Startup a thread for updating the client and store the thread
            // Then we start the updater thread
            _Updater = new Thread(new ThreadStart(UpdateClient));
            _Updater.Start();

            // Check if the splash screen already showed up completely
            while (t.IsAlive) ;

            // Show our GUI because all loading was done successfully
            Visible = true;

            // Focus our GUI
            Focus();
        }

        /// <summary>
        /// Shows the splash screen for a specific time and closes it afterwards
        /// DON'T CALL THIS EXPLICITLY
        /// </summary>
        private void SplashThread()
        {
            // Create a new SplashScreen object and shows it up
            SplashScreen splashScreen = new SplashScreen();
            splashScreen.Show();
            // Waits for the specific time
            Thread.Sleep(c_showSplashTime); // Default: 5000
            // Closes the splash screen
            splashScreen.Close();
        }

        /// <summary>
        /// Updates the client every tick in a seperate thread
        /// DON'T CALL THIS EXPLICITLY
        /// </summary>
        private void UpdateClient()
        {
            while (!_RequestShutdown)
            {
                // Gray out the LOGIN or LOGOUT button
                // Dependent on the current LoggedIn-State
                if (IsLoggedIn)
                {
                    LoginButton.Enabled = false;
                    LogoutButton.Enabled = true;
                }
                else
                {
                    LoginButton.Enabled = true;
                    LogoutButton.Enabled = false;
                }

                // TODO: Insert more update logic here

                // Wait until the next tick
                Thread.Sleep(1000 / c_ticks);
            }
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            _RequestShutdown = true;
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Show the Login Form
            LoginForm loginform = new LoginForm();
            loginform.ShowDialog(this);
            if (loginform.Completed)
            {
                String username = loginform.Username;
                String password = loginform.Password;
                byte[] pwbytes = MD5.Create().ComputeHash(Encoding.Default.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (byte digestbyte in pwbytes)
                {
                    sb.Append(digestbyte.ToString("x2"));
                }
                String pwdigest = sb.ToString().ToUpper();
                MessageBox.Show("Username: " + username + "\nDigest: " + pwdigest);
                // TODO: Insert login logic here
                IsLoggedIn = true;
            }
        }

        private void LogoutButton_Click(object sender, EventArgs e)
        {
            // TODO: Insert logout logic here
            IsLoggedIn = false;
        }
    }
}
