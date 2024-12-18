using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace Hospital_management_system
{
    public partial class Hospital_Management_page : Form
    {
        public static DatabaseHelper DbHelper;

        public Hospital_Management_page()
        {
            InitializeComponent();

            DbHelper = new DatabaseHelper("database.db");

        }

        private void Hospital_Management_page_Load(object sender, EventArgs e)
        {

        }

        private void btnChart_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string id= textBoxIDnumber.Text;
            string name = textBoxFirstName.Text;
            string surname = textBoxLastName.Text;
            string phone = maskedTextBoxPhoneNumber.Text;
            string gender = comboBoxGender.Text;
            string age = textBoxAge.Text;
            string disease = textBoxDisease.Text;
            string registrationDate = maskedTextBoxRegistrationDate.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Name, Surname, and Phone fields are mandatory.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            try
            {
                // Build SQL query to insert data into the patients table
                string query = @"
            INSERT INTO patients (name, surname, phone, gender, age, disease, registration_date)
            VALUES (@name, @surname, @phone, @gender, @age, @disease, @registrationDate);";

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        // Add parameters to prevent SQL injection
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@surname", surname);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@age", string.IsNullOrEmpty(age) ? (object)DBNull.Value : int.Parse(age));
                        cmd.Parameters.AddWithValue("@disease", disease);
                        cmd.Parameters.AddWithValue("@registrationDate", registrationDate);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Patient added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ClearInputFields(); // Optional: Clear the input fields after a successful insert
                        }
                        else
                        {
                            MessageBox.Show("Failed to add patient.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Optional method to clear input fields after adding a patient
        private void ClearInputFields()
        {
            textBoxIDnumber.Clear();
            textBoxFirstName.Clear();
            textBoxLastName.Clear();
            maskedTextBoxPhoneNumber.Clear();
            comboBoxGender.SelectedIndex = -1;
            textBoxAge.Clear();
            textBoxDisease.Clear();
            maskedTextBoxRegistrationDate.Clear();
        }

        private void showButton_Click(object sender, EventArgs e)
        {
            try {
                string query = "SELECT * FROM patients;";
                var dt = DbHelper.ExecuteQuery(query);
                if (dt.Rows.Count > 0)
                {
                    string patientsList = "";
                    foreach (DataRow row in dt.Rows)
                    {
                        patientsList += $"ID: {row["id"]}, Name: {row["name"]}, Surname: {row["surname"]}";
                        MessageBox.Show(patientsList);
                    }

                }
                else
                {
                    MessageBox.Show("No patients found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch { 
                MessageBox.Show("An error occurred while fetching patients.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
