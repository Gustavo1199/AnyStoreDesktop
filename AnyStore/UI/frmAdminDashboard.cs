using AnyStore.DAL;
using AnyStore.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnyStore
{
    public partial class frmAdminDashboard : Form
    {
        public frmAdminDashboard()
        {
            InitializeComponent();
        }
        productsDAL pdal = new productsDAL();
        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUsers user = new frmUsers();
            user.Show();
        }

        private void frmAdminDashboard_FormClosed(object sender, FormClosedEventArgs e)
        {
            frmLogin login = new frmLogin();
            login.Show();
            this.Hide();
        }

        private void frmAdminDashboard_Load(object sender, EventArgs e)
        {
            lblLoggedInUser.Text = frmLogin.loggedIn;

            DataTable pdt = pdal.ProductsAlmostOver();
            dgvProducts.DataSource = pdt;
            textBox1.Text = ConfigurationManager.AppSettings["Telefono"].ToString();
        }

        private void categoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCategories category = new frmCategories();
            category.Show();
        }

        private void productsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmProducts product = new frmProducts();
            product.Show();
        }

        private void deealerAndCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmDeaCust DeaCust = new frmDeaCust();
            DeaCust.Show();
        }

        private void transactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTransactions transaction = new frmTransactions("ADMIN");
            transaction.Show();
        }

        private void inventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInventory inventory = new frmInventory();
            inventory.Show();
        }

        private void bodegaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWarehouse inventory = new frmWarehouse();
            inventory.Show();
        }

        private void almacénToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmWarehouse warehouse = new frmWarehouse();
            warehouse.Show();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            DataTable pdt = pdal.ProductsAlmostOver();
            dgvProducts.DataSource = pdt;
        }
        private static void SetSetting(string key, string value)
        {
            Configuration configuration =
            ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            configuration.AppSettings.Settings[key].Value = value;
            configuration.Save(ConfigurationSaveMode.Full, true);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            SetSetting("Telefono", textBox1.Text);
        }
    }
}
