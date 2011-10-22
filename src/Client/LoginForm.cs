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
    public sealed partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        public String Username { get; private set; }
        public String Password { get; private set; }
        public bool Completed { get; private set; }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Set the button to disabled because the user has to insert some data
            LoginButton.Enabled = false;

            // Set the default values for our properties
            Username = null;
            Password = null;
            Completed = false;
        }

        private void UsernameBox_TextChanged(object sender, EventArgs e)
        {
            // Check if we have valid user input for username and password
            if (UsernameBox.Text.Length > 2 && PasswordBox.Text.Length > 2)
            {
                // If we have valid input, write them to our variables and enable the login button
                Username = UsernameBox.Text;
                Password = PasswordBox.Text;
                LoginButton.Enabled = true;
            }
            else
            {
                // If we don't have valid input, erase the current variables and disable the login button
                Username = null;
                Password = null;
                LoginButton.Enabled = false;
            }
        }

        private void LoginButton_Click(object sender, EventArgs e)
        {
            // If the login button was clicked, set the Completed-Property to true and then close the form
            Completed = true;
            Close();
        }

        private void AbortButton_Click(object sender, EventArgs e)
        {
            // If the cancel button was clicked, set the Completed-Property to false and then close the form
            // Furthermore, erase our input properties to prevent leaking password or username
            Username = null;
            Password = null;
            Completed = false;
            Close();
        }

        private void PasswordBox_TextChanged(object sender, EventArgs e)
        {
            // Redirect the call to UsernameBox_TextChanged(sender, e) since it would do the same
            UsernameBox_TextChanged(sender, e);
        }
    }
}
