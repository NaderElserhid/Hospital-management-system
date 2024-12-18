using System;
using System.Data;
using System.Data.SQLite;

namespace Hospital_management_system
{
    public class DatabaseHelper
    {
        private string connectionString;

        public DatabaseHelper(string dbPath)
        {
            connectionString = $"Data Source={dbPath};Version=3;";
            InitializeDatabase();
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public DataTable ExecuteQuery(string query)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public int ExecuteNonQuery(string query)
        {
            using (SQLiteConnection conn = GetConnection())
            {
                conn.Open();
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        private void InitializeDatabase()
        {
            try
            {
                using (SQLiteConnection conn = GetConnection())
                {
                    conn.Open();

                    // Create doctors table if it doesn't exist
                    string createDoctorsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS doctors (
                            id INTEGER PRIMARY KEY AUTOINCREMENT,
                            username TEXT NOT NULL UNIQUE,
                            password TEXT NOT NULL
                        );";
                    using (SQLiteCommand cmd = new SQLiteCommand(createDoctorsTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Insert default doctor
                    string insertDefaultDoctorQuery = @"
                        INSERT OR IGNORE INTO doctors (username, password) 
                        VALUES ('nadir', '1234');";
                    using (SQLiteCommand cmd = new SQLiteCommand(insertDefaultDoctorQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Create patients table if it doesn't exist
                    string createPatientsTableQuery = @"
                        CREATE TABLE IF NOT EXISTS patients (
                            id INTEGER PRIMARY KEY AUTOINCREMENT
                        );";
                    using (SQLiteCommand cmd = new SQLiteCommand(createPatientsTableQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }

                    // Ensure the patients table has the required columns
                    EnsurePatientsTableColumns(conn);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error initializing database: " + ex.Message);
                throw;
            }
        }

        private void EnsurePatientsTableColumns(SQLiteConnection conn)
        {
            string[] requiredColumns = {
                "id INTEGER PRIMARY KEY AUTOINCREMENT",
                "name TEXT",
                "surname TEXT",
                "phone TEXT",
                "gender TEXT",
                "age INTEGER",
                "disease TEXT",
                "registration_date TEXT"
            };

            foreach (string column in requiredColumns)
            {
                string columnName = column.Split(' ')[0];
                string checkColumnQuery = $"PRAGMA table_info(patients);";
                bool columnExists = false;

                using (SQLiteCommand cmd = new SQLiteCommand(checkColumnQuery, conn))
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (reader["name"].ToString() == columnName)
                        {
                            columnExists = true;
                            break;
                        }
                    }
                }

                if (!columnExists)
                {
                    string addColumnQuery = $"ALTER TABLE patients ADD COLUMN {column};";
                    using (SQLiteCommand cmd = new SQLiteCommand(addColumnQuery, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
