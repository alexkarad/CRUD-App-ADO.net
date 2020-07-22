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
using System.Configuration;

namespace ADO.net_Training
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public void clearTB()
        {
            firstNameTB.Text = "";
            lastNameTB.Text = "";
            majorCB.Text = "";
        }
        public void findRow(String firstName, String lastName)
        {
            String connection = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Student WHERE FirstName like @FirstName OR LastName like @LastName", con);
                cmd.Parameters.AddWithValue("@FirstName", firstNameTB.Text);
                cmd.Parameters.AddWithValue("@LastName", lastNameTB.Text);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                if (read.Read() && (!String.IsNullOrEmpty(lastNameTB.Text) && !String.IsNullOrEmpty(firstNameTB.Text)))
                {
                    majorCB.Text = read.GetValue(3).ToString();
                } else if (String.IsNullOrEmpty(lastNameTB.Text) || String.IsNullOrEmpty(firstNameTB.Text))
                {
                    MessageBox.Show("Please fill both a first and last name.");
                } else
                {
                    MessageBox.Show("Nothing was found.");
                }
            }
        }

        public bool isFound(String firstName, String lastName)
        {
            bool isFound = false;
            String connection = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Student WHERE FirstName = @FirstName AND LastName = @LastName", con);
                cmd.Parameters.AddWithValue("@FirstName", firstNameTB.Text);
                cmd.Parameters.AddWithValue("@LastName", lastNameTB.Text);
                con.Open();
                SqlDataReader read = cmd.ExecuteReader();
                if(read.Read())
                {
                    isFound = true;
                }
            }
            return isFound;
        }

        public void deleteRow(String firstName, String lastName)
        {
            String connection = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                if (!String.IsNullOrEmpty(firstNameTB.Text) && !String.IsNullOrEmpty(lastNameTB.Text))
                {
                    SqlCommand cmd = new SqlCommand("DELETE FROM dbo.Student WHERE FirstName = @FirstName AND LastName = @LastName", con);
                    cmd.Parameters.AddWithValue("@FirstName", firstNameTB.Text);
                    cmd.Parameters.AddWithValue("@LastName", lastNameTB.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void addRow(String firstName, String lastName)
        {
            //For dropdown menu. if nothing is selected, index is -1. If something is selected, index > -1. majorCB.SelectedIndex > -1
            String connection = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                if (!String.IsNullOrEmpty(lastNameTB.Text) && !String.IsNullOrEmpty(firstNameTB.Text))
                {
                    SqlCommand cmd = new SqlCommand("INSERT INTO dbo.Student (FirstName, LastName, Major)" +
                                                    "VALUES" +
                                                    "(@FirstName, @LastName, @Major)", con);
                    cmd.Parameters.AddWithValue("@FirstName", firstNameTB.Text);
                    cmd.Parameters.AddWithValue("@LastName", lastNameTB.Text);
                    cmd.Parameters.AddWithValue("@Major", majorCB.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                } else
                {
                    MessageBox.Show("You must enter a first and last name in order to add to the database.");
                }
            }
        }

        public void updateMajor(String firstName, String lastName)
        {
            String connection = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                if (!String.IsNullOrEmpty(lastNameTB.Text) && !String.IsNullOrEmpty(firstNameTB.Text))
                {
                    SqlCommand cmd = new SqlCommand("UPDATE dbo.Student SET Major = @Major WHERE FirstName = @FirstName AND LastName = @LastName", con);
                    cmd.Parameters.AddWithValue("@Major", majorCB.Text);
                    cmd.Parameters.AddWithValue("@FirstName", firstNameTB.Text);
                    cmd.Parameters.AddWithValue("@LastName", lastNameTB.Text);
                    con.Open();
                    cmd.ExecuteNonQuery();
                } else
                {
                    MessageBox.Show("You must enter a first and last name in order to update someone's major.");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            String connection = ConfigurationManager.ConnectionStrings["MyConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connection))
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM dbo.Student;", con);
                con.Open();
                cmd.ExecuteReader();
            }

        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void findButton_Click(object sender, EventArgs e)
        {
            if (isFound(firstNameTB.Text, lastNameTB.Text))
            {
                findRow(firstNameTB.Text, lastNameTB.Text);
            } else
            {
                MessageBox.Show("Person not found in the database!");
            }
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            if (!isFound(firstNameTB.Text, lastNameTB.Text))
            {
                addRow(firstNameTB.Text, lastNameTB.Text);
                clearTB();
            } else
            {
                MessageBox.Show("This person is already in the database!");
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if(!isFound(firstNameTB.Text, lastNameTB.Text))
            {
                MessageBox.Show("Person not found or invalid input. Please make sure the first and last name boxes match with the element you want " +
                    "to delete.");
                
            } else
            {
                deleteRow(firstNameTB.Text, lastNameTB.Text);
                clearTB();
            }
        }

        private void updateButton_Click(object sender, EventArgs e)
        {
            if(isFound(firstNameTB.Text, lastNameTB.Text))
            {
                updateMajor(firstNameTB.Text, lastNameTB.Text);
                clearTB();
            }
            else
            {
                MessageBox.Show("Person not found.");
            }
        }
    }
}
