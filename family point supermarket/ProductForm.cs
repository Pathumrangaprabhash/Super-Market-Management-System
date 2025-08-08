using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Guna.UI2.WinForms;

namespace family_point_supermarket
{
    public partial class ProductForm : Form
    {
        public ProductForm()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HI\OneDrive\Documents\fpsmdb.mdf;Integrated Security=True;Connect Timeout=30");
        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Make sure a valid row is selected
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = ProdDGV.Rows[e.RowIndex];

                ProdId.Text = row.Cells[0].Value.ToString();        // Product ID
                ProdName.Text = row.Cells[1].Value.ToString();      // Product Name
                ProdQty.Text = row.Cells[2].Value.ToString();       // Quantity
                ProdPrice.Text = row.Cells[3].Value.ToString();     // Price
                CatCb.SelectedValue = row.Cells[4].Value.ToString(); // Category
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
        private void fillcombo()
        {
            //This Method Will bind the Combobox with the Database
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CatName from CategoryTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CatName",  typeof(string));
            dt.Load(rdr);
            CatCb.ValueMember = "CatName";
            CatCb.DataSource = dt;
            Con.Close();
        }
        private void ProductForm_Load(object sender, EventArgs e)
        {
            fillcombo(); // Existing: CatCb
            fillSearchCategoryCombo(); // New: SearchCb
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CategoryForm cat = new CategoryForm();
            cat.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            SellerForm1 seller = new SellerForm1();
            seller.Show();
        }
            

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "insert into ProductTbl values(" + ProdId.Text + ",'" + ProdName.Text + "','" + ProdQty.Text + "','" + ProdPrice.Text + "','" + CatCb.SelectedValue.ToString() + "')";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Product Added Successfully");
                Con.Close();
                //populate();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                if (ProdId.Text == "" || ProdName.Text == "" || ProdQty.Text == "" || ProdPrice.Text == "")
                {
                    MessageBox.Show("Missing Information");
                }
                else
                {
                    Con.Open();
                    string query = "update ProductTbl set ProdName='" + ProdName.Text + "',ProdQty='" + ProdQty.Text + "',ProdPrice='" + ProdPrice.Text + "' where ProdId=" + ProdId.Text + ";";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Successfully Updated");
                    Con.Close();
                    //populate();
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
                if (ProdId.Text == "")
                {
                    MessageBox.Show("Select The Category To Delete");
                }
                else
                {
                    Con.Open();
                    string query = "delete from ProductTbl where ProdId=" + ProdId.Text + "";
                    SqlCommand cmd = new SqlCommand(query, Con);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Product Deleted Successfully");
                    Con.Close();
                    //populate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();

                Con.Open();
                string selectedCategory = SearchCb.SelectedValue.ToString();
                string query = "SELECT * FROM ProductTbl WHERE ProdCat = @Cat";
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                sda.SelectCommand.Parameters.AddWithValue("@Cat", selectedCategory);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                ProdDGV.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (Con.State == ConnectionState.Open)
                    Con.Close();
            }

        }
        
        
private void fillSearchCategoryCombo()
        {
            try
            {
                Con.Open();
                SqlCommand cmd = new SqlCommand("SELECT CatName FROM CategoryTbl", Con);
                SqlDataReader rdr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Columns.Add("CatName", typeof(string));
                dt.Load(rdr);
                SearchCb.ValueMember = "CatName";
                SearchCb.DataSource = dt;
                Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
     
        
        private void CatCb_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void RefreshBtn_Click(object sender, EventArgs e)
        {
            try
            {
                Con.Open();
                string query = "SELECT * FROM ProductTbl";
                SqlDataAdapter sda = new SqlDataAdapter(query, Con);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                ProdDGV.DataSource = dt;
                Con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }
    }
}
