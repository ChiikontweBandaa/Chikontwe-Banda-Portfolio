using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalParkingSystem
{
    public partial class PaymentForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";
        private decimal chargeRate = 5.0m; // 5 Zambian Kwacha per hour

        public PaymentForm()
        {
            InitializeComponent();
            cmbPaymentMethod.SelectedIndex = 0;
            UpdatePaymentFields();
        }

        private void btnFindTicket_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTicketId.Text) || !int.TryParse(txtTicketId.Text, out int ticketId))
            {
                MessageBox.Show("Please enter a valid ticket ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT t.TicketId, v.LicensePlate, t.EntryTime, 
                                   DATEDIFF(MINUTE, t.EntryTime, GETDATE()) AS MinutesParked
                                   FROM ParkingTickets t
                                   JOIN Vehicles v ON t.VehicleId = v.VehicleId
                                   WHERE t.TicketId = @TicketId AND t.Status = 'Active'";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TicketId", ticketId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        lblLicensePlate.Text = reader["LicensePlate"].ToString();
                        lblEntryTime.Text = Convert.ToDateTime(reader["EntryTime"]).ToString("g");

                        int minutesParked = Convert.ToInt32(reader["MinutesParked"]);
                        decimal hoursParked = Math.Round(minutesParked / 60.0m, 2);
                        decimal totalCharge = Math.Round(hoursParked * chargeRate, 2);

                        lblDuration.Text = $"{hoursParked} hours";
                        lblTotalAmount.Text = $"{totalCharge} ZMW";
                        txtAmount.Text = totalCharge.ToString("0.00");

                        gbPayment.Enabled = true;
                    }

                    else
                    {
                        MessageBox.Show("Ticket not found or already paid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gbPayment.Enabled = false;
                    }
                    reader.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error finding ticket: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePaymentFields();
        }

        private void UpdatePaymentFields()
        {
            // Show/hide fields based on payment method
            lblMobileNumber.Visible = cmbPaymentMethod.SelectedIndex == 1;
            txtMobileNumber.Visible = cmbPaymentMethod.SelectedIndex == 1;
            lblCardNumber.Visible = cmbPaymentMethod.SelectedIndex == 2;
            txtCardNumber.Visible = cmbPaymentMethod.SelectedIndex == 2;
            lblExpiry.Visible = cmbPaymentMethod.SelectedIndex == 2;
            txtExpiry.Visible = cmbPaymentMethod.SelectedIndex == 2;
            lblCVV.Visible = cmbPaymentMethod.SelectedIndex == 2;
            txtCVV.Visible = cmbPaymentMethod.SelectedIndex == 2;
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtTicketId.Text, out int ticketId))
            {
                MessageBox.Show("Invalid ticket ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!decimal.TryParse(txtAmount.Text, out decimal amount))
            {
                MessageBox.Show("Invalid amount.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string paymentMethod = cmbPaymentMethod.SelectedItem.ToString();
            string transactionRef = "";

            // Validate payment details based on method
            if (paymentMethod == "Mobile Money" && string.IsNullOrEmpty(txtMobileNumber.Text))
            {
                MessageBox.Show("Please enter mobile number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (paymentMethod == "Card")
            {
                if (string.IsNullOrEmpty(txtCardNumber.Text) || string.IsNullOrEmpty(txtExpiry.Text) || string.IsNullOrEmpty(txtCVV.Text))
                {
                    MessageBox.Show("Please enter all card details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                transactionRef = "CARD" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            else if (paymentMethod == "Mobile Money")
            {
                transactionRef = "MM" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
            else // Cash
            {
                transactionRef = "CASH" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Start transaction
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Insert payment
                        string paymentQuery = @"INSERT INTO Payments (TicketId, Amount, PaymentMethod, PaymentTime, TransactionReference, Status)
                                             VALUES (@TicketId, @Amount, @PaymentMethod, GETDATE(), @TransactionReference, 'Completed')";
                        SqlCommand paymentCommand = new SqlCommand(paymentQuery, connection, transaction);
                        paymentCommand.Parameters.AddWithValue("@TicketId", ticketId);
                        paymentCommand.Parameters.AddWithValue("@Amount", amount);
                        paymentCommand.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                        paymentCommand.Parameters.AddWithValue("@TransactionReference", transactionRef);

                        paymentCommand.ExecuteNonQuery();

                        // Update ticket status
                        string ticketQuery = "UPDATE ParkingTickets SET Status = 'Paid', ExitTime = GETDATE() WHERE TicketId = @TicketId";
                        SqlCommand ticketCommand = new SqlCommand(ticketQuery, connection, transaction);
                        ticketCommand.Parameters.AddWithValue("@TicketId", ticketId);
                        ticketCommand.ExecuteNonQuery();

                        // Update parking space status
                        string spaceQuery = @"UPDATE ParkingSpaces SET Status = 'Available' 
                                           WHERE SpaceId = (SELECT SpaceId FROM ParkingTickets WHERE TicketId = @TicketId)";
                        SqlCommand spaceCommand = new SqlCommand(spaceQuery, connection, transaction);
                        spaceCommand.Parameters.AddWithValue("@TicketId", ticketId);
                        spaceCommand.ExecuteNonQuery();

                        // Commit transaction
                        transaction.Commit();

                        // Show receipt
                        ReceiptForm receiptForm = new ReceiptForm(ticketId, transactionRef);
                        receiptForm.ShowDialog();

                        // If mobile money or card, show notification
                        if (paymentMethod != "Cash")
                        {
                            MessageBox.Show($"Payment notification sent to customer. Transaction reference: {transactionRef}",
                                "Notification Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }

                        this.Close();
                    }

                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show("Error processing payment: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void txtMobileNumber_TextChanged(object sender, EventArgs e)
        {
            string phoneNumber = txtMobileNumber.Text.Trim();

            // Basic length check
            if (phoneNumber.Length > 10)
            {
                txtMobileNumber.Text = phoneNumber.Substring(0, 10);
                txtMobileNumber.SelectionStart = 10;
            }

            // Allow only digits
            if (!System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^[0-9]*$"))
            {
                txtMobileNumber.Text = phoneNumber.Substring(0, phoneNumber.Length - 1);
                txtMobileNumber.SelectionStart = phoneNumber.Length;
            }
        }

        private void txtMobileNumber_Validating(object sender, CancelEventArgs e)
        {
            string phoneNumber = txtMobileNumber.Text.Trim();
            bool isValid = System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^0[97]\d{8}$");

            if (!isValid && !string.IsNullOrEmpty(phoneNumber))
            {
                errorProvider1.SetError(txtMobileNumber, "Invalid Zambian mobile number (e.g., 0971234567)");
                e.Cancel = true; // Prevents focus change
            }
            else
            {
                errorProvider1.SetError(txtMobileNumber, "");
            }
        }

        private bool isFormatting = false;

        private void txtCardNumber_TextChanged(object sender, EventArgs e)
        {
            if (isFormatting) return;
            isFormatting = true;

            string cardNumber = txtCardNumber.Text.Replace(" ", "");

            // Allow only digits
            if (!System.Text.RegularExpressions.Regex.IsMatch(cardNumber, @"^[0-9]*$"))
            {
                cardNumber = cardNumber.Substring(0, cardNumber.Length - 1);
            }

            // Auto-format with spaces (e.g., 4242 4242 4242 4242)
            if (cardNumber.Length > 0)
            {
                StringBuilder formatted = new StringBuilder();
                for (int i = 0; i < cardNumber.Length; i++)
                {
                    if (i > 0 && i % 4 == 0)
                    {
                        formatted.Append(" ");
                    }
                    formatted.Append(cardNumber[i]);
                }

                txtCardNumber.Text = formatted.ToString();
                txtCardNumber.SelectionStart = txtCardNumber.Text.Length;
            }

            // Mask all digits except last 4 when not focused
            if (!txtCardNumber.Focused && cardNumber.Length > 4)
            {
                string masked = new string('•', cardNumber.Length - 4) + cardNumber.Substring(cardNumber.Length - 4);
                txtCardNumber.Text = Regex.Replace(masked, ".{4}", "$0 ").Trim();
            }

            isFormatting = false;
        }

        private void txtCardNumber_Validating(object sender, CancelEventArgs e)
        {
            string cardNumber = txtCardNumber.Text.Replace(" ", "");

            if (cardNumber.Length != 16)
            {
                errorProvider1.SetError(txtCardNumber, "Card number must be 16 digits");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(txtCardNumber, "");
            }
        }

        private void txtCardNumber_Enter(object sender, EventArgs e)
        {
            // Show full number when focused
            if (txtCardNumber.Text.Contains('•'))
            {
                txtCardNumber.Text = txtCardNumber.Text.Replace(" ", "").Replace("•", "");
            }
        }

        private void txtCardNumber_Leave(object sender, EventArgs e)
        {
            // Re-mask when focus is lost
            txtCardNumber_TextChanged(sender, e);
        }
    }
}




