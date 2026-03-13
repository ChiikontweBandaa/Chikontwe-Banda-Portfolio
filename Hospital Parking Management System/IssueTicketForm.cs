using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalParkingSystem
{
    public partial class IssueTicketForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";
        public IssueTicketForm()
        {
            InitializeComponent();
            LoadAvailableSpaces();
            LoadVehicles();
            dtpEntryTime.Value = DateTime.Now;
        }

        private void LoadAvailableSpaces()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT s.SpaceId, ps.SectionName + ' - ' + s.SpaceNumber AS SpaceInfo
                                   FROM ParkingSpaces s
                                   JOIN ParkingSections ps ON s.SectionId = ps.SectionId
                                   WHERE s.Status = 'Available'";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    cmbParkingSpace.DataSource = table;
                    cmbParkingSpace.DisplayMember = "SpaceInfo";
                    cmbParkingSpace.ValueMember = "SpaceId";
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading parking spaces: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadVehicles()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT VehicleId, LicensePlate + ' - ' + Make + ' ' + Model AS VehicleInfo FROM Vehicles";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    cmbVehicle.DataSource = table;
                    cmbVehicle.DisplayMember = "VehicleInfo";
                    cmbVehicle.ValueMember = "VehicleId";
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading vehicles: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private void btnIssueTicket_Click(object sender, EventArgs e)

        {

            if (cmbVehicle.SelectedValue == null || cmbParkingSpace.SelectedValue == null)
            {
                MessageBox.Show("Please select both vehicle and parking space.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int vehicleId = (int)cmbVehicle.SelectedValue;
            int spaceId = (int)cmbParkingSpace.SelectedValue;
            DateTime entryTime = dtpEntryTime.Value;
            int ticketId = 0;
           


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Start transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert ticket
                        string ticketQuery = @"INSERT INTO ParkingTickets (VehicleId, SpaceId, EntryTime, Status)
                                              VALUES (@VehicleId, @SpaceId, @EntryTime, 'Active'); SELECT SCOPE_IDENTITY();";

                        SqlCommand ticketCommand = new SqlCommand(ticketQuery, connection, transaction);
                        ticketCommand.Parameters.AddWithValue("@VehicleId", vehicleId);
                        ticketCommand.Parameters.AddWithValue("@SpaceId", spaceId);
                        ticketCommand.Parameters.AddWithValue("@EntryTime", entryTime);
                        ticketId = Convert.ToInt32(ticketCommand.ExecuteScalar());
                        ticketCommand.ExecuteNonQuery();

                        // Update parking space status
                        string spaceQuery = "UPDATE ParkingSpaces SET Status = 'Occupied' WHERE SpaceId = @SpaceId";
                        SqlCommand spaceCommand = new SqlCommand(spaceQuery, connection, transaction);
                        spaceCommand.Parameters.AddWithValue("@SpaceId", spaceId);
                        spaceCommand.ExecuteNonQuery();

                        // Commit transaction
                        transaction.Commit();

                        
                       

                        MessageBox.Show($"Ticket issued successfully! Ticket ID: {ticketId}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Show ticket details
                        TicketDetailsForm detailsForm = new TicketDetailsForm(ticketId);
                        detailsForm.ShowDialog();




                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error issuing ticket: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}

