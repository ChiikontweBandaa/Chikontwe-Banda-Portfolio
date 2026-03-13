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
    public partial class MemViewBookForm : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");
        public MemViewBookForm()
        {
            InitializeComponent();
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {
            LoadBookData();
        }

        private void LoadBookData()
        {
            try
            {
                con.Open();
                string query = "SELECT * FROM BOOK";
                SqlDataAdapter adapter = new SqlDataAdapter(query, con);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dgvData.DataSource = dataTable;
            }

            catch (Exception ex)
            {
                MessageBox.Show("an arror occurred:" + ex.Message);
            }
        }
    }
}
