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
    public partial class frmAutorizacion : Form
    {
        public readonly int codigo;
        public readonly string tipo;
        public frmAutorizacion(int codigo, string tipo, string rol)
        {
            this.codigo = codigo;
            this.tipo = tipo;
            InitializeComponent();
            if(rol == "USER") 
            {
                StartPosition = FormStartPosition.CenterParent;
                ShowDialog();
            }
            if(rol == "ADMIN")
            {
                new transactionDetailDAL().Update(codigo, tipo);
                MessageBox.Show("Transaccion cancelada satisfactoriamente");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            frmLogin login = new frmLogin();

            var rol = login.LoginExternal(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            switch (rol)
            {

                    case "Admin":
                    {

                        transactionDetailDAL detailDAL = new transactionDetailDAL();
                        detailDAL.Update(codigo, tipo);
                        MessageBox.Show("Transaccion cancelada satisfactoriamente");

                        this.Close();
                    }
                        break;

                    case "User":
                    {
                        MessageBox.Show("Solo los usuarios Administradores pueden modificar factura");
                        this.Close();
                    }
                        break;

                    default:
                    {
                        //Display an error message
                        MessageBox.Show("Usuario y/o contraseña incorrectos");
                        this.Hide();

                    }
                    break;
            }
        

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pboxClose_Click(object sender, EventArgs e)
        {

        }
    }
}
