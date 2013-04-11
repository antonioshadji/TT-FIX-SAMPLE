namespace FIX_TRADER
{
    partial class Form1
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
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.btnOff = new System.Windows.Forms.Button();
            this.btnNewOrderSingle = new System.Windows.Forms.Button();
            this.btnSecurityDefinitionRequest = new System.Windows.Forms.Button();
            this.btnGatewayStatusRequest = new System.Windows.Forms.Button();
            this.btnRequestForPosition = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbxContracts = new System.Windows.Forms.ComboBox();
            this.cbxBS = new System.Windows.Forms.ComboBox();
            this.txtAccount = new System.Windows.Forms.TextBox();
            this.cbxGateways = new System.Windows.Forms.ComboBox();
            this.txtTicker = new System.Windows.Forms.TextBox();
            this.btnOrderStatusRequest = new System.Windows.Forms.Button();
            this.btnOrderCancelReplace = new System.Windows.Forms.Button();
            this.btnOrderCancelRequest = new System.Windows.Forms.Button();
            this.btnMarketDataRequest = new System.Windows.Forms.Button();
            this.btnSecurityStatusRequest = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(315, 199);
            this.listBox1.TabIndex = 0;
            // 
            // btnOff
            // 
            this.btnOff.BackColor = System.Drawing.SystemColors.Control;
            this.btnOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOff.Location = new System.Drawing.Point(161, 77);
            this.btnOff.Name = "btnOff";
            this.btnOff.Size = new System.Drawing.Size(149, 23);
            this.btnOff.TabIndex = 82;
            this.btnOff.Text = "Log Out (5)";
            this.btnOff.UseVisualStyleBackColor = false;
            this.btnOff.Click += new System.EventHandler(this.btnLogout_Click);
            // 
            // btnNewOrderSingle
            // 
            this.btnNewOrderSingle.Location = new System.Drawing.Point(6, 77);
            this.btnNewOrderSingle.Name = "btnNewOrderSingle";
            this.btnNewOrderSingle.Size = new System.Drawing.Size(149, 23);
            this.btnNewOrderSingle.TabIndex = 84;
            this.btnNewOrderSingle.Text = "NewOrderSingle (D)";
            this.btnNewOrderSingle.UseVisualStyleBackColor = true;
            this.btnNewOrderSingle.Click += new System.EventHandler(this.btnNewOrderSingle_Click);
            // 
            // btnSecurityDefinitionRequest
            // 
            this.btnSecurityDefinitionRequest.Location = new System.Drawing.Point(161, 48);
            this.btnSecurityDefinitionRequest.Name = "btnSecurityDefinitionRequest";
            this.btnSecurityDefinitionRequest.Size = new System.Drawing.Size(149, 23);
            this.btnSecurityDefinitionRequest.TabIndex = 85;
            this.btnSecurityDefinitionRequest.Text = "SecurityDefinitionRequest(c)";
            this.btnSecurityDefinitionRequest.UseVisualStyleBackColor = true;
            this.btnSecurityDefinitionRequest.Click += new System.EventHandler(this.btnSecurityDefinitionRequest_Click);
            // 
            // btnGatewayStatusRequest
            // 
            this.btnGatewayStatusRequest.Location = new System.Drawing.Point(6, 48);
            this.btnGatewayStatusRequest.Name = "btnGatewayStatusRequest";
            this.btnGatewayStatusRequest.Size = new System.Drawing.Size(149, 23);
            this.btnGatewayStatusRequest.TabIndex = 86;
            this.btnGatewayStatusRequest.Text = "GatewayStatusRequest";
            this.btnGatewayStatusRequest.UseVisualStyleBackColor = true;
            this.btnGatewayStatusRequest.Click += new System.EventHandler(this.btnGatewayStatusRequest_Click);
            // 
            // btnRequestForPosition
            // 
            this.btnRequestForPosition.Location = new System.Drawing.Point(161, 135);
            this.btnRequestForPosition.Name = "btnRequestForPosition";
            this.btnRequestForPosition.Size = new System.Drawing.Size(149, 23);
            this.btnRequestForPosition.TabIndex = 87;
            this.btnRequestForPosition.Text = "RequestForPosition";
            this.btnRequestForPosition.UseVisualStyleBackColor = true;
            this.btnRequestForPosition.Click += new System.EventHandler(this.btnRequestForPosition_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOff);
            this.groupBox1.Controls.Add(this.btnGatewayStatusRequest);
            this.groupBox1.Controls.Add(this.cbxContracts);
            this.groupBox1.Controls.Add(this.cbxBS);
            this.groupBox1.Controls.Add(this.txtAccount);
            this.groupBox1.Controls.Add(this.cbxGateways);
            this.groupBox1.Controls.Add(this.txtTicker);
            this.groupBox1.Controls.Add(this.btnOrderStatusRequest);
            this.groupBox1.Controls.Add(this.btnOrderCancelReplace);
            this.groupBox1.Controls.Add(this.btnSecurityDefinitionRequest);
            this.groupBox1.Controls.Add(this.btnOrderCancelRequest);
            this.groupBox1.Controls.Add(this.btnMarketDataRequest);
            this.groupBox1.Controls.Add(this.btnSecurityStatusRequest);
            this.groupBox1.Controls.Add(this.btnRequestForPosition);
            this.groupBox1.Controls.Add(this.btnNewOrderSingle);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 199);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(315, 192);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Control Panel";
            // 
            // cbxContracts
            // 
            this.cbxContracts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cbxContracts.FormattingEnabled = true;
            this.cbxContracts.Location = new System.Drawing.Point(113, 19);
            this.cbxContracts.Name = "cbxContracts";
            this.cbxContracts.Size = new System.Drawing.Size(58, 21);
            this.cbxContracts.TabIndex = 99;
            // 
            // cbxBS
            // 
            this.cbxBS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxBS.FormattingEnabled = true;
            this.cbxBS.Items.AddRange(new object[] {
            "BUY",
            "SELL"});
            this.cbxBS.Location = new System.Drawing.Point(177, 19);
            this.cbxBS.Name = "cbxBS";
            this.cbxBS.Size = new System.Drawing.Size(50, 21);
            this.cbxBS.TabIndex = 98;
            this.cbxBS.TextChanged += new System.EventHandler(this.cbxBS_TextChanged);
            // 
            // txtAccount
            // 
            this.txtAccount.Location = new System.Drawing.Point(233, 20);
            this.txtAccount.Name = "txtAccount";
            this.txtAccount.Size = new System.Drawing.Size(72, 20);
            this.txtAccount.TabIndex = 97;
            this.txtAccount.Text = "sl5302";
            this.txtAccount.TextChanged += new System.EventHandler(this.account_TextChanged);
            // 
            // cbxGateways
            // 
            this.cbxGateways.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cbxGateways.FormattingEnabled = true;
            this.cbxGateways.Location = new System.Drawing.Point(6, 19);
            this.cbxGateways.Name = "cbxGateways";
            this.cbxGateways.Size = new System.Drawing.Size(55, 21);
            this.cbxGateways.TabIndex = 96;
            this.cbxGateways.Text = "CME";
            this.cbxGateways.TextChanged += new System.EventHandler(this.cbxGateways_TextChanged);
            // 
            // txtTicker
            // 
            this.txtTicker.Location = new System.Drawing.Point(67, 19);
            this.txtTicker.Name = "txtTicker";
            this.txtTicker.Size = new System.Drawing.Size(40, 20);
            this.txtTicker.TabIndex = 94;
            this.txtTicker.Text = "ES";
            this.txtTicker.TextChanged += new System.EventHandler(this.txtTicker_TextChanged);
            // 
            // btnOrderStatusRequest
            // 
            this.btnOrderStatusRequest.Location = new System.Drawing.Point(6, 164);
            this.btnOrderStatusRequest.Name = "btnOrderStatusRequest";
            this.btnOrderStatusRequest.Size = new System.Drawing.Size(149, 23);
            this.btnOrderStatusRequest.TabIndex = 93;
            this.btnOrderStatusRequest.Text = "Order Status Request (H)";
            this.btnOrderStatusRequest.UseVisualStyleBackColor = true;
            this.btnOrderStatusRequest.Click += new System.EventHandler(this.btnOrderStatusRequest_Click);
            // 
            // btnOrderCancelReplace
            // 
            this.btnOrderCancelReplace.Location = new System.Drawing.Point(6, 135);
            this.btnOrderCancelReplace.Name = "btnOrderCancelReplace";
            this.btnOrderCancelReplace.Size = new System.Drawing.Size(149, 23);
            this.btnOrderCancelReplace.TabIndex = 92;
            this.btnOrderCancelReplace.Text = "Order Cancel/Replace(G)";
            this.btnOrderCancelReplace.UseVisualStyleBackColor = true;
            this.btnOrderCancelReplace.Click += new System.EventHandler(this.btnOrderCancelReplace_Click);
            // 
            // btnOrderCancelRequest
            // 
            this.btnOrderCancelRequest.Location = new System.Drawing.Point(6, 106);
            this.btnOrderCancelRequest.Name = "btnOrderCancelRequest";
            this.btnOrderCancelRequest.Size = new System.Drawing.Size(149, 23);
            this.btnOrderCancelRequest.TabIndex = 91;
            this.btnOrderCancelRequest.Text = "Order Cancel Request (F)";
            this.btnOrderCancelRequest.UseVisualStyleBackColor = true;
            this.btnOrderCancelRequest.Click += new System.EventHandler(this.btnOrderCancelRequest_Click);
            // 
            // btnMarketDataRequest
            // 
            this.btnMarketDataRequest.Location = new System.Drawing.Point(161, 106);
            this.btnMarketDataRequest.Name = "btnMarketDataRequest";
            this.btnMarketDataRequest.Size = new System.Drawing.Size(149, 23);
            this.btnMarketDataRequest.TabIndex = 90;
            this.btnMarketDataRequest.Text = "Market Data Request (V)";
            this.btnMarketDataRequest.UseVisualStyleBackColor = true;
            this.btnMarketDataRequest.Click += new System.EventHandler(this.btnMarketDataRequest_Click);
            // 
            // btnSecurityStatusRequest
            // 
            this.btnSecurityStatusRequest.Location = new System.Drawing.Point(161, 164);
            this.btnSecurityStatusRequest.Name = "btnSecurityStatusRequest";
            this.btnSecurityStatusRequest.Size = new System.Drawing.Size(149, 23);
            this.btnSecurityStatusRequest.TabIndex = 89;
            this.btnSecurityStatusRequest.Text = "Security Status Request (e)";
            this.btnSecurityStatusRequest.UseVisualStyleBackColor = true;
            this.btnSecurityStatusRequest.Click += new System.EventHandler(this.btnSecurityStatusRequest_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 394);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(315, 22);
            this.statusStrip1.TabIndex = 6;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(315, 416);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listBox1);
            this.Name = "Form1";
            this.Text = "FIX_TRADER";
            this.Load += new System.EventHandler(this.main_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.main_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button btnOff;
        private System.Windows.Forms.Button btnNewOrderSingle;
        private System.Windows.Forms.Button btnSecurityDefinitionRequest;
        private System.Windows.Forms.Button btnGatewayStatusRequest;
        private System.Windows.Forms.Button btnRequestForPosition;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSecurityStatusRequest;
        private System.Windows.Forms.Button btnOrderStatusRequest;
        private System.Windows.Forms.Button btnOrderCancelReplace;
        private System.Windows.Forms.Button btnOrderCancelRequest;
        private System.Windows.Forms.Button btnMarketDataRequest;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TextBox txtTicker;
        private System.Windows.Forms.ComboBox cbxGateways;
        private System.Windows.Forms.TextBox txtAccount;
        private System.Windows.Forms.ComboBox cbxBS;
        private System.Windows.Forms.ComboBox cbxContracts;
    }
}

