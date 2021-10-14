using AnyStore.BLL;
using AnyStore.DAL;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Windows.Forms;

namespace AnyStore.UI
{
    public partial class frmPurchaseAndSales : Form
    {
        private bool FORCEPRINT = false;
        public frmPurchaseAndSales()
        {
            InitializeComponent();

        }
        private productsBLL Producto = new productsBLL();
        int? itemIndex;

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        DeaCustDAL dcDAL = new DeaCustDAL();
        productsDAL pDAL = new productsDAL();
        userDAL uDAL = new userDAL();
        transactionDAL tDAL = new transactionDAL();
        transactionDetailDAL tdDAL = new transactionDetailDAL();
        warehouseDAL wdal = new warehouseDAL();

        DataTable transactionDT = new DataTable();
        private void frmPurchaseAndSales_Load(object sender, EventArgs e)
        {
            //Get the transactionType value from frmUserDashboard
            string type = frmUserDashboard.transactionType;
            //Set the value on lblTop
            lblTop.Text = type;
            if (type == "Venta")
            {
                lblDeaCustTitle.Text = "Detalle de Cliente";
            }
            else
            {
                btnSave.Text = "Registrar Compra";
                lblDeaCustTitle.Text = "Detalle de Proveedor";
            }
            //Specify Columns for our TransactionDataTable
            transactionDT.Columns.Add("Nombre Producto");
            transactionDT.Columns.Add("Precio");
            transactionDT.Columns.Add("Cantidad");
            transactionDT.Columns.Add("Total");
            transactionDT.Columns.Add("Almacén");

            DataTable almcaenDT = wdal.Select();
            //Specify DataSource for Category ComboBox
            comboBox1.DataSource = almcaenDT;
            //Specify Display Member and Value Member for Combobox
            comboBox1.DisplayMember = "title";
            comboBox1.ValueMember = "Nombre";
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the keyword fro the text box

            string keyword = txtSearch.Text;

            if (keyword == "")
            {
                //Clear all the textboxes
                txtName.Text = "";
                txtEmail.Text = "";
                txtContact.Text = "";
                txtAddress.Text = "";
                return;
            }

            //Write the code to get the details and set the value on text boxes
            if (lblTop.Text == "Venta")
            {
                DeaCustBLL dc = dcDAL.SearchCustomerForTransaction(keyword);

                //Now transfer or set the value from DeCustBLL to textboxes
                txtName.Text = dc.name;
                txtEmail.Text = dc.email;
                txtContact.Text = dc.contact;
                txtAddress.Text = dc.address;
            }
            if (lblTop.Text == "Compra")
            {
                DeaCustBLL dc = dcDAL.SearchDealerForTransaction(keyword);

                //Now transfer or set the value from DeCustBLL to textboxes
                txtName.Text = dc.name;
                txtEmail.Text = dc.email;
                txtContact.Text = dc.contact;
                txtAddress.Text = dc.address;

            }

        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(comboBox1.Text))
            {
                MessageBox.Show("Especifique en el almacén que se buscará.");
            }
            //Get the keyword from productsearch textbox
            string keyword = txtSearchProduct.Text;

            //Check if we have value to txtSearchProduct or not
            if (keyword == "")
            {
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                TxtQty.Text = "";
                tbxGanancia.Text = "";
                return;
            }

            //Search the product and display on respective textboxes
            productsBLL p = pDAL.GetProductsForTransaction(keyword, comboBox1.Text);

            
            //Set the values on textboxes based on p object
            Producto = p;
            txtProductName.Text = p.name;
            txtInventory.Text = p.qty.ToString();
            txtRate.Text = (p.rate + (p.rate / 100 * p.Gain)).ToString();
            tbxGanancia.Text = p.Gain.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //Get Product Name, Rate and Qty customer wants to buy
            if (Producto.id != 0)
            {
                if (!pDAL.EvaluarPrecioMínimo(Producto.id.ToString(), decimal.Parse(txtRate.Text)))
                    return;

            }
            if (string.IsNullOrEmpty(TxtQty.Text) || TxtQty.Text == "0")
            {
                MessageBox.Show("Ingrese la cantidad de producto a agregar.");
                return;
            }
            string productName = txtProductName.Text;
            decimal Rate = decimal.Parse(txtRate.Text);
            decimal Gain = decimal.Parse(tbxGanancia.Text);
            decimal Qty = 0;
            
