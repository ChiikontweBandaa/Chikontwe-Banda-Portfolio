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
    public partial class MemberDashboard : Form
    {
        public MemberDashboard(String username)
        {
            InitializeComponent();
            label1.Text = username;
        }

        private void addNewBookToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MemberDashboard_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void booksToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void returnBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Return_Book return_Book = new Return_Book();
            return_Book.ShowDialog();

        }

        private void issueBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
          
        }

        private void viewBookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemViewBookForm viewBookForm = new MemViewBookForm();
            viewBookForm.ShowDialog();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void viewBorrowingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewMemberBorrowing vmb = new ViewMemberBorrowing();
            vmb.ShowDialog();
        }

        private void borrowABookToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MemberBorrowingForm bbmf = new MemberBorrowingForm();
            bbmf.ShowDialog();
        }
    }
}
