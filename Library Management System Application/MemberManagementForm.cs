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
    public partial class MemberManagementForm : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");
        private int selectMemberId = -1;

        public MemberManagementForm()
        {
            InitializeComponent();
            LoadMembers();
            
        }

        private void LoadMembers()
        {
            string query = "SELECT * FROM MEMBER";
            SqlDataAdapter adapter = new SqlDataAdapter(query, con);
            DataTable membersTable = new DataTable();
            adapter.Fill(membersTable);
            dataGridViewMEMBER.DataSource = membersTable;
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            AddMember();
        }

        private void AddMember()
        {
            string query = "INSERT INTO MEMBER (FirstName, LastName, Email, PhoneNumber, MembershipDate) VALUES ( @FirstName, @LastName, @Email, @PhoneNumber, @MembershipDate)";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@FirstName", textBox1.Text);
            command.Parameters.AddWithValue("@LastName", textBox4.Text);
            command.Parameters.AddWithValue("@Email", textBox3.Text);
            command.Parameters.AddWithValue("@PhoneNumber", textBox2.Text);
            command.Parameters.AddWithValue("@MembershipDate", dateTimePicker2.Value);
            


            con.Open();
            int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Member added successfully.");
            }
            else
            {
                MessageBox.Show("error");
                LoadMembers();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditMember();
           
            
        }

        private void EditMember()
        {
            

            string query = "UPDATE MEMBER SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNumber = @PhoneNumber, MembershipDate = @MembershipDate WHERE MemberID = @MemberID";
            SqlCommand command = new SqlCommand(query, con);
            int MemberID = int.Parse(textBox5.Text);
            command.Parameters.AddWithValue("@MemberID", MemberID);
            command.Parameters.AddWithValue("@FirstName", textBox1.Text);
            command.Parameters.AddWithValue("@LastName", textBox4.Text);
            command.Parameters.AddWithValue("@Email", textBox3.Text);
            command.Parameters.AddWithValue("@PhoneNumber", textBox2.Text);
            command.Parameters.AddWithValue("@MembershipDate", dateTimePicker2.Value);

            con.Open();
            
           int result = command.ExecuteNonQuery();
            if (result > 0)
            {
                MessageBox.Show("Member updated successfully.");
            }
            else
            {
                MessageBox.Show("Member not found.");
                LoadMembers();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DeleteMember();
            
 
               
        }

        private void DeleteMember()
        {


            int MemberID = int.Parse(textBox5.Text);

           
            {
                string query = "DELETE FROM MEMBER WHERE MemberID = @MemberID";
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@MemberID", MemberID);

                con.Open();
                int result = command.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Member deleted successfully.");
                }
                else
                {
                    MessageBox.Show("Member not found.");
                    LoadMembers();
                    
                }
            }



        }

        private void dataGridViewMEMBER_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridViewMEMBER.Rows[e.RowIndex];
                selectMemberId = Convert.ToInt32(row.Cells["MemberID"].Value);
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
            textBox2.Clear();
            textBox5.Clear();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