            if (ValidateNumber(TxtQty.Text))
            {
                Qty = decimal.Parse(TxtQty.Text);
            }
            else
            {
                TxtQty.Clear();
                return;
            }

            decimal Total = Rate * Qty; //Total=RatexQty

            //Display the Subtotal in textbox
            //Get the subtotal value from textbox
            decimal subTotal;
            if (ValidateNumber(txtSubTotal.Text))
            {
                subTotal = decimal.Parse(txtSubTotal.Text);
            }
            else
            {
                subTotal = 0;
            }
            subTotal = subTotal + Total;
            //Check whether the product is selected or not
            if (productName == "")
            {
                //Display error MEssage
                MessageBox.Show("Selecciona el producto.");
            }
            else
            {
                //Add product to the dAta Grid View
                transactionDT.Rows.Add(productName, Rate, Qty, Total, comboBox1.Text);
                //Show in DAta Grid View
                dgvAddedProducts.DataSource = transactionDT;
                //Display the Subtotal in textbox

                UpdateSubTotal();

                //Clear the Textboxes
                txtSearchProduct.Text = "";
                txtProductName.Text = "";
                txtInventory.Text = "";
                txtRate.Text = "";
                TxtQty.Text = "";
                tbxGanancia.Text = "";
            }
            Producto = new productsBLL();
        }

        private void txtDiscount_TextChanged(object sender, EventArgs e)
        {
            if(txtDiscount.Text != "")
            {

                if (ValidateNumber(txtDiscount.Text))
                {
                    TotalSum();
                }
                else
                {
                    txtDiscount.Clear();

                    txtDiscount.Text = "";
                }
                if(txtDiscount.Text != "")
                {
                    if (decimal.Parse(txtDiscount.Text) > 5)
                    {
                        txtDiscount.Text = "0";
                        MessageBox.Show("El valor máximo de descuento es 5%");
                    }
                }
                
            }
            

        }

        private void txtVat_TextChanged(object sender, EventArgs e)
        {
            if (ValidateNumber(txtVat.Text))
            {
                if (Convert.ToDouble(txtVat.Text) > 18)
                {
                    MessageBox.Show("El ITBIS no puede ser mayor que 18.");
                }
                TotalSum();
            }
            else
            {
                txtVat.Clear();
                TotalSum();

            }
        }

        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal paidAmount;
                decimal grandTotal;

                if (string.IsNullOrWhiteSpace(txtPaidAmount.Text))
                    paidAmount = 0;
                else
                    paidAmount = decimal.Parse(txtPaidAmount.Text);

                if (string.IsNullOrWhiteSpace(txtGrandTotal.Text))
                    grandTotal = 0;
                else
                    grandTotal = decimal.Parse(txtGrandTotal.Text);

                if(paidAmount != 0)
                {
                    decimal returnAmount = paidAmount - grandTotal;

                    //Display the return amount as well
                    txtReturnAmount.Text = returnAmount.ToString();
                }
                else
                {
                    txtReturnAmount.Clear();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Caracter inválido.");
                txtPaidAmount.Clear();
            }

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (transactionDT.Rows.Count == 0)
            {
                MessageBox.Show("Debe añadir productos a la factura");
                return;
            }
            //Get the Values from PurchaseSales Form First
            transactionsBLL transaction = new transactionsBLL();

            transaction.type = lblTop.Text;

            //Get the ID of Dealer or Customer Here
            //Lets get name of the dealer or customer first
            string deaCustName = txtName.Text;
            DeaCustBLL dc = dcDAL.GetDeaCustIDFromName(deaCustName);

            transaction.dea_cust_id = dc.id;
            transaction.grandTotal = Math.Round(decimal.Parse(txtGrandTotal.Text), 2);
            transaction.transaction_date = DateTime.Now;
            if (txtVat.Text == "")
                transaction.tax = 0;
            else
                transaction.tax = decimal.Parse(txtVat.Text);


            if (txtDiscount.Text == "")
            {
                transaction.discount = 0;

            }
            else if (ValidateNumber(txtDiscount.Text))
            {
                transaction.discount = decimal.Parse(txtDiscount.Text);
            }


