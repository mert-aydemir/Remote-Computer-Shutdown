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

namespace TCP_Listener
{
    internal class MainCommands
    {

        // HENÜZ HAZIR DEĞİLDİR
        
        static Socket soket = new Socket
            (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        const int PORT = 52000;
        string macadress;
        string ipadress;
        string pcname;
        string pcmode;

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

                    Console.WriteLine("Gelen mesaj: " + mesaj);

                    if (mesaj.ToLower().StartsWith("exit"))
                    {
                        //string bilgisayarAdi = Dns.GetHostName();

                        //// Eğer mesaj 'exit' şeklinde başlıyorsa, bağlantıyı düzgün bir şekilde kapatıyoruz.
                        //Console.WriteLine("Bağlantı kapatılıyor.");
                        //System.Diagnostics.Process.Start("C:\\Program Files\\Mert Aydemir\\Client Version 1.0\\kapatma_test.bat");
                        ////Process.Start("cmd", "/ K shutdown / s");
                        //dinleyiciSoket.Dispose();
                        Console.WriteLine("Hata");



                        break;
                    }
                    else
                    {
                        for(int i = 0; i < 2; i++)
                        {
                            string gecici = mesaj;
                            System.Diagnostics.Process.Start("C:\\Users\\merta\\Desktop\\kapatma_dosyalari\\ipconfig test.bat");
                            break;
                        }
                    }
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

        }


    }
}
