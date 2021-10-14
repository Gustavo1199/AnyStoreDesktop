using AnyStore.DAL;
using AnyStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore
{
    public partial class frmUserDashboard : Form
    {
        public frmUserDashboard()
        {
            InitializeComponent();
        }
        productsDAL pdal = new productsDAL();

        //Set a public static method to specify whether the form is purchase or sales
        public static string transactionType;
        private void frmUserDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void frmUserDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;

            DataTable pdt = pdal.ProductsAlmostOver();
            dgvProducts.DataSource = pdt;
        }

        private void dealerAndCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust DeaCust = new frmDeaCust();
            DeaCust.Show();
        }

        private void purchaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //set value on transactionType static method
            transactionType = "Compra";
            frmPurchaseAndSales purchase = new frmPurchaseAndSales();
            purchase.Show();

        }

        private void salesFormsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Set the value to transacionType method to sales
            transactionType = "Venta";
            frmPurchaseAndSales sales = new frmPurchaseAndSales();
            sales.Show();

        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInventory inventory = new frmInventory();
            inventory.Show();
        }

        private void transaccionesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTransactions transaction = new frmTransactions("USER");
            transaction.Show();
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DataTable pdt = pdal.ProductsAlmostOver();
            dgvProducts.DataSource = pdt;
        }
    }
}
