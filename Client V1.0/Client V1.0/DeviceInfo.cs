using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Net.Sockets;
using Microsoft.Win32;
using System.Net.NetworkInformation;


namespace Client_V1._0
{   

    internal class DeviceInfo
    {

        SqlConnection con = new SqlConnection("Data Source=MERT;Initial Catalog=Shutdown_Project;Integrated Security=True");
        SqlCommand cmd;
        SqlDataAdapter adapt;
        SqlDataReader reader;



        static Socket soket = new Socket
             (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        const int PORT = 52000;

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
        } // Mac Adresini tespit ediyor.
        // Getting the MAC Adress. Just here for a new update. Now useless.
        public string IpAdress()
        {
            string bilgisayarAdi = Dns.GetHostName();
            string ipAdresi = Dns.GetHostByName(bilgisayarAdi).AddressList[0].ToString();
            return ipAdresi;
        } // Ip adresi tespiti yapıyor.
        // Getting the IP Adress. Just here for a new update. Now useless.
        public string PcName()
        {
            string bilgisayarAdi = Dns.GetHostName();
            return bilgisayarAdi;
        } // Cihazın adını tespit ediyor. ( Clean Code'a uymak için adı ve ip adresi ayrı fonksiyonlardan çekiliyor. )
        // Getting the PC Name. Just here for a new update. Now useless.
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
                    string mesaj = (Encoding.UTF8.GetString(gelenData)).Split('\0')[0];

                    Console.WriteLine("Gelen komut: " + mesaj);

                    if (mesaj.ToLower().StartsWith("exit"))
                    {
                        string bilgisayarAdi = Dns.GetHostName();

                        // Eğer komut 'exit' şeklinde başlıyorsa,
                        // bilgisayarı kapatmamızı sağlayan bat dosyasını çalıştırıyoruz ve
                        // bağlantıyı düzgün bir şekilde kapatıyoruz.
                        Console.WriteLine("Bağlantı kapatılıyor.");
                        System.Diagnostics.Process.Start("C:\\Program Files\\Mert Aydemir\\Client Version 1.0\\kapatma_test.bat");
                        dinleyiciSoket.Dispose();
                        break;
                    }

                    else if (mesaj.ToLower().StartsWith("restart"))
                    {
                        // Eğer komut 'restart' şeklinde başlıyorsa,
                        // bilgisayarı tekrar başlatmamızı sağlayan bat dosyasını çalıştırıyoruz ve
                        // bağlantıyı düzgün bir şekilde kapatıyoruz.
                        Console.WriteLine("Bağlantı kapatılıyor.");
                        System.Diagnostics.Process.Start("C:\\Program Files\\Mert Aydemir\\Client Version 1.0\\restart_test.bat");
                        dinleyiciSoket.Dispose();
                        break;
                    }
                    else
                    {
                        // Gelen komut, spesifik olarak belirtilen komutlardan biri değilse hiçbir işlem yapmayacak.
                    }
                }
                catch
                {
                    // Eğer gönderici program TCP bağlantısı kurulduktan sonra, düzgün bir şekilde
                    // yani exit veya restart komutu verilmeden kapatılırsa bir hata oluşacaktır. Hatayı burda
                    // ele alarak programın çökmesini engelliyoruz.
                    Console.WriteLine("Bağlantı kesildi. Çıkış yapılıyor.");
                    break;
                }
            }




            Console.WriteLine("Kapatmak için ENTER'a basın.");

        } // A TCP listener for the commands are coming from main program.
        // Waiting commands and doing the actions for what commands is comming.
        // Trigger the .bat files for restart and shutdown actions.
        public void TCPCommunicate(string mac, string ip, string nickname)
        {
            Program program = new Program();
            const string uzakBilgisayarIP = ""; // Buraya ana bilgisayarın Ip adresi yazılmalıdır.
                                                // Ip adresi const olduğu için değişmemelidir.
                                                // Değişirse program çökebilir.
                                                // Tüm işlevlerini yaptığı taktirde bu çökmeyi engelleyeceğim.




            // Dinleyici programımızın çalıştığı
            // uzak bilgisayarın IP adresini isteyelim:

            

            Console.WriteLine(uzakBilgisayarIP + " adresindeki bilgisayara bağlanılıyor...");

            // Uzak bilgisayara bağlanmaya çalışalım
            try
            {
                soket.Connect(new IPEndPoint(IPAddress.Parse(uzakBilgisayarIP), PORT));
                Console.WriteLine("Başarıyla bağlanıldı!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("\n (X) -> Bağlanmaya çalışırken hata oluştu: " + ex.Message);
            }

            // Bağlantı sağlandığı sürece her ENTER tuşuna basıldığında
            // ekrana yazılmış olanlar diğer bilgisayara gönderilecek.



            try
            {
                while (true && soket.Connected)
                {


                    // Gönderilecek metni alıyoruz.
                    string gonder_ip = ip;
                    // Ağ üzerinden gönderilecek her şey bytelara 
                    // dönüştürülmüş olmalıdır.
                    soket.Send(Encoding.UTF8.GetBytes(gonder_ip));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            try
            {
                while (true && soket.Connected)
                {


                    // Gönderilecek metni alıyoruz.
                    string gonder_durum = "Online";

                    // Ağ üzerinden gönderilecek her şey bytelara 
                    // dönüştürülmüş olmalıdır.
                    soket.Send(Encoding.UTF8.GetBytes(gonder_durum));

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }


        } // Bakımda
                                                                              // ***********************************
                                                                              // Projenin ileriki aşamasında
                                                                              // yapmayı planladığım method burada
                                                                              // duruyor. Şu anda bir işlevi yok.
                                                                              // İleride report komutu ile bilgisayardan
                                                                              // topladığım verileri veritabanına
                                                                              // aktarma işlemi yaparken kullanacağım.
                                                                              // Just here for a new update. Now useless.


                                                                              /* This console app is hidden. Go to the
                                                                              project settings and see that i changed this
                                                                              project type to a Form App so this is not
                                                                              showing while working.
                                                                              */
    }
}
