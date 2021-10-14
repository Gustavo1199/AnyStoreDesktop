namespace AnyStore
{
    partial class frmUserDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserDashboard));
            this.menuStripTop = new System.Windows.Forms.MenuStrip();
            this.purchaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesFormsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.inventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dealerAndCustomerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transaccionesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblLoggedInUser = new System.Windows.Forms.Label();
            this.lblUser = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.menuStripTop.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripTop
            // 
            this.menuStripTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(228)))), ((int)(((byte)(240)))));
            this.menuStripTop.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStripTop.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.purchaseToolStripMenuItem,
            this.salesFormsToolStripMenuItem,
            this.inventoryToolStripMenuItem,
            this.dealerAndCustomerToolStripMenuItem,
            this.transaccionesToolStripMenuItem});
            this.menuStripTop.Location = new System.Drawing.Point(0, 0);
            this.menuStripTop.Name = "menuStripTop";
            this.menuStripTop.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStripTop.Size = new System.Drawing.Size(936, 24);
            this.menuStripTop.TabIndex = 0;
            this.menuStripTop.Text = "menuStrip1";
            // 
            // purchaseToolStripMenuItem
            // 
            this.purchaseToolStripMenuItem.Name = "purchaseToolStripMenuItem";
            this.purchaseToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.purchaseToolStripMenuItem.Text = "Compras";
            this.purchaseToolStripMenuItem.Click += new System.EventHandler(this.purchaseToolStripMenuItem_Click);
            // 
            // salesFormsToolStripMenuItem
            // 
            this.salesFormsToolStripMenuItem.Name = "salesFormsToolStripMenuItem";
            this.salesFormsToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.salesFormsToolStripMenuItem.Text = "Ventas";
            this.salesFormsToolStripMenuItem.Click += new System.EventHandler(this.salesFormsToolStripMenuItem_Click);
            // 
            // inventoryToolStripMenuItem
            // 
            this.inventoryToolStripMenuItem.Name = "inventoryToolStripMenuItem";
            this.inventoryToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.inventoryToolStripMenuItem.Text = "Inventario";
            this.inventoryToolStripMenuItem.Click += new System.EventHandler(this.inventoryToolStripMenuItem_Click);
            // 
            // dealerAndCustomerToolStripMenuItem
            // 
            this.dealerAndCustomerToolStripMenuItem.Name = "dealerAndCustomerToolStripMenuItem";
            this.dealerAndCustomerToolStripMenuItem.Size = new System.Drawing.Size(138, 20);
            this.dealerAndCustomerToolStripMenuItem.Text = "Clientes y Proveedores";
            this.dealerAndCustomerToolStripMenuItem.Click += new System.EventHandler(this.dealerAndCustomerToolStripMenuItem_Click);
            // 
            // transaccionesToolStripMenuItem
            // 
            this.transaccionesToolStripMenuItem.Name = "transaccionesToolStripMenuItem";
            this.transaccionesToolStripMenuItem.Size = new System.Drawing.Size(92, 20);
            this.transaccionesToolStripMenuItem.Text = "Transacciones";
            this.transaccionesToolStripMenuItem.Click += new System.EventHandler(this.transaccionesToolStripMenuItem_Click);
            // 
            // lblLoggedInUser
            // 
            this.lblLoggedInUser.AutoSize = true;
            this.lblLoggedInUser.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggedInUser.ForeColor = System.Drawing.Color.LimeGreen;
            this.lblLoggedInUser.Location = new System.Drawing.Point(65, 41);
            this.lblLoggedInUser.Name = "lblLoggedInUser";
            this.lblLoggedInUser.Size = new System.Drawing.Size(0, 17);
            this.lblLoggedInUser.TabIndex = 8;
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(12, 41);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(60, 17);
            this.lblUser.TabIndex = 7;
            this.lblUser.Text = "Usuario: ";
            // 
            // pnlFooter
            // 
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(172)))));
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 367);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(936, 34);
            this.pnlFooter.TabIndex = 12;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(126)))), ((int)(((byte)(172)))));
            this.panel1.Controls.Add(this.btnActualizar);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dgvProducts);
            this.panel1.Location = new System.Drawing.Point(147, 82);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(608, 268);
            this.panel1.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(82, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(459, 29);
            this.label1.TabIndex = 6;
            this.label1.Text = "PRODUCTOS AGOTANDO EXISTENCIA";
            // 
            // dgvProducts
            // 
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Location = new System.Drawing.Point(28, 46);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.RowHeadersWidth = 51;
            this.dgvProducts.Size = new System.Drawing.Size(556, 186);
            this.dgvProducts.TabIndex = 5;
            this.dgvProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellContentClick);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(530, 238);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(75, 23);
            this.btnActualizar.TabIndex = 8;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // frmUserDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(936, 401);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.lblLoggedInUser);
            this.Controls.Add(this.lblUser);
            this.Controls.Add(this.menuStripTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripTop;
            this.Name = "frmUserDashboard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Menu";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmUserDashboard_FormClosed);
            this.Load += new System.EventHandler(this.frmUserDashboard_Load);
            this.menuStripTop.ResumeLayout(false);
            this.menuStripTop.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripTop;
        private System.Windows.Forms.ToolStripMenuItem purchaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salesFormsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem inventoryToolStripMenuItem;
        private System.Windows.Forms.Label lblLoggedInUser;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel pnlFooter;
        private System.Windows.Forms.ToolStripMenuItem dealerAndCustomerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transaccionesToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Button btnActualizar;
    }
}