            //Get the Username of Logged in user
            string username = frmLogin.loggedIn;
            userBLL u = uDAL.GetIDFromUsername(username);

            transaction.added_by = u.id;
            transaction.transactionDetails = transactionDT;

            //Lets Create a Boolean Variable and set its value to false
            bool success = false;

            //Actual Code to Insert Transaction And Transaction Details
            using (TransactionScope scope = new TransactionScope())
            {
                if (transaction.type == "Compra")
                {
                    transaction.grandTotal = transaction.grandTotal * -1;
                }
                int transactionID = -1;
                //Create aboolean value and insert transaction 
                bool w = tDAL.Insert_Transaction(transaction, out transactionID);

                var tID = tDAL.GetTransactionID(transaction.transaction_date, transaction.added_by);

                //Use for loop to insert Transaction Details
                for (int i = 0; i < transactionDT.Rows.Count; i++)
                {
                    //Get all the details of the product
                    transactionDetailBLL transactionDetail = new transactionDetailBLL();
                    //Get the Product name and convert it to id
                    string ProductName = transactionDT.Rows[i][0].ToString();
                    productsBLL p = pDAL.GetProductIDFromName(ProductName, comboBox1.Text);

                    transactionDetail.transactionID = int.Parse(tID);
                    transactionDetail.product_id = p.id;
                    transactionDetail.rate = decimal.Parse(transactionDT.Rows[i][1].ToString());
                    transactionDetail.qty = decimal.Parse(transactionDT.Rows[i][2].ToString());
                    transactionDetail.total = Math.Round(decimal.Parse(transactionDT.Rows[i][3].ToString()), 2);
                    transactionDetail.dea_cust_id = dc.id;
                    transactionDetail.added_date = DateTime.Now;
                    transactionDetail.added_by = u.id;

                    //Here Increase or Decrease Product Quantity based on Purchase or sales
                    string transactionType = lblTop.Text;

                    //Lets check whether we are on Purchase or Sales
                    bool x = false;
                    if (transactionType == "Compra")
                    {
                        //Increase the Product
                        x = pDAL.IncreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }
                    else if (transactionType == "Venta")
                    {
                        //Decrease the Product Quntiyt
                        x = pDAL.DecreaseProduct(transactionDetail.product_id, transactionDetail.qty);
                    }

                    //Insert Transaction Details inside the database
                    bool y = tdDAL.InsertTransactionDetail(transactionDetail);
                    success = w && x && y;
                }

                if (success == true)
                {
                    //Transaction Complete
                    scope.Complete();
                    try
                    {
                        if (lblTop.Text == "Venta")
                        {
                            printDialog1.Document = printDocument1;
                            if (printDialog1.ShowDialog() == DialogResult.OK)
                            {
                                FORCEPRINT = false;
                                printDocument1.Print();
                                FORCEPRINT = true;
                                printDocument1.Print();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("ha ocurrido un error al imprimir, revise la impresora");
                    }
                            
                    MessageBox.Show("Transacción completada satisfactoriamente.");
                    //Celar the Data Grid View and Clear all the TExtboxes
                    dgvAddedProducts.DataSource = null;
                    dgvAddedProducts.Rows.Clear();

                    txtSearch.Text = "";
                    txtName.Text = "";
                    txtEmail.Text = "";
                    txtContact.Text = "";
                    txtAddress.Text = "";
                    txtSearchProduct.Text = "";
                    txtProductName.Text = "";
                    txtInventory.Text = "";
                    txtRate.Text = "";
                    TxtQty.Text = "";
                    txtSubTotal.Text = "";
                    txtDiscount.Text = "";
                    txtVat.Text = "";
                    txtGrandTotal.Text = "";
                    txtPaidAmount.Text = "";
                    txtReturnAmount.Text = "";
                }
                else
                {
                    //Transaction Failed
                    MessageBox.Show("Transacción Fallida");
                }
                transactionDT.Clear();
            }
            //if (ChkFacFis.Checked)
            //    new ManejadorExcel().ExportarExcel();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSubTotal_TextChanged(object sender, EventArgs e)
        {
            TotalSum();
        }

        private void TotalSum()
        {
            decimal subTotal, discount, itbis;

            if (string.IsNullOrWhiteSpace(txtVat.Text))
                itbis = 0;
            else
                itbis = decimal.Parse(txtVat.Text);

            if (string.IsNullOrWhiteSpace(txtSubTotal.Text))
                subTotal = 0;
            else
                subTotal = decimal.Parse(txtSubTotal.Text);

            if (string.IsNullOrWhiteSpace(txtDiscount.Text))
                discount = 0;
            else
                discount = decimal.Parse(txtDiscount.Text);

            decimal grandTotal = ((100 + itbis) / 100) * subTotal;
            grandTotal = ((100 - discount) / 100) * grandTotal;
            txtGrandTotal.Text = grandTotal.ToString();

        }

        private void txtProductName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtInventory_TextChanged(object sender, EventArgs e)
        {

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
                    return false;
                }
                MessageBox.Show("Caracter inválido.");
                return false;
            }
        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            if (ValidateNumber(txtRate.Text))
            {
                return;
            }
            else
            {
                //txtRate.Clear();
                //string keyword = txtSearchProduct.Text;

                ////Check if we have value to txtSearchProduct or not
                //if (keyword == "")
                //{
                //    txtProductName.Text = "";
                //    txtInventory.Text = "";
                //    txtRate.Text = "";
                //    TxtQty.Text = "";
                //    return;
                //}

                ////Search the product and display on respective textboxes
                //productsBLL p = pDAL.GetProductsForTransaction(keyword, comboBox1.Text);
                //txtProductName.Text = p.name;
                //txtInventory.Text = p.qty.ToString();
                //txtRate.Text = p.rate.ToString();
            }
        }

