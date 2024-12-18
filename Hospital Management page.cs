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
            LoadPatients();

            // Initialize the buttons to be hidden or disabled initially
            btnEdit.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            Form form2 = new Form2();
            form2.Show();
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
                            LoadPatients();     // Refresh the ListView
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
            textBoxIDnumber.Enabled = true;
        }

        private void showButton_Click(object sender, EventArgs e)
        {
         
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Show the buttons only if an item is selected
            if (listView1.SelectedItems.Count > 0)
            {
                btnEdit.Visible = true;  // Enable the Edit button
                btnDelete.Visible = true;  // Enable the Delete button
                btnAdd.Visible = false;  // Disable the Add button
                btnSave.Visible = false;  // Disable the Save button
            }
            else
            {
                btnEdit.Visible = false;  // Disable the Edit button
                btnDelete.Visible = false;  // Disable the Delete button
                btnAdd.Visible = true;  // Enable the Add button
                btnSave.Visible = false;  // Disable the Save button
            }

            ClearInputFields(); // Clear the input fields when an item is selected
        }


        private void LoadPatients()
        {
            // Clear existing items
            listView1.Items.Clear();

            try
            {
                // Query to fetch all patients
                string query = "SELECT id, name, surname, phone, gender, age, disease, registration_date FROM patients";

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new ListViewItem
                                var item = new ListViewItem(reader["id"].ToString());
                                item.SubItems.Add(reader["name"].ToString());
                                item.SubItems.Add(reader["surname"].ToString());
                                item.SubItems.Add(reader["phone"].ToString());
                                item.SubItems.Add(reader["gender"].ToString());
                                item.SubItems.Add(reader["age"].ToString());
                                item.SubItems.Add(reader["disease"].ToString());
                                item.SubItems.Add(reader["registration_date"].ToString());

                                // Add the item to the ListView
                                listView1.Items.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while loading patients: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the ListView
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a patient to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the ID of the selected patient from the first column of the selected ListViewItem
            string selectedId = listView1.SelectedItems[0].SubItems[0].Text;

            // Confirm deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this patient?", "Confirm Deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.No)
            {
                return;
            }


            try
            {
                // Build SQL query to delete the patient with the selected ID
                string query = "DELETE FROM patients WHERE id = @id;";

                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        // Add the ID parameter to prevent SQL injection
                        cmd.Parameters.AddWithValue("@id", selectedId);

                        // Execute the query
                        int rowsAffected = cmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Patient deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPatients(); // Refresh the ListView
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete patient.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the ListView
            if (listView1.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a patient to delete.", "No Selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if an item is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected item from the ListView
                ListViewItem selectedItem = listView1.SelectedItems[0];

                // Assuming your ListView has columns: ID, Name, Surname, etc.
                // Fill the form fields with the selected item data
                textBoxIDnumber.Text = selectedItem.SubItems[0].Text;  // ID (assuming it's in the first column)
                textBoxIDnumber.Enabled = false;
                textBoxFirstName.Text = selectedItem.SubItems[1].Text; // Name (second column)
                textBoxLastName.Text = selectedItem.SubItems[2].Text;  // Surname (third column)
                maskedTextBoxPhoneNumber.Text = selectedItem.SubItems[3].Text; // Phone (fourth column)
                comboBoxGender.Text = selectedItem.SubItems[4].Text; // Gender (fifth column)
                textBoxAge.Text = selectedItem.SubItems[5].Text; // Age (sixth column)
                textBoxDisease.Text = selectedItem.SubItems[6].Text; // Disease (seventh column)
                maskedTextBoxRegistrationDate.Text = selectedItem.SubItems[7].Text; // Registration Date (eighth column)
            
                btnSave.Visible = true;  // Enable the Save button
                btnEdit.Visible = false;  // Disable the Edit button
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Check if an item is selected in the ListView
            if (listView1.SelectedItems.Count > 0)
            {
                // Get the selected item from the ListView
                ListViewItem selectedItem = listView1.SelectedItems[0];

                // Retrieve the data from the form fields
                string id = textBoxIDnumber.Text;
                string name = textBoxFirstName.Text;
                string surname = textBoxLastName.Text;
                string phone = maskedTextBoxPhoneNumber.Text;
                string gender = comboBoxGender.Text;
                string age = textBoxAge.Text;
                string disease = textBoxDisease.Text;
                string data = maskedTextBoxRegistrationDate.Text;

                // Update the database with the new values
                using (var conn = DbHelper.GetConnection())
                {
                    conn.Open();

                    string query = "UPDATE patients SET " +
                                   "name = @name, " +
                                   "surname = @surname, " +
                                   "phone = @phone, " +
                                   "gender = @gender, " +
                                   "age = @age, " +
                                   "disease = @disease, " +
                                   "registration_date = @data " +
                                   "WHERE id = @id";
                    
                    using (SQLiteCommand command = new SQLiteCommand(query, conn))
                    {
                        // Add parameters to avoid SQL injection
                        command.Parameters.AddWithValue("@id", id);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@surname", surname);
                        command.Parameters.AddWithValue("@phone", phone);
                        command.Parameters.AddWithValue("@gender", gender);
                        command.Parameters.AddWithValue("@age", age);
                        command.Parameters.AddWithValue("@disease", disease);
                        command.Parameters.AddWithValue("@data", data);

                        // Execute the query
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Patient updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadPatients(); // Refresh the ListView
                            ClearInputFields(); // Clear the input fields
                            btnSave.Visible = false;  // Disable the Save button
                            btnEdit.Visible = false;  // Enable the Edit button
                            btnDelete.Visible = false;
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            MessageBox.Show("Failed to delete patient.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
        }

        private void btnLogeOut_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Logout Successful", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Hide();
            Form form1 = new LogInPage();
            form1.Show();

        }
    }
}
