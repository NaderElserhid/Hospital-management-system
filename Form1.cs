using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Hospital_management_system
{
    public partial class LogInPage : Form
    {
        
        public LogInPage()
        {
            InitializeComponent();
        }
        string connectionString = @"Data Source=C:\Users\nadir\OneDrive\Desktop\C#\Hospital management system\bin\Debug\hospetalDB.db;Version=3;";

        private void LogInPage_Load(object sender, EventArgs e)
        {

    }
        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string username = TexBoxUserName.Text;
            string password = TexBoxPassword.Text;

            // Dummy credentials for testing
            string validUsername = "admin";
            string validPassword = "1234";

            if (username == validUsername && password == validPassword)
            {
              //MessageBox.Show("Login Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Hide();
                Form form1 = new PrograssParPage();
                 form1.Show();
            }
            else
            {
              MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
