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
    public partial class MemberBorrowingForm : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");
        private int selectMemBorrowingId = -1;
        public MemberBorrowingForm()
        {
            InitializeComponent();
            LoadMemBorrowings();
        }

        private void LoadMemBorrowings()
        {
            string query = "SELECT * FROM BOOK";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable borrowingTable = new DataTable();
            adapter.Fill(borrowingTable);
            dataGridView1.DataSource = borrowingTable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddBorrow();
        }

        private void AddBorrow()
        {
            string query = "INSERT INTO MemberBorrowing (BookID, MemberID, BorrowDate, DueDate, ReturnDate) VALUES (@BookID, @MemberID, @BorrowDate, @DueDate, @ReturnDate)";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@BookID", Convert.ToInt32(textBox1.Text));
            command.Parameters.AddWithValue("@MemberID", Convert.ToInt32(textBox4.Text));
            command.Parameters.AddWithValue("@BorrowDate", dateTimePicker1.Value);
            command.Parameters.AddWithValue("@DueDate", dateTimePicker2.Value);
            command.Parameters.AddWithValue("@ReturnDate", dateTimePicker3.Value);



            con.Open();
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Book added successfully.");
            }
            else
            {
                MessageBox.Show("Book not found.");
                LoadMemBorrowings();

            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditBorrow();
        }

        private void EditBorrow()
        {


            string query = "UPDATE MemberBorrowing SET BookID = @BookID, MemberID = @MemberID, BorrowDate = @BorrowDate, DueDate = @DueDate, ReturnDate = @ReturnDate WHERE BorrowingID = @BorrowingID";
            SqlCommand command = new SqlCommand(query, con);
            int MemBorrowingID = int.Parse(textBox2.Text);
            command.Parameters.AddWithValue("@BorrowingID", MemBorrowingID);
            command.Parameters.AddWithValue("@BookID", Convert.ToInt32(textBox1.Text));
            command.Parameters.AddWithValue("MemberID", Convert.ToInt32(textBox4.Text));
            command.Parameters.AddWithValue("@BorrowDate", dateTimePicker1.Value);
            command.Parameters.AddWithValue("@DueDate", dateTimePicker2.Value);
            command.Parameters.AddWithValue("@ReturnDate", dateTimePicker3.Value);

            con.Open();
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Borrowwing record updated successfully.");
            }
            else
            {
                MessageBox.Show("error.");
                LoadMemBorrowings();

            }





        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteBorrow();
        }

        private void DeleteBorrow()
        {
            int MemBorrowingID = int.Parse(textBox2.Text);
            {


                string query = "DELETE FROM MemberBorrowing WHERE BorrowingID = @BorrowingID";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@BorrowingID", MemBorrowingID);

                con.Open();
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Borrowed record deleted successfully.");
                }
                else
                {
                    MessageBox.Show("error.");
                    LoadMemBorrowings();

                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                selectMemBorrowingId = Convert.ToInt32(row.Cells["BookID"].Value);
                textBox1.Text = row.Cells["Title"].Value.ToString();
                textBox4.Text = row.Cells["Author"].Value.ToString();
                textBox3.Text = row.Cells["ISBN"].Value.ToString();
                textBox2.Text = row.Cells["Publisher"].Value.ToString();


            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox4.Clear();
            textBox3.Clear();
            textBox2.Clear();
        }
    }
}
