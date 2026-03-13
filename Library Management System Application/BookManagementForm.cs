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
    public partial class BookManagementForm : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");
        private int selectBookId = -1;
        public BookManagementForm()
        {
            InitializeComponent();
            LoadBooks();
        }

        private void LoadBooks()
        {
            string query = "SELECT * FROM BOOK";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable bookTable = new DataTable();
            adapter.Fill(bookTable);
            dataGridViewBOOK.DataSource = bookTable;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddBook();
        }

        private void AddBook()
        {
            string query = "INSERT INTO BOOK (Title, Author, ISBN, Publisher, Year, CopiesAvailable) VALUES (@Title, @Author, @ISBN, @Publisher, @Year, @CopiesAvailable)";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Title", textBox1.Text);
            command.Parameters.AddWithValue("@Author", textBox4.Text);
            command.Parameters.AddWithValue("@ISBN", textBox3.Text);
            command.Parameters.AddWithValue("@Publisher", textBox2.Text);
            command.Parameters.AddWithValue("@Year", dateTimePicker1.Value);
            command.Parameters.AddWithValue("@CopiesAvailable", textBox5.Text);


            con.Open();
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Book added successfully.");
            }
            else
            {
                MessageBox.Show("error");
                LoadBooks();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditBook();
        }

        private void EditBook()
        {


            string query = "UPDATE BOOK SET Title = @Title, Author = @Author, ISBN = @ISBN, Publisher = @Publisher, Year = @Year, CopiesAvailable = @CopiesAvailable WHERE BookID = @BookID";
            SqlCommand command = new SqlCommand(query, con);
            int BookID = int.Parse(textBox6.Text);
            command.Parameters.AddWithValue("@BookID", BookID);
            command.Parameters.AddWithValue("@Title", textBox1.Text);
            command.Parameters.AddWithValue("@Author", textBox4.Text);
            command.Parameters.AddWithValue("@ISBN", textBox3.Text);
            command.Parameters.AddWithValue("@Publisher", textBox2.Text);
            command.Parameters.AddWithValue("@Year", dateTimePicker1.Value);
            command.Parameters.AddWithValue("@CopiesAvailable", Convert.ToInt32(textBox5.Text));


            con.Open();
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Book updated successfully.");
            }
            else
            {
                MessageBox.Show("Book not found.");
                LoadBooks();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteBook();
        }

        private void DeleteBook()
        {

            int BookID = int.Parse(textBox6.Text);


            {
                string query = "DELETE FROM BOOK WHERE BookID = @BookID";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@BookID", BookID);

                con.Open();
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Book deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Book not found.");
                    LoadBooks();
                }
            }




        }

        private void dataGridViewBOOK_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewBOOK.Rows[e.RowIndex];
                selectBookId = Convert.ToInt32(row.Cells["BookID"].Value);
                textBox1.Text = row.Cells["Title"].Value.ToString();
                textBox4.Text = row.Cells["Author"].Value.ToString();
                textBox3.Text = row.Cells["ISBN"].Value.ToString();
                textBox2.Text = row.Cells["Publisher"].Value.ToString();
                textBox5.Text = row.Cells["CopiesAvailable"].Value.ToString();

            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    } 
}