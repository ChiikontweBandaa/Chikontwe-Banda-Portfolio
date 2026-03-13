using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class AdminDashboard : Form

    {
        public AdminDashboard(String username)
        {
            InitializeComponent();
            adminUsernameLbl.Text = username;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("are you sure you want to exit?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                Application.Exit();

            }
            
        }

        private void addNewBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BookManagementForm ab = new BookManagementForm();
            ab.ShowDialog();
        }

        private void addStudentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberManagementForm mm = new MemberManagementForm();
            mm.ShowDialog();
        }

        private void issueBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void studentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void viewBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewBookForm vbf = new ViewBookForm();
            vbf.ShowDialog();
        }

        private void toolStripDropDownButton1_Click(object sender, EventArgs e)
        {
            LoginForm LF = new LoginForm();
            LF.ShowDialog();
        }

        private void viewBorrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BorrowingManagementForm bmf = new BorrowingManagementForm();
            bmf.ShowDialog();
            
        }

        private void viewBorrowingToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewBorrowingForm viewBorrowingForm = new ViewBorrowingForm();
            viewBorrowingForm.ShowDialog();
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
        {

        }
    }
}
