using Remote_Computer_Shutdown.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace Remote_Computer_Shutdown
{
    public  class Sorgu
    {



       
        SqlConnection con = new SqlConnection("Data Source=MERT;Initial Catalog=Shutdown_Project;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader reader;
        public static Form1 form1 = new Form1();
        public static panel panel = new panel();
        static Socket soket = new Socket
             (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        const int PORT = 52000;
        public bool isInserted = false;
        public bool isDeviceInserted = false;
        public bool isDeleteStarted = false;



        public void Insert_Into(string Ip, string Nickname)
        {
            try
            {
                cmd = new SqlCommand("insert into Device(Ip,Nickname) values(@Ip,@Nickname)", con);
                con.Open();

                cmd.Parameters.AddWithValue("@Ip", Ip);
                cmd.Parameters.AddWithValue("@Nickname", Nickname);

                cmd.ExecuteNonQuery();
                con.Close();
                isDeviceInserted = true;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n"+ex.Message.ToString(), "Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                con.Close();
            }
            
        }

        public void Insert_Status(string Ip, string Condition="Shutdown")
        {
            try
            {
                if (isDeviceInserted == true)
                {
                    cmd = new SqlCommand("insert into Status(Ip,Condition) values(@Ip,@Condition)", con);
                    con.Open();

                    cmd.Parameters.AddWithValue("@Ip", Ip);
                    cmd.Parameters.AddWithValue("@Condition", Condition);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    isInserted = true;
                    isDeviceInserted=false;
                    MessageBox.Show("Kayıt Başarılı!");
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

        }

        public DataTable Select_All()
        {
            
            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select device.Ip , device.Nickname , condition from device , Status where device.Ip=status.Ip;", con);
            adapt.Fill(dt);
            con.Close();

            return dt;
        }

        public void Delete_Device(string Ip)
        {
            try
            {
                if(!(String.IsNullOrEmpty(Ip.ToString())))
                {
                    cmd = new SqlCommand("delete Device where Ip=@Ip", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Ip", Ip);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    isDeleteStarted = true;

                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToString() == "'System.Exception' türünde özel durum oluşturuldu.")
                {
                    MessageBox.Show("Bir hata ile karşılaşıldı. \n \n" + " Silme işlemi yaparken Ip boş geçilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                }
                else
                {
                    MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();

                }
            }
        }

        public void Delete_Status(string Ip)
        {
            try
            {
                if (isDeleteStarted == true)
                {
                    cmd = new SqlCommand("delete Status where Ip=@Ip", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Ip", Ip);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    isDeleteStarted=false;
                    MessageBox.Show("Kayıt Silindi");

                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();

            }

        }

        public DataTable Select_One(string Ip)
        {

            con.Open();
            DataTable dt = new DataTable();
            adapt = new SqlDataAdapter("select device.Ip,Nickname,condition from device,status where device.Ip = status.Ip and device.Ip=@Ip", con);
            adapt.SelectCommand.Parameters.AddWithValue("@Ip",Ip);
            adapt.Fill(dt);
            con.Close();

            return dt;
        }

        public void Update_Device (string Ip, string Nickname)
        {
            try
            {
                if (!(String.IsNullOrEmpty(Nickname)))
                {
                    cmd = new SqlCommand("update device set Nickname=@Nickname where Ip=@Ip", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Nickname", Nickname);
                    cmd.Parameters.AddWithValue("@Ip", Ip);
                    cmd.ExecuteNonQuery();
                    con.Close();

                    MessageBox.Show("Nickname güncellendi.");
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        }

        public bool Login(string Id, string Password)
        {
            bool isLoggedIn = false;
            con.Open();
             
            cmd = new SqlCommand("select * from Login where Id=@Id and Password=@Password;");
            cmd.Connection = con;
            cmd.Parameters.AddWithValue("@Id", Id);
            cmd.Parameters.AddWithValue("@Password", Password);
            reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                
                
                MessageBox.Show("Giriş yapıldı.","Giriş Başarılı",MessageBoxButtons.OK,MessageBoxIcon.Asterisk);
                panel.Show();
                form1.Hide();           
                isLoggedIn=true;
                
                
                
                Application.DoEvents();
                
                
                
            }
            else
            {
                MessageBox.Show("Kullanıcı Id veya şifre yanlış.", "Giriş Başarısız", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isLoggedIn=false;
            }
            con.Close();
            return isLoggedIn;
        }


        public void ShutDown()
        {

            //System.Diagnostics.Process proc = new System.Diagnostics.Process();
            //proc.EnableRaisingEvents = false;
            //proc.StartInfo.FileName = "shutdown.exe";
            //proc.StartInfo.Arguments = @"\\merta /t:10 ""The computer is shutting down"" /y /c";
            //proc.Start();

            //Process.Start("shutdown.exe", "-s \\172.19.240.53 -t 00");
            //Process.Start(@"kapatma_test.bat");


            System.Diagnostics.Process.Start("C:\\Users\\merta\\Desktop\\kapatma_dosyalari\\kapatma_test.bat");

        }

        public string Mac()
        {
            ManagementClass manager = new ManagementClass("Win32_NetworkAdapterConfiguration");
            foreach (ManagementObject obj in manager.GetInstances())
            {
                if ((bool)obj["IPEnabled"])
                {
                    return obj["MacAddress"].ToString();
                }
            }

            return String.Empty;
        }

        public void IpAndName()
        {
            DateTime bugun = DateTime.Now;
            
            string bilgisayarAdi = Dns.GetHostName();
            string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
            MessageBox.Show("Bilgisayarınız adı: " + bilgisayarAdi + " Ip Adresiniz: " + ipAdresi);
            MessageBox.Show(bugun.ToString());
            Insert_Into(ipAdresi,bilgisayarAdi);
            Insert_Status(ipAdresi);
            // ---------- Loglama ----------
            string fileName = @"C:\\Users\\merta\\Desktop\\kapatma_dosyalari\\Logs\\"+bugun.Date.Year.ToString()+ "." + bugun.Date.Month.ToString()+ "." + bugun.Date.Day.ToString() + "." + bugun.Date.Hour.ToString() + ".txt";

            string ad_log = "Bilgisayarın Adı: "+bilgisayarAdi;
            string ip_log = "Ip Adresi: " + ipAdresi;
            

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Close();
            File.AppendAllText(fileName, Environment.NewLine + ad_log);
            File.AppendAllText(fileName, Environment.NewLine + ip_log);

        }

        public void TCPCommunicate(string ipadresi,string komut)
        {
            string uzakBilgisayarIP = "";




            // Dinleyici programımızın çalıştığı
            // uzak bilgisayarın IP adresini isteyelim:

            uzakBilgisayarIP = ipadresi;

            Console.WriteLine(uzakBilgisayarIP + " adresindeki bilgisayara bağlanılıyor...");

            // Uzak bilgisayara bağlanmaya çalışalım
            try
            {
                soket.Connect(new IPEndPoint(IPAddress.Parse(uzakBilgisayarIP), PORT));
                MessageBox.Show("Başarıyla bağlanıldı!");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                MessageBox.Show("\n (X) -> Bağlanmaya çalışırken hata oluştu: " + ex.Message);
            }

            // Bağlantı sağlandığı sürece her ENTER tuşuna basıldığında
            // ekrana yazılmış olanlar diğer bilgisayara gönderilecek.
            while (true && soket.Connected)
            {
                

                // Gönderilecek metni alıyoruz.
                string gonder =komut;

                // Ağ üzerinden gönderilecek her şey bytelara 
                // dönüştürülmüş olmalıdır.
                soket.Send(Encoding.UTF8.GetBytes(gonder));
            }



        }



    }
}
