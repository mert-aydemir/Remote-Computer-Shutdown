using Remote_Computer_Shutdown.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;



namespace Remote_Computer_Shutdown
{
    
    public partial class panel : Form
    {
        
        Sorgu sorgu = new Sorgu();
        //SqlConnection con = new SqlConnection("Data Source=MERT;Initial Catalog=Shutdown_Project;Integrated Security=True");
        //SqlCommand cmd;
        //SqlDataAdapter adapt;



        public panel()
        {
            InitializeComponent();
            
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }
        ////////////////////////////////////////////////////////////////
        private void btn_selectall_Click(object sender, EventArgs e)
        {   
            dgrid_devices.DataSource = sorgu.Select_All();
        }

        private void btn_add_Click(object sender, EventArgs e)
        {   
            sorgu.Insert_Into(txt_ipaddress.Text,txt_nickname.Text);
            sorgu.Insert_Status(txt_ipaddress.Text);
            // Ekleme işleminin yapıldığını göstermek için select all işlemi yapılıyor.
            if (sorgu.isInserted == true)
            {
                dgrid_devices.DataSource = sorgu.Select_All();
                sorgu.isInserted=false;
            }

        }

        private void dgrid_devices_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            sorgu.Delete_Device(txt_ipaddress.Text);
            sorgu.Delete_Status(txt_ipaddress.Text);
            // Silme işleminin gerçekleştiğini göstermek için select all butonunun işlemi yaptırılıyor.
            dgrid_devices.DataSource=sorgu.Select_All();

        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            dgrid_devices.DataSource=sorgu.Select_One(txt_ipaddress.Text);
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            sorgu.Update_Device(txt_ipaddress.Text, txt_nickname.Text);
            // Güncelleme işleminin yapıldığını göstermek için select all işlemi yaptırılıyor.
            dgrid_devices.DataSource = sorgu.Select_All();
        }

        private void dgrid_devices_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_ipaddress.Text = dgrid_devices.CurrentRow.Cells[0].Value.ToString();
            txt_nickname.Text = dgrid_devices.CurrentRow.Cells[1].Value.ToString();
        }

        private void panel_Load(object sender, EventArgs e)
        {
            dgrid_devices.DataSource = sorgu.Select_All();
        }

        private void btn_shutdown_Click(object sender, EventArgs e)
        {
            sorgu.ShutDown();
        }

        private void btn_shutdownall_Click(object sender, EventArgs e)
        {
            string mac = sorgu.Mac();
            MessageBox.Show("Mac adresiniz: " + mac);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sorgu.IpAndName();
            dgrid_devices.DataSource = sorgu.Select_All();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            sorgu.TCPCommunicate(txt_ipaddress.Text, txt_komut.Text);
        }
    }
}
