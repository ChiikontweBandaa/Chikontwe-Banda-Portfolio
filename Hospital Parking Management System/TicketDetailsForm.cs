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
    public partial class TicketDetailsForm : Form
    {
        private string connectionString = "Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS;Initial Catalog=HospitalParkingDB;Integrated Security=True";
        private int ticketId;
        private decimal chargeRate = 5.0m; // 5 kwacha per hour


        public TicketDetailsForm(int ticketId)
        {
            InitializeComponent();
            this.ticketId = ticketId;
            LoadTicketDetails();

        }



        private void LoadTicketDetails()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string query = @"SELECT t.TicketId, v.LicensePlate, v.Make, v.Model, v.Color, v.OwnerName,
                                   ps.SectionName, s.SpaceNumber, t.EntryTime, 
                                   DATEDIFF(MINUTE, t.EntryTime, GETDATE()) AS MinutesParked
                                   FROM ParkingTickets t
                                   JOIN Vehicles v ON t.VehicleId = v.VehicleId
                                   JOIN ParkingSpaces s ON t.SpaceId = s.SpaceId
                                   JOIN ParkingSections ps ON s.SectionId = ps.SectionId
                                   WHERE t.TicketId = @TicketId";

                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TicketId", ticketId);

                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        lblTicketId.Text = reader["TicketId"].ToString();
                        lblLicensePlate.Text = reader["LicensePlate"].ToString();
                        lblMakeModel.Text = $"{reader["Make"]} {reader["Model"]}";
                        lblColor.Text = reader["Color"].ToString();
                        lblOwner.Text = reader["OwnerName"].ToString();
                        lblParkingSpace.Text = $"{reader["SectionName"]} - {reader["SpaceNumber"]}";
                        lblEntryTime.Text = Convert.ToDateTime(reader["EntryTime"]).ToString("g");

                        int minutesParked = Convert.ToInt32(reader["MinutesParked"]);
                        decimal hoursParked = Math.Round(minutesParked / 60.0m, 2);
                        decimal currentCharge = Math.Round(hoursParked * chargeRate, 2);

                        
                        
                    }
                    reader.Close();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Error loading ticket details: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       

       

        private void btnPrint_Click_1(object sender, EventArgs e)
        {
            MessageBox.Show("Ticket printed successfully!", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}

          

       

        
    


