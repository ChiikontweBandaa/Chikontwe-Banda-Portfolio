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
    public partial class VehicleSearchForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";
        public VehicleSearchForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string licensePlate = txtLicensePlate.Text.Trim();

            if (string.IsNullOrEmpty(licensePlate))
            {
                lblStatus.Text = "Error: Please enter a license plate.";
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"
    SELECT 
        v.VehicleId,
        v.LicensePlate,
        v.Make,
        v.Model,
        t.TicketId,
        t.EntryTime,
        t.Status
    FROM Vehicles v
    JOIN (
        SELECT *, ROW_NUMBER() OVER (PARTITION BY VehicleId ORDER BY EntryTime DESC) AS RowNum
        FROM ParkingTickets
    ) t ON v.VehicleId = t.VehicleId AND t.RowNum = 1
    WHERE v.LicensePlate LIKE @LicensePlate";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@LicensePlate", "%" + licensePlate + "%");

                    DataTable results = new DataTable();
                    adapter.Fill(results);

                    if (results.Rows.Count > 0)
                    {
                        dgvResults.DataSource = results;
                        lblStatus.Text = $"Found {results.Rows.Count} records.";
                    }
                    else
                    {
                        lblStatus.Text = "No matching vehicles found.";
                        dgvResults.DataSource = null; // Clear previous results
                    }
                }
            }
            catch (Exception ex)
            {
                lblStatus.Text = $"Error: {ex.Message}";
            }
        }

      

        private void dgvResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvResults.Rows[e.RowIndex];

            if (row.Cells["TicketId"].Value != null)
            {
                int ticketId = Convert.ToInt32(row.Cells["TicketId"].Value);
                string recordType = row.Cells["RecordType"].Value.ToString();

                if (recordType == "Reservation")
                {
                    // Open ReservationDetailsForm instead
                    ReservationDetailsForm reservationDetailsForm = new ReservationDetailsForm();
                    reservationDetailsForm.ShowDialog();

                }
                else
                {
                    // Open standard TicketDetailsForm
                    TicketDetailsForm detailsForm = new TicketDetailsForm(ticketId);
                    detailsForm.ShowDialog();
                }
            }
        }

    }
}

