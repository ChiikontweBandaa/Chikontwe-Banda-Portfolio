using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalParkingSystem
{
    public partial class ParkingSectionsForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";

        public ParkingSectionsForm()
        {
            InitializeComponent();
            LoadAllSections();
        }

        private void LoadAllSections()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT ps.SectionName, COUNT(s.SpaceId) AS TotalSpaces, 
                                   SUM(CASE WHEN s.Status = 'Available' THEN 1 ELSE 0 END) AS AvailableSpaces
                                   FROM ParkingSections ps
                                   JOIN ParkingSpaces s ON ps.SectionId = s.SectionId
                                   GROUP BY ps.SectionName";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridViewSections.DataSource = table;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading sections: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVisitors_Click(object sender, EventArgs e)
        {
            LoadSpacesBySections("Visitors");
        }

        private void btnPatients_Click(object sender, EventArgs e)
        {
            LoadSpacesBySections("Patients");
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            LoadSpacesBySections("Hospital Staff");
        }

        private void btnEmergency_Click(object sender, EventArgs e)
        {
            LoadSpacesBySections("Emergency Vehicles");
        }

        private void btnDisabled_Click(object sender, EventArgs e)
        {
            LoadSpacesBySections("Disabled or Infirm");
        }

        private void LoadSpacesBySections(string sectionName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT s.SpaceNumber, s.Status 
                                   FROM ParkingSpaces s
                                   JOIN ParkingSections ps ON s.SectionId = ps.SectionId
                                   WHERE ps.SectionName = @SectionName
                                   ORDER BY s.SpaceNumber";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SectionName", sectionName);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dataGridViewSpaces.DataSource = table;
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading spaces: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }

       
    
}
