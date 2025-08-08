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
using System.Windows.Forms.VisualStyles;

namespace family_point_supermarket
{
    public partial class SellerForm1 : Form
    {
        public SellerForm1()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HI\OneDrive\Documents\fpsmdb.mdf;Integrated Security=True;Connect Timeout=30");
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "insert into SellerTbl values(" + SellerId.Text + ",'" + SellerName.Text + "','" + SellerAge.Text + "','" + SellerPhone.Text + "','" + SellerPass.Text + "')";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Seller Added Successfully");
                Con.Close();
                //populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SellerForm1_Load(object sender, EventArgs e)
        {
            populate();
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            SellerId.Text = SellerDGV.SelectedRows[0].Cells[0].Value.ToString();
            SellerName.Text = SellerDGV.SelectedRows[0].Cells[1].Value.ToString();
            SellerAge.Text = SellerDGV.SelectedRows[0].Cells[2].Value.ToString();
            SellerPhone.Text = SellerDGV.SelectedRows[0].Cells[3].Value.ToString();
            SellerPass.Text = SellerDGV.SelectedRows[0].Cells[4].Value.ToString();
        }
        private void populate()
        {
            Con.Open();
            string query = "select * from SellerTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            SellerDGV.DataSource = ds.Tables[0];
            ;
            Con.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (SellerId.Text == "" || SellerName.Text == "" || SellerAge.Text == "" || SellerPhone.Text == "" || SellerPass.Text == "")
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    Con.Open();
                    string query = "update SellerTbl set SellerName='" + SellerName.Text + "',SellerAge='" + SellerAge.Text + "',SellerPhone='" + SellerPhone.Text +"',SellerPass='" + SellerPass.Text +"' where SellerId=" + SellerId.Text + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Seller Successfully Updated");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                if (SellerId.Text == "")
                {
                    MessageBox.Show("Select The Seller To Delete");
                }
                else
                {
                    Con.Open();
                    string query = "delete from SellerTbl where SellerId=" + SellerId.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Seller Deleted Successfully");
                    Con.Close();
                    populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ProductForm prod = new ProductForm();
            prod.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CategoryForm cat = new CategoryForm();
            cat.Show();
            this.Hide();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    }
}
