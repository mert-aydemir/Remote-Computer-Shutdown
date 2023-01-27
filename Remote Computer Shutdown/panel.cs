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
        SqlConnection con = new SqlConnection("Data Source=PCNAME;Initial Catalog=Shutdown_Project;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;


        public panel()
        {
            InitializeComponent();
            
        }

        private void btn_logout_Click(object sender, EventArgs e)
        {
            try
            {
                Form1 form1 = new Form1();
                sorgu.isLoggedIn = false;
                this.Hide();
                form1.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n"+ex.Message.ToString(),"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
        ////////////////////////////////////////////////////////////////
        private void btn_selectall_Click(object sender, EventArgs e)
        {
            try
            {
            dgrid_devices.DataSource = sorgu.Select_All();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);            
            }
            
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
            try
            {
                DialogResult result = MessageBox.Show("Silme işlemini gerçekleştirmek istediğinizden emin misiniz?", "Emin misiniz?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    sorgu.Delete_Device(txt_ipaddress.Text);
                    sorgu.Delete_Status(txt_ipaddress.Text);
                    // Silme işleminin gerçekleştiğini göstermek için select all butonunun işlemi yaptırılıyor.
                    dgrid_devices.DataSource=sorgu.Select_All();
                                
                }
                else
                {

                }

            }

            catch (MissingMemberException nonip)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı\n\n" + "Silmeye çalıştığınız Ip adresi bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //catch(DataException )

            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı\n\n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            

        }

        private void btn_select_Click(object sender, EventArgs e)
        {
            dgrid_devices.DataSource=sorgu.Select_One(txt_ipaddress.Text);
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult updateresult = MessageBox.Show("Güncellemek istediğinizden emin misiniz?", "Emin misiniz?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
                if(updateresult == DialogResult.Yes)
                {
                    sorgu.Update_Device(txt_ipaddress.Text, txt_nickname.Text);
                    // Güncelleme işleminin yapıldığını göstermek için select all işlemi yaptırılıyor.
                    dgrid_devices.DataSource = sorgu.Select_All();

                }
                else
                {

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı..\n\n"+ex.Message.ToString(),"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void dgrid_devices_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_ipaddress.Text = dgrid_devices.CurrentRow.Cells[0].Value.ToString();
            txt_nickname.Text = dgrid_devices.CurrentRow.Cells[1].Value.ToString();
        }

        private void panel_Load(object sender, EventArgs e)
        {
            txt_ipaddress.Focus();
            dgrid_devices.DataSource = sorgu.Select_All();
        }

        private void btn_shutdown_Click(object sender, EventArgs e)
        {
            int Cihaz_Sayisi = (int)sorgu.CountDevices();
            MessageBox.Show("Toplam cihaz sayısı:\n\n"+ Cihaz_Sayisi.ToString(),"Count Devices",MessageBoxButtons.OK,MessageBoxIcon.Information);
            sorgu.Listing();   
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
            //sorgu.TCPCommunicate(txt_ipaddress.Text, sorgu.AdminIp().ToString());
            sorgu.TCPCommunicate(txt_ipaddress.Text, cmb_komut.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sorgu.isItOnline(txt_ipaddress.Text);
            dgrid_devices.DataSource= sorgu.Select_All();
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            const int EventId = 17;
            const int FailedEventId = 18;
            try
            {
                sorgu.Listing();
                sorgu.isThemOnline();
                sorgu.LogUser(lbl_user.Text, EventId);
            }
            catch (Exception ex)
            {
                sorgu.LogUser(lbl_user.Text, FailedEventId);
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            sorgu.OnlineListing();
            sorgu.ShutDownAll("exit");
        }

        private void btn_restartall_Click(object sender, EventArgs e)
        {
            sorgu.OnlineListing();
            sorgu.RestartAll("restart");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            sorgu.isLoggedIn = false;
            Application.Exit();
        }
    }
}
