using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace HospitalParkingSystem
{
    public partial class RegisterVehicleForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";

        public RegisterVehicleForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrEmpty(txtLicensePlate.Text) || string.IsNullOrEmpty(txtMake.Text) ||
                string.IsNullOrEmpty(txtModel.Text) || string.IsNullOrEmpty(txtColor.Text) ||
                string.IsNullOrEmpty(txtOwnerName.Text))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Validate phone number if provided
            if (!string.IsNullOrEmpty(txtOwnerPhone.Text) && !IsValidPhoneNumber(txtOwnerPhone.Text))
            {
                MessageBox.Show("Please enter a valid phone number (10 digits, may include spaces or dashes).",
                    "Invalid Phone", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOwnerPhone.Focus();
                return;
            }

            // Validate email if provided
            if (!string.IsNullOrEmpty(txtOwnerEmail.Text) && !IsValidEmail(txtOwnerEmail.Text))
            {
                MessageBox.Show("Please enter a valid email address.",
                    "Invalid Email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtOwnerEmail.Focus();
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"INSERT INTO Vehicles (LicensePlate, Make, Model, Color, OwnerName, OwnerPhone, OwnerEmail)
                                   VALUES (@LicensePlate, @Make, @Model, @Color, @OwnerName, @OwnerPhone, @OwnerEmail)";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@LicensePlate", txtLicensePlate.Text);
                    command.Parameters.AddWithValue("@Make", txtMake.Text);
                    command.Parameters.AddWithValue("@Model", txtModel.Text);
                    command.Parameters.AddWithValue("@Color", txtColor.Text);
                    command.Parameters.AddWithValue("@OwnerName", txtOwnerName.Text);
                    command.Parameters.AddWithValue("@OwnerPhone", string.IsNullOrEmpty(txtOwnerPhone.Text) ? (object)DBNull.Value : FormatPhoneNumber(txtOwnerPhone.Text));
                    command.Parameters.AddWithValue("@OwnerEmail", string.IsNullOrEmpty(txtOwnerEmail.Text) ? (object)DBNull.Value : txtOwnerEmail.Text.Trim());

                    connection.Open();
                    command.ExecuteNonQuery();

                    MessageBox.Show("Vehicle registered successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
            }
            catch (SqlException sqlEx)
            {
                if (sqlEx.Number == 2627) // Unique constraint violation
                {
                    MessageBox.Show("A vehicle with this license plate already exists.",
                        "Duplicate Entry", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Database error: " + sqlEx.Message,
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error registering vehicle: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Email validation using regular expression
        private bool IsValidEmail(string email)
        {
            try
            {
                // Simple email pattern - can be made more sophisticated
                string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                return Regex.IsMatch(email, pattern, RegexOptions.IgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        // Phone number validation (US format)
        private bool IsValidPhoneNumber(string phone)
        {
            // Remove all non-digit characters
            string digitsOnly = Regex.Replace(phone, @"[^\d]", "");

            // Check if we have 10 digits (US phone number)
            return digitsOnly.Length == 10;
        }

        // Format phone number consistently for storage
        private string FormatPhoneNumber(string phone)
        {
            string digitsOnly = Regex.Replace(phone, @"[^\d]", "");

            if (digitsOnly.Length == 10)
            {
                return $"({digitsOnly.Substring(0, 3)}) {digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6)}";
            }

            return digitsOnly; // return as-is if not standard length
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
