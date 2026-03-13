using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Return_Book : Form
    {
        public Return_Book()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string bookId = textBox1.Text;
            string memberId = textBox4.Text;
            string email = textBox3.Text;
            DateTime borrowdate = dateTimePicker1.Value;
            DateTime duedate = dateTimePicker2.Value;
            DateTime returndate = dateTimePicker3.Value;

            ReturnBook(bookId, memberId, email, borrowdate, duedate, returndate);
        }

        private void ReturnBook(string bookId, string memberId, string email, DateTime borrowdate, DateTime duedate, DateTime returndate)
        {
            SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");

            string query = "INSERT INTO BOOKRETURN (BookID, MemberID, Email, BorrowDate, DueDate, ReturnDate) " +
                "VALUES (@BookID, @MemberID, @Email, @BorrowDate, @DueDate, @ReturnDate)";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@BookID", bookId);
            command.Parameters.AddWithValue("@MemberID", memberId);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@BorrowDate", borrowdate);
            command.Parameters.AddWithValue("@DueDate", duedate);
            command.Parameters.AddWithValue("@ReturnDate", returndate);

            try
            {
                con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                if (rowsAffected > 0)
                {
                    MessageBox.Show("Book returned successfully");
                }

                else
                {
                    MessageBox.Show("Error returning the book");
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("An error ocurred: " + ex.Message);
            }
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