        private void TxtQty_TextChanged(object sender, EventArgs e)
        {
            if (ValidateNumber(TxtQty.Text))
            {
                try
                {
                    int.Parse(TxtQty.Text);
                    return;
                }
                catch (Exception)
                {
                    TxtQty.Clear();
                }
            }
            else
            {
                TxtQty.Clear();

            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (itemIndex.HasValue && itemIndex + 1 <= transactionDT.Rows.Count)
            {
                transactionDT.Rows.Remove(transactionDT.Rows[itemIndex.Value]);
                itemIndex = null;
                UpdateSubTotal();
            }
            else
            {
                MessageBox.Show("Elija el producto a eliminar.");

                return;
            }
        }

        private void dgvAddedProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            itemIndex = e.RowIndex;
        }

        private void UpdateSubTotal()
        {
            double sum = 0;
            for (int i = 0; i < dgvAddedProducts.Rows.Count; ++i)
            {
                sum += Convert.ToDouble(dgvAddedProducts.Rows[i].Cells[3].Value);
            }
            txtSubTotal.Text = sum.ToString();
        }

        private void txtGrandTotal_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal paidAmount;
                decimal grandTotal;

                if (string.IsNullOrWhiteSpace(txtPaidAmount.Text))
                    paidAmount = 0;
                else
                    paidAmount = decimal.Parse(txtPaidAmount.Text);

                if (string.IsNullOrWhiteSpace(txtGrandTotal.Text))
                    grandTotal = 0;
                else
                    grandTotal = decimal.Parse(txtGrandTotal.Text);


                //Display the return amount as well
                if (paidAmount != 0)
                {
                    decimal returnAmount = paidAmount - grandTotal;

                    //Display the return amount as well
                    txtReturnAmount.Text = returnAmount.ToString();
                }
                else
                {
                    txtReturnAmount.Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Caracter inválido.");
                txtPaidAmount.Clear();
            }
        }

        private void TxtQty_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnAdd_Click(sender, e);
            }
        }

        private void pnlDeaCust_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtSearchProduct_TextChanged(sender, e);
        }

        private void lblTop_Click(object sender, EventArgs e)
        {

        }

        private void dgvAddedProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            itemIndex = null;
        }

        private void lblRate_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            new ManejadorImpresora().Imprimir(e, dgvAddedProducts , txtGrandTotal.Text, txtDiscount.Text, txtVat.Text, txtSubTotal.Text, printDocument1, null);
            if(!FORCEPRINT)
            e.Cancel = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();

        }

        private void Btn_Buscar_Click(object sender, EventArgs e)
        {
            new frmBuscar().ShowDialog();

        }
    }
}
