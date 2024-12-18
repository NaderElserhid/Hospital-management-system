using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Hospital_management_system
{
    public partial class Form2 : Form
    {
        public static DatabaseHelper DbHelper;

        public Form2()
        {
            InitializeComponent();
            DbHelper = new DatabaseHelper("database.db");
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            LoadCharts();
        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void LoadCharts()
        {
            // Clear existing data in the charts
            chart1.Series.Clear();
            chart2.Series.Clear();


            using (var connection = DbHelper.GetConnection())
            {
                connection.Open();

                // Query for Ages (chart1)
                string ageQuery = "SELECT age, COUNT(*) AS count FROM patients GROUP BY age";
                
                using (SQLiteCommand ageCommand = new SQLiteCommand(ageQuery, connection))
                {
                    using (SQLiteDataReader reader = ageCommand.ExecuteReader())
                    {
                        // Add series to chart1
                        Series ageSeries = new Series("Age Distribution");
                        ageSeries.ChartType = SeriesChartType.Column;

                        while (reader.Read())
                        {
                            string age = reader["age"].ToString();
                            int count = Convert.ToInt32(reader["count"]);
                            ageSeries.Points.AddXY(age, count);
                        }

                        chart1.Series.Add(ageSeries);
                    }
                }

                // Query for Genders (chart2)
                string genderQuery = "SELECT gender, COUNT(*) AS count FROM patients GROUP BY gender";
                using (SQLiteCommand genderCommand = new SQLiteCommand(genderQuery, connection))
                {
                    using (SQLiteDataReader reader = genderCommand.ExecuteReader())
                    {
                        // Add series to chart2
                        Series genderSeries = new Series("Gender Distribution");
                        genderSeries.ChartType = SeriesChartType.Pie;

                        while (reader.Read())
                        {
                            string gender = reader["gender"].ToString();
                            int count = Convert.ToInt32(reader["count"]);
                            genderSeries.Points.AddXY(gender, count);
                        }

                        chart2.Series.Add(genderSeries);
                    }
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
