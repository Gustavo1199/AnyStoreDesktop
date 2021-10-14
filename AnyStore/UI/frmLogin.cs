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
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        loginBLL l = new loginBLL();
        loginDAL dal = new loginDAL();
        public static string loggedIn;
        
        private void pboxClose_Click(object sender, EventArgs e)
        {
            //Code to close this form
            Application.Exit();            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            var rol = LoginExternal(txtUsername.Text.Trim(), txtPassword.Text.Trim());

            switch (rol)
            {

                case "Admin":
                    {
                        MessageBox.Show(string.Format("Bienvenido {0}", l.username));
                        loggedIn = l.username;
                        //Display Admin Dashboard
                        frmAdminDashboard admin = new frmAdminDashboard();
                        admin.Show();
                        this.Hide();
                    }
                    break;

                case "User":
                    {
                        MessageBox.Show(string.Format("Bienvenido {0}", l.username));
                        loggedIn = l.username;
                        //Display User Dashboard
                        frmUserDashboard user = new frmUserDashboard();
                        user.Show();
                        this.Hide();
                    }
                    break;

                default:
                    {
                        //Display an error message
                        MessageBox.Show("Usuario y/o contraseña incorrectos");
                    }
                    break;
            }
        }

        public string LoginExternal(string username, string password)
        {
            l.username = username;
            l.password = password;

            //Checking the login credentials
            var rol = dal.loginCheck(l);
            //Need to open Respective Forms based on User Type
            return rol;
        }

        private void cmbUserType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPassword_Enter(object sender, EventArgs e)
        {

        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
