using System;
using System.Data;
using System.Windows.Forms;

namespace Hospital_management_system
{
    public partial class LogInPage : Form
    {
        public static DatabaseHelper DbHelper;

        public LogInPage()
        {
            InitializeComponent();
            DbHelper = new DatabaseHelper("database.db");
        }

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


            try {
                // Check if the user exists in the database
                string query = $"SELECT * FROM doctors WHERE username = '{username}' AND password = '{password}'";
                DataTable dt = DbHelper.ExecuteQuery(query);

                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("Login Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Hide();
                    Form form1 = new PrograssParPage();
                    form1.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
