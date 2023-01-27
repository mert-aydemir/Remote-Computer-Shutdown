
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
    


    public partial class Form1 : Form
    {
        Sorgu sorgu = new Sorgu();
        panel panel = new panel();
        SqlConnection con = new SqlConnection("Data Source=PCNAME;Initial Catalog=Shutdown_Project;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        public bool LogIn=false;
        


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                
                    string GeciciUser;
                    const int EventId = 1;
                    const int FailedEventId = 2;

                    GeciciUser = txt_id.Text;
            
                    //this.Hide();
                    //panel panel = new panel();
                    //panel.Show();
                    //sorgu.Login(txt_id.Text,txt_password.Text);

                    

                    if (sorgu.Login(txt_id.Text, txt_password.Text) == true)
                    { 
                        LogIn = true;
                        sorgu.LogUser(GeciciUser.ToString(), EventId);
                        this.Hide(); 
                
                
                    }
                    else
                    {
                    if ((String.IsNullOrEmpty(txt_id.ToString()) || (String.IsNullOrEmpty(txt_password.ToString()))))
                    {
                    sorgu.LogUser(GeciciUser.ToString(), FailedEventId);

                    }
                    // Hataya sebebiyet verebiliyor.
                    }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n"+ex.Message.ToString(),"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            
            
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Focus();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();   
        }
    }
}
