using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace family_point_supermarket
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static string Sellername = "";
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HI\OneDrive\Documents\fpsmdb.mdf;Integrated Security=True;Connect Timeout=30");
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            UnameTb.Text = "";
            PassTb.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(UnameTb.Text =="" ||  PassTb.Text == "")
            {
                MessageBox.Show("Enter the UserName And Password");
            }
            else
            {
                if (RoleCb.SelectedIndex > -1)
                {
                    if (RoleCb.SelectedItem.ToString() == "ADMIN")
                    {
                        if (UnameTb.Text == "Admin" && PassTb.Text == "Admin")
                        {
                            ProductForm prod = new ProductForm();
                            prod.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("If You are The Admin, Enter The Correct ID and Password");
                        }
                    }
                    else
                    {
                        //MessageBox.Show("You In The Seller Selection");
                        Con.Open();
                        SqlDataAdapter sda = new SqlDataAdapter("Select count(8) from SellerTbl where SellerName='"+UnameTb.Text+"' and SellerPass='"+PassTb.Text+"'", Con);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        if (dt.Rows[0][0].ToString() == "1")
                        {
                            Sellername = UnameTb.Text; 
                            SellingForm sell = new SellingForm();
                            sell.Show();
                            this.Show();
                            Con.Close();
                        }
                        else
                        {
                            MessageBox.Show("Wrong User Name or Password");
                        }
                        Con.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Sellect a Role");
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
