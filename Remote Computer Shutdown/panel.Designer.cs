namespace Remote_Computer_Shutdown
{
    partial class panel
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
            this.grpbox_devices = new System.Windows.Forms.GroupBox();
            this.dgrid_devices = new System.Windows.Forms.DataGridView();
            this.grpbox_adddevice = new System.Windows.Forms.GroupBox();
            this.btn_selectall = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_select = new System.Windows.Forms.Button();
            this.btn_add = new System.Windows.Forms.Button();
            this.txt_nickname = new System.Windows.Forms.TextBox();
            this.txt_ipaddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_logout = new System.Windows.Forms.Button();
            this.btn_shutdownall = new System.Windows.Forms.Button();
            this.btn_shutdown = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.txt_komut = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.grpbox_devices.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgrid_devices)).BeginInit();
            this.grpbox_adddevice.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpbox_devices
            // 
            this.grpbox_devices.Controls.Add(this.dgrid_devices);
            this.grpbox_devices.Location = new System.Drawing.Point(316, 46);
            this.grpbox_devices.Name = "grpbox_devices";
            this.grpbox_devices.Size = new System.Drawing.Size(676, 461);
            this.grpbox_devices.TabIndex = 49;
            this.grpbox_devices.TabStop = false;
            this.grpbox_devices.Text = "Devices";
            // 
            // dgrid_devices
            // 
            this.dgrid_devices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgrid_devices.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dgrid_devices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgrid_devices.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgrid_devices.Location = new System.Drawing.Point(3, 26);
            this.dgrid_devices.Name = "dgrid_devices";
            this.dgrid_devices.RowHeadersWidth = 51;
            this.dgrid_devices.RowTemplate.Height = 24;
            this.dgrid_devices.Size = new System.Drawing.Size(670, 432);
            this.dgrid_devices.TabIndex = 50;
            this.dgrid_devices.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrid_devices_CellContentClick);
            this.dgrid_devices.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrid_devices_CellContentDoubleClick);
            this.dgrid_devices.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgrid_devices_CellContentClick);
            // 
            // grpbox_adddevice
            // 
            this.grpbox_adddevice.Controls.Add(this.label2);
            this.grpbox_adddevice.Controls.Add(this.txt_komut);
            this.grpbox_adddevice.Controls.Add(this.btn_selectall);
            this.grpbox_adddevice.Controls.Add(this.btn_delete);
            this.grpbox_adddevice.Controls.Add(this.btn_update);
            this.grpbox_adddevice.Controls.Add(this.btn_select);
            this.grpbox_adddevice.Controls.Add(this.btn_add);
            this.grpbox_adddevice.Controls.Add(this.txt_nickname);
            this.grpbox_adddevice.Controls.Add(this.txt_ipaddress);
            this.grpbox_adddevice.Controls.Add(this.label3);
            this.grpbox_adddevice.Controls.Add(this.label1);
            this.grpbox_adddevice.Location = new System.Drawing.Point(15, 46);
            this.grpbox_adddevice.Name = "grpbox_adddevice";
            this.grpbox_adddevice.Size = new System.Drawing.Size(283, 461);
            this.grpbox_adddevice.TabIndex = 22;
            this.grpbox_adddevice.TabStop = false;
            this.grpbox_adddevice.Text = "Add Device";
            // 
            // btn_selectall
            // 
            this.btn_selectall.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btn_selectall.ForeColor = System.Drawing.Color.White;
            this.btn_selectall.Location = new System.Drawing.Point(116, 412);
            this.btn_selectall.Name = "btn_selectall";
            this.btn_selectall.Size = new System.Drawing.Size(158, 43);
            this.btn_selectall.TabIndex = 11;
            this.btn_selectall.Text = "Select All";
            this.btn_selectall.UseVisualStyleBackColor = false;
            this.btn_selectall.Click += new System.EventHandler(this.btn_selectall_Click);
            // 
            // btn_delete
            // 
            this.btn_delete.BackColor = System.Drawing.Color.Red;
            this.btn_delete.ForeColor = System.Drawing.Color.White;
            this.btn_delete.Location = new System.Drawing.Point(116, 307);
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.Size = new System.Drawing.Size(158, 43);
            this.btn_delete.TabIndex = 10;
            this.btn_delete.Text = "Delete";
            this.btn_delete.UseVisualStyleBackColor = false;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_update
            // 
            this.btn_update.BackColor = System.Drawing.Color.Gold;
            this.btn_update.ForeColor = System.Drawing.Color.White;
            this.btn_update.Location = new System.Drawing.Point(116, 258);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(158, 43);
            this.btn_update.TabIndex = 9;
            this.btn_update.Text = "Update";
            this.btn_update.UseVisualStyleBackColor = false;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_select
            // 
            this.btn_select.BackColor = System.Drawing.Color.LightSeaGreen;
            this.btn_select.ForeColor = System.Drawing.Color.White;
            this.btn_select.Location = new System.Drawing.Point(116, 209);
            this.btn_select.Name = "btn_select";
            this.btn_select.Size = new System.Drawing.Size(158, 43);
            this.btn_select.TabIndex = 8;
            this.btn_select.Text = "Find/Select";
            this.btn_select.UseVisualStyleBackColor = false;
            this.btn_select.Click += new System.EventHandler(this.btn_select_Click);
            // 
            // btn_add
            // 
            this.btn_add.BackColor = System.Drawing.Color.Green;
            this.btn_add.ForeColor = System.Drawing.Color.White;
            this.btn_add.Location = new System.Drawing.Point(116, 160);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(158, 43);
            this.btn_add.TabIndex = 7;
            this.btn_add.Text = "Add/Insert";
            this.btn_add.UseVisualStyleBackColor = false;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // txt_nickname
            // 
            this.txt_nickname.Location = new System.Drawing.Point(116, 88);
            this.txt_nickname.Name = "txt_nickname";
            this.txt_nickname.Size = new System.Drawing.Size(158, 30);
            this.txt_nickname.TabIndex = 6;
            // 
            // txt_ipaddress
            // 
            this.txt_ipaddress.Location = new System.Drawing.Point(116, 48);
            this.txt_ipaddress.Name = "txt_ipaddress";
            this.txt_ipaddress.Size = new System.Drawing.Size(158, 30);
            this.txt_ipaddress.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 25);
            this.label3.TabIndex = 5;
            this.label3.Text = "IP Adress:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 25);
            this.label1.TabIndex = 3;
            this.label1.Text = "Nickname:";
            // 
            // btn_logout
            // 
            this.btn_logout.Location = new System.Drawing.Point(885, 12);
            this.btn_logout.Name = "btn_logout";
            this.btn_logout.Size = new System.Drawing.Size(109, 35);
            this.btn_logout.TabIndex = 40;
            this.btn_logout.Text = "Logout";
            this.btn_logout.UseVisualStyleBackColor = true;
            this.btn_logout.Click += new System.EventHandler(this.btn_logout_Click);
            // 
            // btn_shutdownall
            // 
            this.btn_shutdownall.BackColor = System.Drawing.Color.IndianRed;
            this.btn_shutdownall.ForeColor = System.Drawing.Color.White;
            this.btn_shutdownall.Location = new System.Drawing.Point(831, 510);
            this.btn_shutdownall.Name = "btn_shutdownall";
            this.btn_shutdownall.Size = new System.Drawing.Size(158, 41);
            this.btn_shutdownall.TabIndex = 13;
            this.btn_shutdownall.Text = "Shutdown All";
            this.btn_shutdownall.UseVisualStyleBackColor = false;
            this.btn_shutdownall.Click += new System.EventHandler(this.btn_shutdownall_Click);
            // 
            // btn_shutdown
            // 
            this.btn_shutdown.BackColor = System.Drawing.Color.IndianRed;
            this.btn_shutdown.ForeColor = System.Drawing.Color.White;
            this.btn_shutdown.Location = new System.Drawing.Point(316, 510);
            this.btn_shutdown.Name = "btn_shutdown";
            this.btn_shutdown.Size = new System.Drawing.Size(158, 41);
            this.btn_shutdown.TabIndex = 12;
            this.btn_shutdown.Text = "Shutdown";
            this.btn_shutdown.UseVisualStyleBackColor = false;
            this.btn_shutdown.Click += new System.EventHandler(this.btn_shutdown_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.IndianRed;
            this.button1.ForeColor = System.Drawing.Color.White;
            this.button1.Location = new System.Drawing.Point(572, 510);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(158, 41);
            this.button1.TabIndex = 50;
            this.button1.Text = "Ip ve Ad";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_komut
            // 
            this.txt_komut.Location = new System.Drawing.Point(116, 124);
            this.txt_komut.Name = "txt_komut";
            this.txt_komut.Size = new System.Drawing.Size(158, 30);
            this.txt_komut.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 124);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 25);
            this.label2.TabIndex = 13;
            this.label2.Text = "Komut:";
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.IndianRed;
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(572, 12);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(158, 41);
            this.button2.TabIndex = 51;
            this.button2.Text = "Komut Yolla";
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // panel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.ClientSize = new System.Drawing.Size(1006, 553);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_shutdown);
            this.Controls.Add(this.btn_shutdownall);
            this.Controls.Add(this.btn_logout);
            this.Controls.Add(this.grpbox_devices);
            this.Controls.Add(this.grpbox_adddevice);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "panel";
            this.Text = "Remote Computer Shutdown - Panel";
            this.Load += new System.EventHandler(this.panel_Load);
            this.grpbox_devices.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgrid_devices)).EndInit();
            this.grpbox_adddevice.ResumeLayout(false);
            this.grpbox_adddevice.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpbox_devices;
        private System.Windows.Forms.GroupBox grpbox_adddevice;
        private System.Windows.Forms.Button btn_selectall;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_select;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_logout;
        private System.Windows.Forms.Button btn_shutdownall;
        private System.Windows.Forms.Button btn_shutdown;
        public System.Windows.Forms.TextBox txt_ipaddress;
        public System.Windows.Forms.TextBox txt_nickname;
        public System.Windows.Forms.DataGridView dgrid_devices;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox txt_komut;
        private System.Windows.Forms.Button button2;
    }
}