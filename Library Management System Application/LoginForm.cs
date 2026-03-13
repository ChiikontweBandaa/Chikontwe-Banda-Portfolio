using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;





namespace WindowsFormsApp2
{
    public partial class LoginForm : Form
    {
        SqlConnection con = new SqlConnection("Data Source=LAPTOP-ICPVAUK1\\SQLEXPRESS; Initial Catalog=InformationSystem;Integrated Security = True");
        public LoginForm()
        {
            InitializeComponent();
        }

        private void X_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtUsername_MouseEnter(object sender, EventArgs e)
        {

        
        }

        private void txtUsername_MouseClick(object sender, MouseEventArgs e)
        {
            if (txtUsername.Text == "Username")
            {
                txtUsername.Clear();
            }
        }

        private void txtPassword_MouseClick(object sender, MouseEventArgs e)
        {
            if(txtPassword.Text == "Password")
            {
                txtPassword.Clear();
                txtPassword.PasswordChar = '*';
            }
        }

        

       

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            //User user = AuthenticateUser(username, password);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your username and password","login failed",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            try
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Role FROM ENDUSER WHERE Username = @Username AND Password = @Password", con);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                object result = cmd.ExecuteScalar();

                if (result != null)
                {
                    string role = result.ToString();
                    MessageBox.Show("Login succesfull", "success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    String you_username;
                    if (role.Equals("Admin", StringComparison.OrdinalIgnoreCase))
                    {
                        you_username = username;
                        AdminDashboard adminDashboard = new AdminDashboard(you_username);
                        adminDashboard.Show();
                        this.Hide();

                    }
                    else if (role.Equals("Member", StringComparison.OrdinalIgnoreCase))
                    {
                        you_username = username;
                       
                        MemberDashboard memberDashboard = new MemberDashboard(you_username);
                        memberDashboard.Show();
                        this.Hide();



                    }

                }
                else
                {
                    MessageBox.Show("Invalid username or password", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally {

                con.Close();
            }
            
        }

        private User AuthenticateUser(string username, string password)
        {
            if(username == "admin" && password == "password123")
            {
                return new User(username, password, "Admin");
            }
            else
            {
                return null;
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
