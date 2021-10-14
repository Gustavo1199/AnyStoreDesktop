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
    public partial class DetailsProducts : Form
    {
        public DetailsProducts()
        {
            InitializeComponent();
        }
        DataTable dataTable = new DataTable();
        int transID;
        string tipoTran;
        int deaCustID;
        public DetailsProducts(DataTable dataTable, int transID, string tipoTran, string Rol, int deaCustId, decimal ITB, decimal DESCUENTO)
        {
            InitializeComponent();
            this.dataTable = dataTable;
            this.transID = transID;
            this.tipoTran = tipoTran;
            deaCustID = deaCustId;
            txtDiscount.Text = DESCUENTO.ToString();
            txtVat.Text = ITB.ToString();
            if(Rol.ToUpper() == "USER")
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                btnActualizaCabecera.Enabled = false;
            }
        }
        userDAL uDAL = new userDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();
        transactionDAL tDAL = new transactionDAL();
        productsDAL pDAL = new productsDAL();
        DeaCustDAL dcDAL = new DeaCustDAL();
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void DetailsProducts_Load(object sender, EventArgs e)
        {
            dgvTransactions.DataSource = dataTable;

            dgvTransactions.Columns["id"].Visible = false;
            dgvTransactions.Columns["product_id"].Visible = false;


            warehouseDAL wdal = new warehouseDAL();
            DataTable almcaenDT = wdal.Select();
            //Specify DataSource for Category ComboBox
            cmbWarehouse.DataSource = almcaenDT;
            //Specify Display Member and Value Member for Combobox
            cmbWarehouse.DisplayMember = "title";
            cmbWarehouse.ValueMember = "Nombre";
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void TxtQty_VisibleChanged(object sender, EventArgs e)
        {
           
        }

        private void TxtQty_TextChanged(object sender, EventArgs e)
        {
            if (ValidateNumber(TxtQty.Text))
                UpdateTotalPrice();
            else
                TxtQty.Text = "0";

        }

        private void UpdateTotalPrice()
        {
            double qty = 0;
            if (!string.IsNullOrWhiteSpace(TxtQty.Text))
                qty = double.Parse(TxtQty.Text);
            double rate = 0;
            if (!string.IsNullOrWhiteSpace(txtRate.Text))
                rate = double.Parse(txtRate.Text);

            txtTotalPrice.Text = (Convert.ToDouble(qty) * Convert.ToDouble(rate)).ToString();
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            if (ValidateNumber(txtRate.Text))
            {
                UpdateTotalPrice();
            }
                
            else
                txtRate.Text = "0";
            UpdateTotalPrice();
        }
        int RowIndex;
        private void dgvTransactions_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Finding the Row Index of the Row Clicked on Data Grid View
            RowIndex = e.RowIndex;
            txtProductID.Text = dgvTransactions.Rows[RowIndex].Cells[1].Value.ToString();
            txtProductName.Text = dgvTransactions.Rows[RowIndex].Cells[2].Value.ToString();
            TxtQty.Text = dgvTransactions.Rows[RowIndex].Cells[4].Value.ToString();
            txtRate.Text = dgvTransactions.Rows[RowIndex].Cells[3].Value.ToString();
            txtTotalPrice.Text = dgvTransactions.Rows[RowIndex].Cells[5].Value.ToString();
        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword from productsearch textbox
            string keyword = txtSearch.Text;

            //Check if we have value to txtSearchProduct or not
            if (keyword == "")
            {
                txtProductName.Text = "";
                txtRate.Text = "";
                TxtQty.Text = "";
                txtProductID.Text = "";
                return;
            }

            productsDAL pDAL = new productsDAL();
            //Search the product and display on respective textboxes
            productsBLL p = pDAL.GetProductsForTransaction(keyword, cmbWarehouse.Text);

            //Set the values on textboxes based on p object
            TxtQty.Text = "";
            txtProductID.Text = "";
            txtProductName.Text = p.name;
            txtRate.Text = p.rate.ToString();
            txtProductID.Text = p.id.ToString();
        }

        private void cmbWarehouse_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearch_TextChanged(sender, e);
        }

        private bool ValidateNumber(string value)
        {
            try
            {
                decimal.Parse(value);
                return true;
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(value))
                {
                    return true;
                }
                MessageBox.Show("Caracter inválido.");
                return false;
            }
        }

        private void TxtQty_Leave(object sender, EventArgs e)
        {
            UpdateTotalPrice();
        }

        private void txtRate_Leave(object sender, EventArgs e)
        {
            UpdateTotalPrice();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtProductID.Text))
            {
                MessageBox.Show("Vuelva a consultar el producto");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRate.Text))
            {
                MessageBox.Show("Debe introducir el precio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtQty.Text))
            {
                MessageBox.Show("Debe introducir la cantidad.");
                return;
            }
            string username = frmLogin.loggedIn;
            userBLL u = uDAL.GetIDFromUsername(username);
            transactionDetailBLL transactionDetail = new transactionDetailBLL();
            productsDAL pDAL = new productsDAL();
            //Get the Product name and convert it to id
            string ProductName = txtProductName.Text;
            productsBLL p = pDAL.GetProductIDFromName(ProductName, cmbWarehouse.Text);

            transactionDetail.transactionID = transID;
            transactionDetail.product_id = p.id;
            transactionDetail.rate = decimal.Parse(txtRate.Text);
            transactionDetail.qty = decimal.Parse(TxtQty.Text);
            transactionDetail.total = Math.Round(decimal.Parse(txtTotalPrice.Text));
            transactionDetail.added_date = DateTime.Now;
            transactionDetail.added_by = u.id;
            bool x = false;
            bool EsCompra = false;
            if (tipoTran == "Compra")
            {
                EsCompra = true;
                //Increase the Product
                x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
            }
            else if (tipoTran == "Venta")
            {
                //Decrease the Product Quntiyt
                x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
            }

            bool y = tdDAL.InsertTransactionDetail(transactionDetail);
            tDAL.UpdateTransactionTotal(transID, EsCompra);
            RefreshDT();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtProductID.Text) || dgvTransactions.Rows.Count == 1)
            {
                MessageBox.Show("Vuelva a seleccionar el producto");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRate.Text))
            {
                MessageBox.Show("Debe introducir el precio.");
                return;
            }
            if (string.IsNullOrWhiteSpace(TxtQty.Text))
            {
                MessageBox.Show("Debe introducir la cantidad.");
                return;
            }
            var id = Convert.ToInt32(dgvTransactions.Rows[RowIndex].Cells[0].Value.ToString());
            //id detalle, 

            //paso 1: detalle nuevo
            transactionDetailBLL transactionDetail = new transactionDetailBLL();
            transactionDetail.id = id;
            transactionDetail.rate = decimal.Parse(txtRate.Text);
            transactionDetail.qty = decimal.Parse(TxtQty.Text);
            transactionDetail.total = Math.Round(decimal.Parse(txtTotalPrice.Text));
            transactionDetail.added_date = DateTime.Now;

            //afectar el inventario
            //consultar el detalle, comparar valores de precio, cantidad
            //en base a ello, tomar la diferencia y sumarlo al inventario y actualizar grandTotal de la cabecera
            transactionDetailBLL dbVal = tdDAL.GetDetailByID(id);
            var QtyDiff = dbVal.qty - transactionDetail.qty;
            var Ttran = false;
            if (tipoTran == "Compra")
            {
                Ttran = true;
                QtyDiff = QtyDiff * -1;
            }
            productsBLL p = pDAL.GetProductIDFromName(txtProductName.Text, cmbWarehouse.Text);
            //afectar inventario
            pDAL.UpdateInventoryFromDetail(p.id, QtyDiff);
            //se manda a actualizar el detalle
            tdDAL.UpdateDetail(transactionDetail);
            
            tDAL.UpdateTransactionTotal(transID, Ttran);
            RefreshDT();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvTransactions.Rows.Count == 1 || string.IsNullOrEmpty(txtProductID.Text))
            {
                MessageBox.Show("Vuelva a seleccionar el producto");        
                return;
            }
            //id producto, cantidad, total, id detalle

            //paso 1: retornar al inventario
            var prodID = int.Parse(txtProductID.Text);
            decimal qty = 0;
            if(!string.IsNullOrWhiteSpace(TxtQty.Text))
                qty = decimal.Parse(TxtQty.Text);
            if (tipoTran == "Compra")
            {
                qty = qty * -1;
            }
            bool EsCompra = false;
            pDAL.UpdateInventoryFromDetail(prodID, qty);
            //paso 2: afectar la cabecera
            double price = 0;
            if (!string.IsNullOrWhiteSpace(txtTotalPrice.Text))
                price = double.Parse(txtTotalPrice.Text);
            if (tipoTran == "Compra")
            {
                EsCompra = true;
                price = price * -1;
            }
            //paso 3: borra el detalle
            var id = dgvTransactions.Rows[RowIndex].Cells[0].Value.ToString();
            var realID = Convert.ToInt32(id);
            tdDAL.DeleteDetail(realID);
            
            tDAL.UpdateTransactionTotal(transID, EsCompra);

            RefreshDT();
        }

        private void txtProductID_Click(object sender, EventArgs e)
        {

        }
        private void RefreshDT()
        {
            DataTable dt = new transactionDetailDAL().GetDetailsFromTransaction(transID.ToString());
            dgvTransactions.DataSource = dt;
            dgvTransactions.Columns["id"].Visible = false;
            dgvTransactions.Columns["product_id"].Visible = false;
        }

        private void txtSearchDeaCust_TextChanged(object sender, EventArgs e)
        {
            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                //Clear all the textboxes
                deaCustID = 0;
                txtName.Text = "";

                return;
            }

            //Write the code to get the details and set the value on text boxes
            if (tipoTran == "Venta")
            {
                DeaCustBLL dc = dcDAL.SearchCustomerForTransaction(keyword);
                //Now transfer or set the value from DeCustBLL to textboxes
                deaCustID = dc.id;
                txtName.Text = dc.name;
            }
            if (tipoTran == "Compra")
            {
                DeaCustBLL dc = dcDAL.SearchDealerForTransaction(keyword);

                //Now transfer or set the value from DeCustBLL to textboxes
                deaCustID = dc.id;
                txtName.Text = dc.name;


            }
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if (txtDiscount.Text != "")
            {
                if (!ValidateNumber(txtDiscount.Text))
                {
                    txtDiscount.Clear();
                }
            }
        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            if (!ValidateNumber(txtVat.Text))
            {
                txtVat.Clear();
            }

        }

        private void btnActualizaCabecera_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtDiscount.Text))
            {
                MessageBox.Show("Debe introducir el Descuento.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtVat.Text))
            {
                MessageBox.Show("Debe introducir el Impuesto.");
                return;
            }
            transactionsBLL tran = new transactionsBLL();
            tran.id = transID;
            tran.dea_cust_id = deaCustID;
            tran.discount = decimal.Parse(txtDiscount.Text);
            tran.tax = decimal.Parse(txtVat.Text);

            new transactionDAL().UpdateTransaction(tran);

            MessageBox.Show("Transacción actualizada satisfactoriamente.");
        }
    }
}
