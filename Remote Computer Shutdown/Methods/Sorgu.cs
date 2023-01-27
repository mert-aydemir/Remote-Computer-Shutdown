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
using System.Net.NetworkInformation;

namespace Remote_Computer_Shutdown
{
    public  class Sorgu
    {
        SqlConnection con = new SqlConnection("Data Source=PCNAME;Initial Catalog=Shutdown_Project;Integrated Security=True");
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
        public string GeciciMac;
        public string GeciciIp;
        public string GeciciNickname;
        public string GeciciDurum;
        public int GeciciDevice = 0;
        public string[] devices_array;
        public string[] devices_array_online;
        public bool isLoggedIn = false;
        



        public void Insert_Into(string Ip, string Nickname)
        {
            try
            {
                if(!(String.IsNullOrEmpty(Ip) || String.IsNullOrEmpty(Nickname)))
                {
                    cmd = new SqlCommand("insert into Device(Ip,Nickname) values(@Ip,@Nickname)", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Ip", Ip);
                    cmd.Parameters.AddWithValue("@Nickname", Nickname);

                    cmd.ExecuteNonQuery();
                    con.Close();
                    isDeviceInserted = true;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n\nCihaz kaydedilirken Ip ve Nickname bölümleri boş geçilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n"+ex.Message.ToString(), "Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
                con.Close();
            }
            
        }  // Insert into to Devices Table

        public void Insert_Status(string Ip, string Condition="Shutdown", int EventId=5, int FailedEventId = 6)
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
                    Log(Ip, EventId, panel.lbl_user.Text);
                }
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log(Ip, FailedEventId, panel.lbl_user.Text);
                con.Close();
            }

        } // Insert into to Status Table

        public DataTable Select_All(int EventId=13, int FailedEventId=14)
        {
            try
            {
                con.Open();
                DataTable dt = new DataTable();
                adapt = new SqlDataAdapter("select device.Ip , device.Nickname , condition from device , Status where device.Ip=status.Ip;", con);
                adapt.Fill(dt);
                con.Close();
                LogUser(panel.lbl_user.Text, EventId);
                return dt;
            }
            catch (Exception ex)
            {
                con.Close();
                LogUser(panel.lbl_user.Text,FailedEventId);
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n"+ex.Message.ToString(),"Hata",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        } // Refresh

        public void Delete_Device(string Ip)
        {
            try
            {
                if(!(String.IsNullOrEmpty(Ip.ToString())))
                {
                    Listing();
                    if (devices_array.Contains(Ip))
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
                        
                    }

                }
                else
                {
                    throw new ArgumentNullException();
                }
            }

            catch (MissingMemberException nonip)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı\n\n" + "Silmeye çalıştığınız Ip adresi bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


            catch (ArgumentNullException)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n \n" + " Silme işlemi yaparken Ip boş geçilemez.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }

            catch (Exception ex)
            {
                {
                    MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();

                }
            }
        } // Same with the Insert_Into

