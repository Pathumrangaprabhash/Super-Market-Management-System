using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace family_point_supermarket
{
    public partial class SellingForm : Form
    {
        public SellingForm()
        {
            InitializeComponent();
        }
        SqlConnection Con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HI\OneDrive\Documents\fpsmdb.mdf;Integrated Security=True;Connect Timeout=30");
        private void populate()
        {
            Con.Open();
            string query = " select ProdName,ProdQty from ProductTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProdDGV1.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void populatebills()
        {
            Con.Open();
            string query = " select * from BillTbl";
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            BillsDGV.DataSource = ds.Tables[0];
            Con.Close();
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void SellingForm_Load(object sender, EventArgs e)
        {
            populate();
            populatebills();
            fillcombo();
            SellerNamelbl.Text = Form1.Sellername;
        }
       
        private void ProdDGV1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = ProdDGV1.Rows[e.RowIndex];

                ProdName.Text = row.Cells[0].Value.ToString(); // Check column index
                ProdPrice.Text = row.Cells[1].Value.ToString(); // Check column index
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Datelbl.Text = DateTime.Today.Day.ToString() + "/" + DateTime.Today.Month.ToString() + "/" + DateTime.Today.Year.ToString();
        }
        int Grdtotal = 0, n = 0;
        private void ProdDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProdDGV1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product from the list.");
                return;
            }
        }

  

        private void button1_Click(object sender, EventArgs e)
        {
            if (ProdName.Text == "" || ProdQty.Text == "")
            {
                MessageBox.Show("Missing Data");
                return;
            }

            int quantity;
            if (!int.TryParse(ProdQty.Text, out quantity) || quantity <= 0)
            {
                MessageBox.Show("Invalid Quantity");
                return;
            }

            int total = Convert.ToInt32(ProdPrice.Text) * quantity;

            DataGridViewRow newRow = new DataGridViewRow();
            newRow.CreateCells(ODDERDGV);
            newRow.Cells[0].Value = n + 1;
            newRow.Cells[1].Value = ProdName.Text;
            newRow.Cells[2].Value = ProdPrice.Text;
            newRow.Cells[3].Value = ProdQty.Text;
            newRow.Cells[4].Value = total;
            ODDERDGV.Rows.Add(newRow);
            n++;
            Grdtotal += total;
            Amtlbl.Text = "" + Grdtotal;

            // 🟨 Reduce product quantity in ProductTbl
            try
            {
                Con.Open();
                string updateQuery = "UPDATE ProductTbl SET ProdQty = ProdQty - @soldQty WHERE ProdName = @prodName";
                SqlCommand cmd = new SqlCommand(updateQuery, Con);
                cmd.Parameters.AddWithValue("@soldQty", quantity);
                cmd.Parameters.AddWithValue("@prodName", ProdName.Text);
                cmd.ExecuteNonQuery();
                Con.Close();
                populate(); // Refresh products list
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error reducing stock: " + ex.Message);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (BillId.Text == "")
            {
                MessageBox.Show("Missing Bill Id");
                return;
            }

            if (ODDERDGV.Rows.Count == 0)
            {
                MessageBox.Show("No items in bill to save.");
                return;
            }

            try
            {
                Con.Open();
                string query = "insert into BillTbl values(@BillID, @SellerName, @Date, @Amount)";
                SqlCommand cmd = new SqlCommand(query, Con);
                cmd.Parameters.AddWithValue("@BillID", BillId.Text);
                cmd.Parameters.AddWithValue("@SellerName", SellerNamelbl.Text);
                cmd.Parameters.AddWithValue("@Date", Datelbl.Text);
                cmd.Parameters.AddWithValue("@Amount", Amtlbl.Text);
                cmd.ExecuteNonQuery();
                MessageBox.Show("Order Added Successfully");
                Con.Close();
                populatebills();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            e.Graphics.DrawString("FAMILYPOINTSUPERMARKET", new Font("Century Gothic", 25, FontStyle.Bold), Brushes.Red, new Point(230));
            e.Graphics.DrawString("Bill ID:"+BillsDGV.SelectedRows[0].Cells[0].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100,70));
            e.Graphics.DrawString("Seller Name:" + BillsDGV.SelectedRows[0].Cells[1].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 100));
            e.Graphics.DrawString("Date:" + BillsDGV.SelectedRows[0].Cells[2].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 130));
            e.Graphics.DrawString("Total Amount:" + BillsDGV.SelectedRows[0].Cells[3].Value.ToString(), new Font("Century Gothic", 20, FontStyle.Bold), Brushes.Blue, new Point(100, 160));
            e.Graphics.DrawString("S.M.P.R.PRABASH", new Font("Century Gothic", 20, FontStyle.Italic), Brushes.Red, new Point(270,230));
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if(printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            populate();
        }



        private void CatCb_SelectedIndexChanged(object sender, EventArgs e)
        {
            Con.Open();
            string query = "select ProdName,ProdQty from ProductTbl where ProdCat='" + SearchCb.SelectedValue.ToString();
            SqlDataAdapter sda = new SqlDataAdapter(query, Con);
            SqlCommandBuilder builder = new SqlCommandBuilder(sda);
            var ds = new DataSet();
            sda.Fill(ds);
            ProdDGV1.DataSource = ds.Tables[0];
            Con.Close();
        }
        
private void fillcombo()
        {
            //This Method Will bind the Combobox with the Database
            Con.Open();
            SqlCommand cmd = new SqlCommand("Select CatName from CategoryTbl", Con);
            SqlDataReader rdr;
            rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Columns.Add("CatName", typeof(string));
            dt.Load(rdr);
            SearchCb.ValueMember = "CatName";
            SearchCb.DataSource = dt;
            Con.Close();
        }



        private void ProdPrice_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void SearchCb_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();
        }

        private void ODDERDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void SellerNamelbl_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}


