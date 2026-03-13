using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalParkingSystem
{
    public partial class DashboardForm : Form
    {
        private string userRole;
        public DashboardForm(string role)
        {
            InitializeComponent();
            userRole = role;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {

        }

        private void btnSections_Click(object sender, EventArgs e)
        {
            ParkingSectionsForm sectionsForm = new ParkingSectionsForm();
            sectionsForm.ShowDialog();
        }

        private void btnRegisterVehicle_Click(object sender, EventArgs e)
        {
            RegisterVehicleForm vehicleForm = new RegisterVehicleForm();
            vehicleForm.ShowDialog();
        }

        private void btnIssueTicket_Click(object sender, EventArgs e)
        {
            IssueTicketForm ticketForm = new IssueTicketForm();
            ticketForm.ShowDialog();
        }

        private void btnProcessPayment_Click(object sender, EventArgs e)
        {
           

            PaymentForm paymentForm = new PaymentForm();
            paymentForm.ShowDialog();
        }

        private void btnReservations_Click(object sender, EventArgs e)
        {
            ReservationsForm reservationsForm = new ReservationsForm();
            reservationsForm.ShowDialog();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure you want to log out?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Hide();
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            VehicleSearchForm vehicleSearchForm = new VehicleSearchForm();
            vehicleSearchForm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