        public void Delete_Status(string Ip, int EventId=11, int FailedEventId = 12)
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
                    panel.txt_ipaddress.Text = null;
                    panel.txt_nickname.Text = null;
                    MessageBox.Show("Kayıt Silindi");
                    Log(Ip, EventId, panel.lbl_user.Text);

                }

            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log(Ip, FailedEventId, panel.lbl_user.Text);
                con.Close();

            }

        } // Same with the Insert_Status

        public DataTable Select_One(string Ip)
        {

            try
            {
                con.Open();
                DataTable dt = new DataTable();
                adapt = new SqlDataAdapter("select device.Ip,Nickname,condition from device,status where device.Ip = status.Ip and device.Ip=@Ip", con);
                adapt.SelectCommand.Parameters.AddWithValue("@Ip",Ip);
                adapt.Fill(dt);
                con.Close();

                return dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        } // Looking for a specified

        public void Update_Device (string Ip, string Nickname, int EventId = 9, int FailedEventId = 10)
        {
            try
            {
                Listing();
                
                if (!(String.IsNullOrEmpty(Nickname)))
                {
                    
                    
                    if (devices_array.Contains(Ip))
                    {
                        cmd = new SqlCommand("update device set Nickname=@Nickname where Ip=@Ip", con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@Nickname", Nickname);
                        cmd.Parameters.AddWithValue("@Ip", Ip);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        Log(Ip,EventId,panel.lbl_user.Text);
                        MessageBox.Show("Nickname güncellendi.");    
                    }
                    else
                    {
                        //throw new InvalidDataException();
                    }
                    
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (InvalidDataException dataexception)
            {
                Log(Ip, FailedEventId, panel.lbl_user.Text);
                MessageBox.Show("Bir hata ile karşılaşıldı.\nGirdiğiniz Ip bulunamadı.\n\n" + dataexception.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            catch (ArgumentNullException)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\nSorun Nickname bölümünün boş geçilmesinden kaynaklanıyor olabilir.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log(Ip.ToString(),FailedEventId,panel.lbl_user.Text);
                con.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log(Ip.ToString(),FailedEventId,panel.lbl_user.Text);
                con.Close();   
            }
            
        } // Updateting the PC Name

        public bool Login(string Id, string Password)
        {
            try
            {
                
                
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
                    panel.lbl_user.Text = Id.ToString();
                    con.Close();
                    return isLoggedIn;
                    

                
                
                
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return isLoggedIn = false;
            }


        } // Login method. Checking the login necessaries.

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
        } // Getting the MAC Adress. Just here for a new update. Now useless.

        public void IpAndName()
        {
            DateTime bugun = DateTime.Now;            
            string bilgisayarAdi = Dns.GetHostName();
            string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
            string mac = Mac();
            string line_divider = "--------------------------------------------------------";
            MessageBox.Show("Bilgisayarınız adı: " + bilgisayarAdi + "\n" + "Ip Adresiniz: " + ipAdresi + "\n" + "Mac Adresi: " + mac);
            MessageBox.Show(bugun.ToString());
            Insert_Into(ipAdresi,bilgisayarAdi);
            Insert_Status(ipAdresi);
            // ---------- Loglama ----------
            string fileName = @"C:\\Users\\merta\\Desktop\\kapatma_dosyalari\\Logs\\"+bugun.Date.Year.ToString()+ "." + bugun.Date.Month.ToString()+ "." + bugun.Date.Day.ToString() + "." + bugun.Date.Hour.ToString() + "." + bugun.Date.Minute.ToString() + bugun.Date.Second.ToString() + ".txt";

            string ad_log = "Bilgisayarın Adı: "+bilgisayarAdi;
            string ip_log = "Ip Adresi: " + ipAdresi;
            string mac_log = "Mac Adresi: " + mac;
            

            FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            fs.Close();
            File.AppendAllText(fileName, Environment.NewLine + ad_log);
            File.AppendAllText(fileName, Environment.NewLine + ip_log);
            File.AppendAllText(fileName, Environment.NewLine + mac_log);
            File.AppendAllText(fileName, Environment.NewLine + line_divider);


        } // // Getting the IP Adress and PC Name. Just here for a new update. Now useless.

        public void TCPCommunicate(string ipadresi,string komut, int EventId = 23, int FailedEventId = 24, int EventId2 = 27, int FailedEventId2 = 28, int DefaultFail=30)
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
                MessageBox.Show("\n (X) -> Bağlanmaya çalışırken hata oluştu: " + ex.Message);
                Log(ipadresi.ToString(), DefaultFail, panel.lbl_user.Text);
            }

            // Bağlantı sağlandığı sürece her ENTER tuşuna basıldığında
            // ekrana yazılmış olanlar diğer bilgisayara gönderilecek.



            try
            {
                while (true && soket.Connected)
                {
                    // Gönderilecek metni alıyoruz.
                    string gonder = komut;

                    // Ağ üzerinden gönderilecek her şey bytelara 
                    // dönüştürülmüş olmalıdır.
                    soket.Send(Encoding.UTF8.GetBytes(gonder));
                    if (komut.ToString() == "restart")
                        Log(ipadresi.ToString(), EventId2, panel.lbl_user.Text);
                    else if(komut.ToString() =="exit")
                        Log(ipadresi, EventId, panel.lbl_user.Text);
                }
            }
            catch(Exception ex)
            {
                if(komut.ToString() == "restart")
                    Log(ipadresi.ToString(),FailedEventId2, panel.lbl_user.Text);
                else if(komut.ToString()=="exit")
                    Log(ipadresi.ToString(),FailedEventId2, panel.lbl_user.Text);

                MessageBox.Show(ex.Message.ToString());
            }

            //Log(ipadresi.ToString(), EventId, panel.lbl_user.Text);

        } // Sending method. I'm sending the commands to the target device/devices with this method.

        public void TCPListener()
        {
            Socket dinleyiciSoket = new Socket
                (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            const int PORT = 52000;

            // TCP dinleyicisini herhangi bir IPAdresinden gelebilecek
            // bağlantı tekliflerini dinleyecek şekilde ayarlıyoruz.
            // Ayrıca gönderici ile aynı PORT üzerinden dinlemesi gerekiyor.
            TcpListener dinle = new TcpListener(IPAddress.Any, PORT);
            dinle.Start();

            Console.WriteLine("Bağlantı bekleniyor...");

            // Buradan aşağısı bağlantı teklifi geldiğinde çalışacaktır.
            // Teklifi kabul ediyoruz ve dinleyiciSoketimize atayarak
            // kurulmuş TCP bağlantısına her zaman erişilebilecek hale getiriyoruz.
            dinleyiciSoket = dinle.AcceptSocket();
            Console.WriteLine("Bağlantı sağlandı. ");

            while (true)
            {
                try
                {
                    // Receive() metodu TCP bağlantısı üzerinden gelecek mesajları beklemeye ve
                    // geldiğinde okumaya yarar. Gelecek mesajı içinde saklayacağımız bir
                    // buffer yaratarak (gelenData) gelen byteları depoluyoruz.
                    // Oluşturduğumuz buffer'ın boyutu tek seferde alınabilecek byte sayısını
                    // belirlediği için, 256 byte'tan daha uzun bir data gönderildiğinde
                    // tamamını alamayacaktır. Daha uzun veriler göndermek istiyorsanız buffer 
                    // boyutunu daha büyük belirtebilirsiniz.
                    byte[] gelenData = new byte[256];
                    dinleyiciSoket.Receive(gelenData);

                    // Gelen karakterler yarrattığımız bufferın tamamını dolduramazsa (ki genelde
                    // doldurmayacaktır) kalan karakterler boşluk olarak gözükür. Bu sebeple Split()
                    // metodunu kullanarak gelen mesajın sadece metnin bittiği noktaya kadar alınmasını
                    // sağlıyoruz.
                    string mesaj_mac = (Encoding.UTF8.GetString(gelenData)).Split('\0')[0];
                    string mesaj_ip = (Encoding.UTF8.GetString(gelenData)).Split('\0')[0];
                    string mesaj_nickname = (Encoding.UTF8.GetString(gelenData)).Split('\0')[0];
                    string mesaj_durum = (Encoding.UTF8.GetString(gelenData)).Split('\0')[0];

                    Console.WriteLine("Gelen mesaj: " + mesaj_mac);
                    Console.WriteLine("Gelen mesaj: " + mesaj_ip);
                    Console.WriteLine("Gelen mesaj: " + mesaj_nickname);
                    Console.WriteLine("Gelen mesaj: " + mesaj_durum);

                    GeciciMac = mesaj_mac;
                    GeciciIp = mesaj_ip;
                    GeciciNickname = mesaj_nickname;
                    GeciciDurum = mesaj_durum;



                }
                catch
                {
                    // Eğer gönderici program TCP bağlantısı kurulduktan sonra, düzgün bir şekilde
                    // yani exit komutu verilmeden kapatılırsa bir hata oluşacaktır. Hatayı burda
                    // ele alarak programın çökmesini engelliyoruz.
                    Console.WriteLine("Bağlantı kesildi. Çıkış yapılıyor.");
                    break;
                }
            }




            Console.WriteLine("Kapatmak için ENTER'a basın.");

        } // Just here for a new update. Now useless.

        public void isItOnline(string targetip, int EventId = 15, int FailedEventId = 16)
        {
            try
            {
                bool isOnline = false;

                Ping ping = new Ping();
                PingReply reply = ping.Send(targetip);

                if (reply.Status == IPStatus.Success)
                {
                    isOnline = true;
                }

                if (isOnline == true)
                {
                    Console.WriteLine(targetip + " is Online");
                    // veri tabanına update yap
                    string onlinetext = "Online";

                    cmd = new SqlCommand("update Status set condition=@condition where Ip=@Ip", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@condition", onlinetext);
                    cmd.Parameters.AddWithValue("@Ip", targetip);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Log(targetip.ToString(), EventId, panel.lbl_user.Text);
                    Select_All();


                }
                else
                {
                    Console.WriteLine(targetip + " is Offline");
                    // veri tabanına update yap
                    string offlinetext = "Offline";

                    cmd = new SqlCommand("update Status set condition=@condition where Ip=@Ip", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@condition", offlinetext);
                    cmd.Parameters.AddWithValue("@Ip", targetip);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Log (targetip.ToString(), FailedEventId, panel.lbl_user.Text);

                    Select_All();
                }

            } 
            catch (Exception ex)
            {
                Log(targetip.ToString(), EventId, panel.lbl_user.Text);
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n"+ex.Message.ToString(),"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            con.Close();

        } // Checking is specified one online. With sending ping.
        
        public int CountDevices()
        {
            try
            {
            int DeviceCount;

            con.Open();
            SqlCommand cmd = new SqlCommand("select count(*) from Device", con);
            DeviceCount = Convert.ToInt32(cmd.ExecuteScalar());
            con.Close();
            GeciciDevice = DeviceCount;
            return GeciciDevice;
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
                return 0;
            }
        } // Getting how many devices are there in the Device Table.

        public void Listing()
        {
            try
            {
                // Veritabanı bağlantısını aç
                con.Open();
                // Sorgu oluştur ve çalıştır
                SqlCommand cmd = new SqlCommand("select Ip from Device;");
                cmd.Connection = con;

                reader = cmd.ExecuteReader();

                // Okunan değerleri bir List<string> nesnesine ekle
                List<string> devices = new List<string>();
                while (reader.Read())
                {
                    devices.Add(reader["Ip"].ToString());
                }

                // List<string> nesnesini string[] dizisine dönüştür
                devices_array = devices.ToArray();

                // Reader'ı kapat
                reader.Close();

                // devices_array dizisinin 3. elemanını göster
                //MessageBox.Show(devices_array[2].ToString());

                // Veritabanı bağlantısını kapat
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        } // Listing the all IP Adresses in the Device Table.

        public void OnlineListing()
        {
            try
            {
                // Veritabanı bağlantısını aç
                con.Open();
                // Sorgu oluştur ve çalıştır
                SqlCommand cmd = new SqlCommand("select Device.Ip from Device, Status where Condition='online' and Device.Ip=Status.Ip;");
                cmd.Connection = con;

                reader = cmd.ExecuteReader();

                // Okunan değerleri bir List<string> nesnesine ekle
                List<string> devices = new List<string>();
                while (reader.Read())
                {
                    devices.Add(reader["Ip"].ToString());
                }

                // List<string> nesnesini string[] dizisine dönüştür
                devices_array_online = devices.ToArray();

                // Reader'ı kapat
                reader.Close();

                // devices_array dizisinin 3. elemanını göster
                //MessageBox.Show(devices_array[2].ToString());

                // Veritabanı bağlantısını kapat
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                con.Close();
            }
        } // Listing the all [Online] IP Adresses in the Device Table.

        public void isThemOnline(int EventId = 17, int FailedEventId = 18)
        {
            try
            {
            int Cihaz_Sayisi = (int)CountDevices();
            //MessageBox.Show(Cihaz_Sayisi.ToString());

                for (int i = 0; i < Cihaz_Sayisi; i++)
                {
                    bool isOnline = false;
                    //MessageBox.Show(devices_array[2].ToString());
                    string targetip = devices_array[i].ToString();
                

                    Ping ping = new Ping();
                    PingReply reply = ping.Send(targetip);
                
                    if (reply.Status == IPStatus.Success)
                    {
                        isOnline = true;
                    }

                    if (isOnline == true)
                    {
                        Console.WriteLine(targetip + " is Online");
                        // veri tabanına update yap
                        string onlinetext = "Online";

                        cmd = new SqlCommand("update Status set condition=@condition where Ip=@Ip", con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@condition", onlinetext);
                        cmd.Parameters.AddWithValue("@Ip", targetip);
                        cmd.ExecuteNonQuery();
                        con.Close();


                    }
                    else
                    {
                        Console.WriteLine(targetip + " is Offline");
                        // veri tabanına update yap
                        string offlinetext = "Offline";

                        cmd = new SqlCommand("update Status set condition=@condition where Ip=@Ip", con);
                        con.Open();
                        cmd.Parameters.AddWithValue("@condition", offlinetext);
                        cmd.Parameters.AddWithValue("@Ip", targetip);
                        cmd.ExecuteNonQuery();
                        con.Close();

                    }
                    con.Close();
                    //LogUser(panel.lbl_user.Text, EventId);
                }
                panel.dgrid_devices.DataSource = Select_All();
            }
            catch (Exception ex)
            {
                LogUser(panel.lbl_user.Text, FailedEventId);
                con.Close();
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n"+ex.Message.ToString(),"Hata",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
            panel.dgrid_devices.DataSource = Select_All();
        } // Checking are all devices  online. With sending ping.


        public void ShutDownAll(string komut, int EventId=21, int FailedEventId=22)
        {
            try
            {
                int GeciciShutdown = ((int)CountDevices());
                Console.WriteLine(CountDevices().ToString());
                
                for(int i = 0; i < GeciciShutdown; i++)
                {
                
                TCPCommunicate(devices_array_online[i], komut);
                }
                LogUser(panel.lbl_user.Text, EventId);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Dizin, dizi sınırlarının dışındaydı.")
                {
                    MessageBox.Show("Online durumda olan hiçbir bilgisayar bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bir hata ile karşılaşıldı. \n\n"+ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                    LogUser(panel.lbl_user.Text,FailedEventId);

                }
            }
        } //  Sending "exit" command to the all online devices. Working with TCPCommunicate method. Trigger the kapatma_test.bat which means shutdown_test.bat.
        // kapatma_test.bat ---> shutdown /s /f /t 0
        // Probably this method is not working correctly. I think it is not sending the command to the ONLINE devices.
        // I guess it sending the command to the all devices. Didn't check for a while.

        public void RestartAll(string komut, int EventId = 25, int FailedEventId = 26)
        {
            try
            {
                int GeciciShutdown = ((int)CountDevices());
                Console.WriteLine(CountDevices().ToString());
                for (int i = 0; i < GeciciShutdown; i++)
                {

                    TCPCommunicate(devices_array_online[i], komut);
                }
                LogUser(panel.lbl_user.Text, EventId);
            }
            catch (Exception ex)
            {
                if (ex.Message == "Dizin, dizi sınırlarının dışındaydı.")
                {
                    MessageBox.Show("Online durumda olan hiçbir bilgisayar bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Bir hata ile karşılaşıldı. \n\n", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    con.Close();
                    LogUser(panel.lbl_user.Text, FailedEventId);

                }
            }
        } // Same with the ShutDownAll method.
        // It triggers the .bat file whick forcing the system to restart. You can find the .bat file in Client files.

        public void Log(string Ip, int EventId, string Log_User)
        {
           DateTime now = DateTime.Now;
            try
            {
                if (!(String.IsNullOrEmpty(Ip) || String.IsNullOrEmpty(Log_User)))
                {
                    cmd = new SqlCommand("insert into Process_Log(Ip,Event_Id,Log_Time,Log_User) values(@Ip,@EventId,@Log_Time,@Log_User)", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@Ip", Ip);
                    cmd.Parameters.AddWithValue("@EventId", EventId);
                    cmd.Parameters.AddWithValue("@Log_Time", now);
                    cmd.Parameters.AddWithValue("@Log_User", Log_User);

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (ArgumentNullException argument)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n\n"+argument.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUser(panel.lbl_user.Text,29);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUser(panel.lbl_user.Text, 29);
                con.Close();
            }
        } // Logging Method
        // This method for having an IP

        public void LogUser(string User_Id, int EventId)
        {
            DateTime now = DateTime.Now;
            try
            {
                if (! String.IsNullOrEmpty(User_Id))
                {
                    cmd = new SqlCommand("insert into User_Process_Log(User_Id,EventId,Log_Time) values(@User_Id,@EventId,@Log_Time)", con);
                    con.Open();
                    cmd.Parameters.AddWithValue("@User_Id", User_Id);
                    cmd.Parameters.AddWithValue("@EventId", EventId);
                    cmd.Parameters.AddWithValue("@Log_Time", now);

                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    con.Close();
                    throw new ArgumentNullException();
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı. \n\n", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUser(User_Id, EventId);
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata ile karşılaşıldı.\n\n" + ex.Message.ToString(), "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                LogUser(User_Id, EventId);
                con.Close();
            }
        } // Logging Method
        // This method for non-having an IP



        /* EventId and FailedEventsId in methods are just a method for logging. When that method completed
        succesfuly its job, Log methods are using the EventId. And when it didn't complete its job, Log methods
        are using the FailedEventId.

        I added the Database schema and datas in the Database file. You can find them and see what are thoose
        EventId and FailedEventId.  ( Database datas are written in Turkish. So you might not understand them
        quite. You can use the Google Translater. )


        I take no responsibility for any problems the codes may cause. Please carefully read the codes, 
        and make adjustments to suit you before use.

        This is a school project and i do not guarantee of this code is working.

        You can freely use this codes. But i want to say that again : "I take no responsibility for any
        problems the codes may cause. Please carefully read the codes, and make adjustments to suit you before use."

        For more information please read the ReadMe.txt

        */
    }
}
