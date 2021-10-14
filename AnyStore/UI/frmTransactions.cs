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
    public partial class frmTransactions : Form
    {
        public DataTable TransSelec = null;
        public int codigo = 0;
        public string TranType = "";
        public string Rol = "";
        public int RowIndex = 0;
        private bool PrintHelper = false;
        public frmTransactions(string Rol)
        {
            InitializeComponent();

            txtTransacciones.Text = tdal.GetTotalTransaccions();
            this.Rol = Rol;
            cmbMonths.Items.Add("");
            for (int i = 1; i <= 12; i++)
            {
                cmbMonths.Items.Add(i.ToString());
            }
            cmbYear.Items.Add("");
            for (int i = DateTime.Now.Year; i > 1980; i--)
            {
                cmbYear.Items.Add(i.ToString());
            }
            cmbYear.SelectedItem = DateTime.Now.Year.ToString();
            cmbMonths.SelectedItem = DateTime.Now.Month.ToString();

            cmbDias.Items.Add("");
            for (int i = 1; i < 32; i++)
            {
                cmbDias.Items.Add(i.ToString());
            }
            cmbDias.SelectedItem = DateTime.Now.Day.ToString();
        }

        transactionDAL tdal = new transactionDAL();
        transactionDetailDAL tdetdal = new transactionDetailDAL();
        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void FiltrarFecha()
        {
            string year = cmbYear.Text;

            if (cmbYear.SelectedItem.ToString() == "")
            {
                DisplayAll();

            }
            else if (cmbMonths.SelectedIndex != -1 && cmbMonths.SelectedItem.ToString() == "")
            {
                DataTable dt = tdal.DisplayTransactionByDate(year, "", "", cmbTransactionType.Text);
                dgvTransactions.DataSource = dt;

                txtTransacciones.Text = tdal.DisplaySumTransactionByDate(year, "", "", cmbTransactionType.Text);
            }
            else if (cmbDias.SelectedIndex != -1 && cmbDias.SelectedItem.ToString() == "")
            {
                DataTable dt = tdal.DisplayTransactionByDate(year, cmbMonths.Text, "", cmbTransactionType.Text);
                dgvTransactions.DataSource = dt;

                txtTransacciones.Text = tdal.DisplaySumTransactionByDate(year, cmbMonths.Text, "", cmbTransactionType.Text);
            }
            else

            {
                try
                {
                    DataTable dt = tdal.DisplayTransactionByDate(year, cmbMonths.Text, cmbDias.Text, cmbTransactionType.Text);
                    dgvTransactions.DataSource = dt;

                    txtTransacciones.Text = tdal.DisplaySumTransactionByDate(year, cmbMonths.Text, cmbDias.Text, cmbTransactionType.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Día inválido para el mes y año especificado");
                }
            }


        }
        private void frmTransactions_Load(object sender, EventArgs e)
        {

            btnCancel.Visible = false;
            //Dispay all the transactions
            //DataTable dt = tdal.DisplayAllTransactions();
            //dgvTransactions.DataSource = dt;
        }

        private void cmbTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarFecha();

        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            //Dispay all the transactions
            DisplayAll();
        }
        private void DisplayAll()
        {
            DataTable dt = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = dt;
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarFecha();
        }

        private void cmbMonths_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarFecha();

        }

        private void dgvTransactions_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvTransactions_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvTransactions_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void dgvTransactions_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int RowIndex = e.RowIndex;
            if (dgvTransactions.Rows[RowIndex].Cells["Estatus"].Value.ToString() == "CANCELADA" || string.IsNullOrWhiteSpace(dgvTransactions.Rows[RowIndex].Cells["Estatus"].Value.ToString()))
                return;
            string id = dgvTransactions.Rows[RowIndex].Cells[0].Value.ToString();
            string tipoTran = dgvTransactions.Rows[RowIndex].Cells[1].Value.ToString();


            DataTable dt = tdetdal.GetDetailsFromTransaction(id);
            int CodDeaCust = int.Parse(dgvTransactions.Rows[RowIndex].Cells["Cliente/Proveedor"].Value.ToString());
            decimal Impuesto = decimal.Parse(dgvTransactions.Rows[RowIndex].Cells["Impuesto"].Value.ToString());
            decimal Descuento = decimal.Parse(dgvTransactions.Rows[RowIndex].Cells["Descuento"].Value.ToString());
            DetailsProducts detailsProducts = new DetailsProducts(dt, int.Parse(id), tipoTran, Rol , CodDeaCust, Impuesto, Descuento);
            detailsProducts.StartPosition = FormStartPosition.CenterParent;
            detailsProducts.ShowDialog();
        }

        private void dgvTransactions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RowIndex = e.RowIndex;

            if (RowIndex > -1)
            {
                
                if ((dgvTransactions.Rows.Count - 1) != RowIndex)
                {
                    codigo = int.Parse(dgvTransactions.Rows[RowIndex].Cells[0].Value.ToString());
                    TranType = dgvTransactions.Rows[RowIndex].Cells[1].Value.ToString();
                    if (dgvTransactions.Rows[RowIndex].Cells["Estatus"].Value.ToString().ToUpper() != "CANCELADA")
                        btnCancel.Visible = true;
                    else
                        btnCancel.Visible = false;
                    button1.Visible = true;
                    TransSelec = new transactionDetailDAL().GetDetailsFromTransaction(codigo.ToString());
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {


            new frmAutorizacion(codigo, TranType, Rol);

            dgvTransactions.DataSource = tdal.DisplayAllTransactions();
            btnCancel.Visible = false;
            RowIndex = -1;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            printDialog1.Document = printDocument1;
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                PrintHelper = false;
                printDocument1.Print();
                PrintHelper = true;
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            decimal sum = 0;
            for(int i = 0;i<TransSelec.Rows.Count;i++)
            {
                sum += decimal.Parse(TransSelec.Rows[i].ItemArray[3].ToString());

            }
            new ManejadorImpresora().Imprimir(e, null, dgvTransactions.Rows[RowIndex].Cells["Monto"].Value.ToString(), dgvTransactions.Rows[RowIndex].Cells["Descuento"].Value.ToString(), dgvTransactions.Rows[RowIndex].Cells["Impuesto"].Value.ToString(), sum.ToString(), printDocument1, TransSelec);
            if (!PrintHelper)
                e.Cancel = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            btnCancel.Visible = false;
            //Dispay all the transactions
            DataTable dt = tdal.DisplayAllTransactions();
            dgvTransactions.DataSource = dt;
            cmbMonths.SelectedIndex = 0;
            cmbYear.SelectedIndex = 0;
            cmbDias.SelectedIndex = 0;
            cmbTransactionType.SelectedIndex = 0;
            txtTransacciones.Text = tdal.GetTotalTransaccions();

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            FiltrarFecha();

        }
    }
}
