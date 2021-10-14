using AnyStore.BLL;
using AnyStore.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore.UI
{
    partial class frmBuscar : Form
    {
        public frmBuscar()
        {
            InitializeComponent();
        }
        private List<productsBLL> addedProducts = new List<productsBLL>();
        categoriesDAL cdal = new categoriesDAL();
        productsDAL pdal = new productsDAL();
        warehouseDAL wdal = new warehouseDAL();
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Addd Functionality to Close this form
            this.Hide();
        }

        private void frmBuscar_Load(object sender, EventArgs e)
        {
            //Display the CAtegories in Combobox
            DataTable cDt = cdal.Select();
            DataTable cDware = wdal.Select();

            cmbCategories.DataSource = cDt;

            //Give the Value member and display member for Combobox
            cmbCategories.DisplayMember = "Título";
            cmbCategories.ValueMember = "Título";



            cmbAlmacen.DataSource = cDware;

            //Give the Value member and display member for Combobox
            cmbAlmacen.DisplayMember = "Nombre";
            cmbAlmacen.ValueMember = "Nombre";

            //Display all the products in Datagrid view when the form is loaded
            DataTable pdt = pdal.Select();
            dgvProducts.DataSource = pdt;
        }

        private void cmbCategories_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Display all the Products Based on Selected CAtegory

            string category = cmbCategories.Text;
            string almacen = cmbAlmacen.Text ?? "";

            DataTable dt = pdal.DisplayProductsByCategory(category, almacen);
            dgvProducts.DataSource = dt;
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            //Display all the productswhen this button is clicked
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }

        private void cmbAlmacen_SelectedIndexChanged(object sender, EventArgs e)
        {
            string category = cmbCategories.Text ?? "";
            string almacen = cmbAlmacen.Text;

            DataTable dt = pdal.DisplayProductsByCategory(category, almacen);
            dgvProducts.DataSource = dt;
        }

        private void tbx_Buscar_TextChanged(object sender, EventArgs e)
        {
            DataTable dt = pdal.Search(tbx_Buscar.Text);
            dgvProducts.DataSource = dt;       
        }

        private void lblTop_Click(object sender, EventArgs e)
        {

        }

        private void Btn_Agregar_Click(object sender, EventArgs e)
        {

        }

        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var row = dgvProducts.Rows[e.RowIndex];

            addedProducts.Add(new productsBLL
            {
                id = int.Parse(row.Cells[0].Value.ToString()),
                name = row.Cells[1].Value.ToString(),
                Categoria = row.Cells[2].Value.ToString(),
                warehouse = row.Cells[3].Value.ToString(),
                description = row.Cells[4].Value.ToString(),
                rate = decimal.Parse(row.Cells[5].Value.ToString()),
                PriceMinimum = decimal.Parse(row.Cells[6].Value.ToString()),
                qty = decimal.Parse(row.Cells[7].Value.ToString()),
                Gain = decimal.Parse(row.Cells[8].Value.ToString())
            });


        }
    }
}
