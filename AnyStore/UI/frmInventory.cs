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
    partial class frmInventory : Form
    {
        public frmInventory()
        {
            InitializeComponent();
        }
        categoriesDAL cdal = new categoriesDAL();
        productsDAL pdal = new productsDAL();
        warehouseDAL wdal = new warehouseDAL();
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Addd Functionality to Close this form
            this.Hide();
        }

        private void frmInventory_Load(object sender, EventArgs e)
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
    }
}
