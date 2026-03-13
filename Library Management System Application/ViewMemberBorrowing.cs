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
    public partial class ViewMemberBorrowing : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");
        public ViewMemberBorrowing()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadMemBorrowingData();

        }

        private void LoadMemBorrowingData()
        {
            try
            {
                con.Open();
                string query = "SELECT * FROM MemberBorrowing";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }

            catch (Exception ex)
            {
                MessageBox.Show("an arror occurred:" + ex.Message);
            }
        }
    }
}
