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
    public partial class frmProducts : Form
    {
        public frmProducts()
        {
            InitializeComponent();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            //Add code to hide this form
            this.Hide();
        }

        categoriesDAL cdal = new categoriesDAL();
        warehouseDAL wdal = new warehouseDAL();
        productsBLL p = new productsBLL();
        productsDAL pdal = new productsDAL();
        userDAL udal = new userDAL();

        private void frmProducts_Load(object sender, EventArgs e)
        {
            //Creating DAta Table to hold the categories from Database
            DataTable categoriesDT = cdal.Select();
            //Specify DataSource for Category ComboBox
            cmbCategory.DataSource = categoriesDT;

            ////////////////////////////////////
            //Creating DAta Table to hold the categories from Database
            DataTable almcaenDT = wdal.Select();
            //Specify DataSource for Category ComboBox
            cmbWareHouse.DataSource = almcaenDT;
            //Specify Display Member and Value Member for Combobox
            cmbWareHouse.DisplayMember = "title";
            cmbCategory.DisplayMember = "Título";
            cmbCategory.ValueMember = "Título";
            cmbWareHouse.ValueMember = "Nombre";

            //Load all the Products in Data Grid View
            DataTable dt = pdal.Select();
            dgvProducts.DataSource = dt;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                //Get All the Values from Product Form
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Ingrese el nombre del producto.");
                    return;
                }
                if (string.IsNullOrEmpty(TbxCantidadMinima.Text))
                {
                    MessageBox.Show("Ingrese la cantidad mínima del producto");
                    return;
                }
                if (string.IsNullOrEmpty(txtRate.Text))
                {
                    MessageBox.Show("Ingrese el precio del producto.");
                    return;
                }
                if (string.IsNullOrEmpty(cmbCategory.Text))
                {
                    MessageBox.Show("Ingrese la categoría del producto.");
                    return;
                }
                if (string.IsNullOrEmpty(tbxGanancia.Text))
                {
                    MessageBox.Show("Ingrese la ganancia del producto.");
                    return;
                }
                if (TbxCantidad.Text == "")
                {
                    TbxCantidad.Text = "0";
                }
                if (string.IsNullOrEmpty(tbxMinPrice.Text))
                {
                    MessageBox.Show("Debe establecer el precio mínimo.");
                    return;
                }

                p.name = txtName.Text;
                p.Categoria = cmbCategory.Text;
                p.description = txtDescription.Text;
                p.rate = decimal.Parse(txtRate.Text);
                p.qty = 0;
                p.added_date = DateTime.Now;
                p.warehouse = cmbWareHouse.Text;
                p.PriceMinimum = int.Parse(tbxMinPrice.Text);
                p.StockMinimum = int.Parse(TbxCantidadMinima.Text);
                p.Gain = decimal.Parse(tbxGanancia.Text);
                //Getting username of logged in user
                String loggedUsr = frmLogin.loggedIn;
                userBLL usr = udal.GetIDFromUsername(loggedUsr);
                p.qty = decimal.Parse(TbxCantidad.Text);
                p.added_by = usr.id;

                //Create Boolean variable to check if the product is added successfully or not
                bool success = pdal.Insert(p);
                //if the product is added successfully then the value of success will be true else it will be false
                if (success == true)
                {
                    //Product Inserted Successfully
                    MessageBox.Show("Agregado satisfactoriamente");
                    //Calling the Clear Method
                    Clear();
                    TbxCantidadMinima.Text = "";
                    tbxMinPrice.Text = "";
                    tbxGanancia.Text = "";
                    TbxCantidad.Text = "";
                    //Refreshing DAta Grid View
                    DataTable dt = pdal.Select();
                    dgvProducts.DataSource = dt;
                }
                else
                {
                    //Failed to Add New product
                    MessageBox.Show("Error al agregar el nuevo producto.");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        public void Clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtDescription.Text = "";
            txtRate.Text = "";
            txtSearch.Text = "";
        }
        private bool ValidateNumber(string value)
        {
            try
            {
                if (value == "")
                    return true;
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
        private void dgvProducts_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //Create integer variable to know which product was clicked
            int rowIndex = e.RowIndex;
            //Display the Value on Respective TextBoxes
            txtID.Text = dgvProducts.Rows[rowIndex].Cells[0].Value.ToString();
            txtName.Text = dgvProducts.Rows[rowIndex].Cells[1].Value.ToString();
            cmbCategory.Text = dgvProducts.Rows[rowIndex].Cells[3].Value.ToString();
            txtRate.Text = dgvProducts.Rows[rowIndex].Cells[4].Value.ToString();
            tbxMinPrice.Text = dgvProducts.Rows[rowIndex].Cells[5].Value.ToString();
            txtDescription.Text = dgvProducts.Rows[rowIndex].Cells[2].Value.ToString();
            TbxCantidadMinima.Text = dgvProducts.Rows[rowIndex].Cells[7].Value.ToString();
            tbxGanancia.Text = dgvProducts.Rows[rowIndex].Cells[8].Value.ToString();
            cmbWareHouse.Text = dgvProducts.Rows[rowIndex].Cells[9].Value.ToString();
            TbxCantidad.Text = dgvProducts.Rows[rowIndex].Cells[6].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                return;
            }
            if (string.IsNullOrEmpty(txtName.Text))
            {
                MessageBox.Show("Ingrese el nombre del producto.");
                return;
            }
            if (string.IsNullOrEmpty(txtRate.Text))
            {
                MessageBox.Show("Ingrese el precio del producto.");
                return;
            }
            if (string.IsNullOrEmpty(tbxMinPrice.Text))
            {
                MessageBox.Show("Establecer un precio mínimo del producto.");
                return;
            }
            if (string.IsNullOrEmpty(TbxCantidadMinima.Text))
            {
                MessageBox.Show("Establezca el valor mínimo para alerta.");
                return;
            }
            if (string.IsNullOrEmpty(tbxGanancia.Text))
            {
                MessageBox.Show("Ingrese la ganancia del producto.");
                return;
            }
            //Get the Values from UI or Product Form
            p.id = int.Parse(txtID.Text);
            p.name = txtName.Text;
            p.Categoria = cmbCategory.Text;
            p.description = txtDescription.Text;
            p.rate = decimal.Parse(txtRate.Text);
            p.added_date = DateTime.Now;
            p.warehouse = cmbWareHouse.Text;
            p.StockMinimum = int.Parse(TbxCantidadMinima.Text);
            p.PriceMinimum = int.Parse(tbxMinPrice.Text);
            p.Gain = decimal.Parse(tbxGanancia.Text);
            p.qty = decimal.Parse(TbxCantidad.Text);
            //Getting Username of logged in user for added by
            String loggedUsr = frmLogin.loggedIn;
            userBLL usr = udal.GetIDFromUsername(loggedUsr);

            p.added_by = usr.id;

            //Create a boolean variable to check if the product is updated or not
            bool success = pdal.Update(p);
            //If the prouct is updated successfully then the value of success will be true else it will be false
            if(success==true)
            {
                //Product updated Successfully
                MessageBox.Show("Actualizado satisfactoriamente");
                Clear();
                TbxCantidadMinima.Text = "";
                tbxMinPrice.Text = "";
                tbxGanancia.Text = "";
                //REfresh the Data Grid View
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                //Failed to Update Product
                MessageBox.Show("Actualización fallida.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //GEt the ID of the product to be deleted
            if (string.IsNullOrEmpty(txtID.Text))
            {
                return;
            }
            p.id = int.Parse(txtID.Text);

            //Create Bool VAriable to Check if the product is deleted or not
            bool success = pdal.Delete(p);

            //If prouct is deleted successfully then the value of success will true else it will be false
            if(success==true)
            {
                //Product Successfuly Deleted
                MessageBox.Show("Eliminado satisfactoriamente.");
                Clear();
                TbxCantidadMinima.Text = "";
                tbxMinPrice.Text = "";
                //Refresh DAta Grid View
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
            else
            {
                //Failed to Delete Product
                MessageBox.Show("Eliminación Fallida.");
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            //Get the Keywordss from Form
            string keywords = txtSearch.Text;

            if(keywords!=null)
            {
                //Search the products
                DataTable dt = pdal.Search(keywords);
                dgvProducts.DataSource = dt;
            }
            else
            {
                //Display All the products
                DataTable dt = pdal.Select();
                dgvProducts.DataSource = dt;
            }
        }

        private void dgvProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtRate_TextChanged(object sender, EventArgs e)
        {
            if (!ValidateNumber(txtRate.Text))
                txtRate.Text = "";
        }

        private void cmbWareHouse_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tbxMinPrice_TextChanged(object sender, EventArgs e)
        {
            if (!ValidateNumber(tbxMinPrice.Text))
                tbxMinPrice.Text = "";
        }

        private void tbxCantMínima_TextChanged(object sender, EventArgs e)
        {
            if (!ValidateNumber(TbxCantidadMinima.Text))
                TbxCantidadMinima.Text = "";
        }

        private void tbxGanancia_TextChanged(object sender, EventArgs e)
        {
            if (!ValidateNumber(tbxGanancia.Text))
                tbxGanancia.Text = "";
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void TbxCantidad_TextChanged(object sender, EventArgs e)
        {
            if (!ValidateNumber(TbxCantidad.Text))
                TbxCantidad.Text = "";
        }

        private void lblRate_Click(object sender, EventArgs e)
        {

        }
    }
}
