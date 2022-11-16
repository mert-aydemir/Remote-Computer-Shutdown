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
        SqlConnection con = new SqlConnection("Data Source=MERT;Initial Catalog=Shutdown_Project;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //this.Hide();
            //panel panel = new panel();
            //panel.Show();
            //sorgu.Login(txt_id.Text,txt_password.Text);
            if (sorgu.Login(txt_id.Text, txt_password.Text) == true) { this.Hide(); }
            
            

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Focus();
        }
    }
}
