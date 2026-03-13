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
    public partial class BorrowingManagementForm : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");
        private int selectedBorrowingId = -1;
        
        public BorrowingManagementForm()
        {
            InitializeComponent();
            LoadBorrowings();
            LoadUsers();
            
            
        }

        private void LoadBorrowings()
        {
            string query = "SELECT * FROM BOOK";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable borrowingTable = new DataTable();
            adapter.Fill(borrowingTable);
            dataGridViewBORROWING.DataSource = borrowingTable;
        }

        private void LoadUsers()
        {
            string query = "SELECT * FROM MEMBER";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable userTable = new DataTable();
            adapter.Fill(userTable);
            dataGridViewUSER.DataSource = userTable;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            AddBorrowing();
        }

        private void AddBorrowing()
        {
            string query = "INSERT INTO BORROWING (BookID, MemberID, BorrowDate, DueDate, ReturnDate) VALUES (@BookID, @MemberID, @BorrowDate, @DueDate, @ReturnDate)";
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
                LoadBorrowings();
                
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditBorrowing();
        }

        private void EditBorrowing()
        {
            

            string query = "UPDATE BORROWING SET BookID = @BookID, MemberID = @MemberID, BorrowDate = @BorrowDate, DueDate = @DueDate, ReturnDate = @ReturnDate WHERE BorrowingID = @BorrowingID";
            SqlCommand command = new SqlCommand(query, con);
            int BorrowingID = int.Parse(textBox2.Text);
            command.Parameters.AddWithValue("@BorrowingID", BorrowingID);
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
                LoadBorrowings();

            }





        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteBorrowing();
        }

        private void DeleteBorrowing()
        {
            int BorrowingID = int.Parse(textBox2.Text);
                {


                string query = "DELETE FROM BORROWING WHERE BorrowingID = @BorrowingID";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@BorrowingID", BorrowingID);

                con.Open();
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Borrowed record deleted successfully.");
                }
                else
                {
                    MessageBox.Show("error.");
                    LoadBorrowings();

                }
            }
        }

        private void dataGridViewBORROWING_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBORROWING.Rows[e.RowIndex];
                selectedBorrowingId = Convert.ToInt32(row.Cells["BookID"].Value);
                textBox1.Text = row.Cells["Title"].Value.ToString();
                textBox4.Text = row.Cells["Author"].Value.ToString();
                textBox3.Text = row.Cells["ISBN"].Value.ToString();
                textBox2.Text = row.Cells["Publisher"].Value.ToString();
                

            }
        }

        private void dataGridViewUSER_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewUSER.Rows[e.RowIndex];
                selectedBorrowingId = Convert.ToInt32(row.Cells["MemberID"].Value);
                textBox1.Text = row.Cells["FirstName"].Value.ToString();
                textBox4.Text = row.Cells["LastName"].Value.ToString();
                textBox3.Text = row.Cells["Email"].Value.ToString();
                textBox2.Text = row.Cells["PhoneNumber"].Value.ToString();

            }
        }



        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox4.Clear();
            textBox3.Clear();
            textBox3.Clear();
           
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
