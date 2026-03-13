using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalParkingSystem
{
    public partial class ReceiptForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";
        private int ticketId;
        private string transactionRef;
        public ReceiptForm(int ticketId, string transactionRef)
        {
            InitializeComponent();
            this.ticketId = ticketId;
            this.transactionRef = transactionRef;
            LoadReceiptDetails();
        }

        private void LoadReceiptDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT t.TicketId, v.LicensePlate, v.Make, v.Model, 
                                   ps.SectionName + ' - ' + s.SpaceNumber AS ParkingSpace,
                                   t.EntryTime, t.ExitTime,
                                   DATEDIFF(MINUTE, t.EntryTime, t.ExitTime) AS MinutesParked,
                                   p.Amount, p.PaymentMethod, p.PaymentTime, p.TransactionReference
                                   FROM ParkingTickets t
                                   JOIN Vehicles v ON t.VehicleId = v.VehicleId
                                   JOIN ParkingSpaces s ON t.SpaceId = s.SpaceId
                                   JOIN ParkingSections ps ON s.SectionId = ps.SectionId
                                   JOIN Payments p ON t.TicketId = p.TicketId
                                   WHERE t.TicketId = @TicketId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TicketId", ticketId);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        lblReceiptNo.Text = transactionRef;
                        lblTicketId.Text = reader["TicketId"].ToString();
                        lblLicensePlate.Text = reader["LicensePlate"].ToString();
                        lblMakeModel.Text = $"{reader["Make"]} {reader["Model"]}";
                        lblParkingSpace.Text = reader["ParkingSpace"].ToString();
                        lblEntryTime.Text = Convert.ToDateTime(reader["EntryTime"]).ToString("g");
                        lblExitTime.Text = Convert.ToDateTime(reader["ExitTime"]).ToString("g");

                        int minutesParked = Convert.ToInt32(reader["MinutesParked"]);
                        decimal hoursParked = Math.Round(minutesParked / 60.0m, 2);

                        lblDuration.Text = $"{hoursParked} hours";
                        lblAmountPaid.Text = $"{reader["Amount"]} ZMW";
                        lblPaymentMethod.Text = reader["PaymentMethod"].ToString();
                        lblPaymentTime.Text = Convert.ToDateTime(reader["PaymentTime"]).ToString("g");
                        lblTransactionRef.Text = reader["TransactionReference"].ToString();
                    }
                    reader.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading receipt details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

       

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            // yet to implement printing functionality

            MessageBox.Show("Receipt printed successfully!", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
    }

}
