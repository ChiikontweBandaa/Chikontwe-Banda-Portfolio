using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalParkingSystem
{
    public partial class ReservationsForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";
        public ReservationsForm()
        {
            InitializeComponent();
            LoadParkingSections();
            dtpReservationDate.Value = DateTime.Now;
        }

        private void LoadParkingSections()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = "SELECT SectionId, SectionName FROM ParkingSections";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                   cmbSection.DataSource = table;
                    cmbSection.DisplayMember = "SectionName";
                    cmbSection.ValueMember = "SectionId";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading parking sections: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSection_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbSection.SelectedValue != null && int.TryParse(cmbSection.SelectedValue.ToString(), out int sectionId))
            {
                LoadAvailableSpaces(sectionId);
            }
        }



        private void LoadAvailableSpaces(int sectionId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT SpaceId, SpaceNumber 
                                   FROM ParkingSpaces 
                                   WHERE SectionId = @SectionId AND Status = 'Available'
                                   ORDER BY SpaceNumber";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    adapter.SelectCommand.Parameters.AddWithValue("@SectionId", sectionId);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    cmbSpace.DataSource = table;
                    cmbSpace.DisplayMember = "SpaceNumber";
                    cmbSpace.ValueMember = "SpaceId";
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading available spaces: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


       

        private void btnMakeReservation_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCustomerName.Text) || string.IsNullOrEmpty(txtLicensePlate.Text) ||
                string.IsNullOrEmpty(txtMake.Text) || string.IsNullOrEmpty(txtModel.Text) ||
                cmbSection.SelectedValue == null || cmbSpace.SelectedValue == null)
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(txtEmail.Text) && !ValidateEmail(txtEmail.Text))
            {
                MessageBox.Show("Invalid email format. Example: user@example.com",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!string.IsNullOrEmpty(txtPhone.Text) && !ValidatePhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Invalid phone number. Example: +260123456789 or 0971234567",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DateTime reservationDate = dtpReservationDate.Value;
            if (reservationDate < DateTime.Now)
            {
                MessageBox.Show("Reservation date cannot be in the past.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int spaceId = (int)cmbSpace.SelectedValue;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Start transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // First check if the space is still available
                        string checkQuery = "SELECT Status FROM ParkingSpaces WHERE SpaceId = @SpaceId";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection, transaction);
                        checkCommand.Parameters.AddWithValue("@SpaceId", spaceId);
                        string status = checkCommand.ExecuteScalar()?.ToString();

                        if (status != "Available")
                        {
                            MessageBox.Show("The selected parking space is no longer available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            transaction.Rollback();
                            return;
                        }

                        // Insert vehicle if not exists
                        string vehicleQuery = @"IF NOT EXISTS (SELECT 1 FROM Vehicles WHERE LicensePlate = @LicensePlate)
                                             INSERT INTO Vehicles (LicensePlate, Make, Model, Color, OwnerName, OwnerPhone, OwnerEmail)
                                             VALUES (@LicensePlate, @Make, @Model, @Color, @OwnerName, @OwnerPhone, @OwnerEmail)";
                        SqlCommand vehicleCommand = new SqlCommand(vehicleQuery, connection, transaction);
                        vehicleCommand.Parameters.AddWithValue("@LicensePlate", txtLicensePlate.Text);
                        vehicleCommand.Parameters.AddWithValue("@Make", txtMake.Text);
                        vehicleCommand.Parameters.AddWithValue("@Model", txtModel.Text);
                        vehicleCommand.Parameters.AddWithValue("@Color", txtColor.Text);
                        vehicleCommand.Parameters.AddWithValue("@OwnerName", txtCustomerName.Text);
                        vehicleCommand.Parameters.AddWithValue("@OwnerPhone", txtPhone.Text ?? (object)DBNull.Value);
                        vehicleCommand.Parameters.AddWithValue("@OwnerEmail", txtEmail.Text ?? (object)DBNull.Value);
                        vehicleCommand.ExecuteNonQuery();

                        // Get vehicle ID
                        string vehicleIdQuery = "SELECT VehicleId FROM Vehicles WHERE LicensePlate = @LicensePlate";
                        SqlCommand vehicleIdCommand = new SqlCommand(vehicleIdQuery, connection, transaction);
                        vehicleIdCommand.Parameters.AddWithValue("@LicensePlate", txtLicensePlate.Text);
                        int vehicleId = Convert.ToInt32(vehicleIdCommand.ExecuteScalar());

                        // Create reservation (as a ticket with Reserved status)
                        string ticketQuery = @"INSERT INTO ParkingTickets (VehicleId, SpaceId, EntryTime, Status)
                                            VALUES (@VehicleId, @SpaceId, @EntryTime, 'Reserved')";
                        SqlCommand ticketCommand = new SqlCommand(ticketQuery, connection, transaction);
                        ticketCommand.Parameters.AddWithValue("@VehicleId", vehicleId);
                        ticketCommand.Parameters.AddWithValue("@SpaceId", spaceId);
                        ticketCommand.Parameters.AddWithValue("@EntryTime", reservationDate);
                        ticketCommand.ExecuteNonQuery();

                        // Update parking space status
                        string spaceQuery = "UPDATE ParkingSpaces SET Status = 'Reserved' WHERE SpaceId = @SpaceId";
                        SqlCommand spaceCommand = new SqlCommand(spaceQuery, connection, transaction);
                        spaceCommand.Parameters.AddWithValue("@SpaceId", spaceId);
                        spaceCommand.ExecuteNonQuery();

                        // Commit transaction
                        transaction.Commit();

                        MessageBox.Show("Reservation made successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        this.Close();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error making reservation: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false; // Skip if optional

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email; // True if valid format
            }
            catch
            {
                return false;
            }
        }

        private bool ValidatePhoneNumber(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return false; // Skip if optional

            // Regex: Allows optional '+' followed by 9-15 digits
            var regex = new Regex(@"^\+?[0-9]{9,15}$");
            return regex.IsMatch(phone);
        }
    }
}



        

