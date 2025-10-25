using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sem7_2025
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private IPAddress maska2num(uint maskaid)
        {
            uint maskawartosc = 0;
            for (byte i = 0; i <= maskaid; i++)
            {
                maskawartosc |= (uint)(1 << (32 - i));
                if (i == 0) maskawartosc = 0;
            }

            return UInt2IPAdress(maskawartosc);
        }

        private string IPAddressToBin(IPAddress address) {
            byte[] adres = address.GetAddressBytes();

            return string.Join(".", adres.Select(i => Convert.ToString(i, 2).PadLeft(8, '0')));
        }

        private string IPAddress2Class(IPAddress adres)
        {
            uint ip = IPAddressToUint(adres) >> 24 & 0xFF;
            if ((ip & 0B11100000) == 0B11100000) return "Klasa D";
            if ((ip & 0B11000000) == 0B11000000) return "Klasa C";
            if ((ip & 0B10000000) == 0B10000000) return "Klasa B";
            return "Klasa A";
        }

        private uint IPAddressToUint(IPAddress address)
        {
            byte[] __adres = address.GetAddressBytes();
            Array.Reverse(__adres);
            return BitConverter.ToUInt32(__adres, 0);

        }

        private IPAddress UInt2IPAdress(uint address)
        {
            var adres = BitConverter.GetBytes(address);
            Array.Reverse(adres);
            return new IPAddress(adres);

        }

        private string IPAddress2Typ(IPAddress address)
        {
            byte[] ip = address.GetAddressBytes();
            if (
                (ip[0] == 192 && ip[1] == 168) ||
                (ip[0] == 10) ||
                (ip[0] == 172 && ip[1] >= 16 && ip[1] <= 32)
                ) 
            return "Prywatny";
            else return "Publiczny";

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            uint maska = 0;
            // generowanie adresów Maski 0-32
            for (byte i = 0; i <= 32; i++) {
               var maskaIp = maska2num((uint)i);
                comboBox1.Items.Add("(/" + i + ") - " +maskaIp);
            }

            comboBox1.SelectedIndex = 24;
        }

        private void zmiana()
        {
            listView1.Items.Clear();
            try
            {
                var ip = IPAddress.Parse(textBox1.Text);
                var lvip = listView1.Items.Add("Podany Adres");
                lvip.SubItems.Add(ip.ToString());
                lvip.SubItems.Add(IPAddressToBin(ip));// ip to bin

                var maska = maska2num((uint)comboBox1.SelectedIndex);
                var lvmaska = listView1.Items.Add("Wybrana maska");
                lvmaska.SubItems.Add(maska.ToString());
                lvmaska.SubItems.Add(IPAddressToBin(maska));// ip to bin

                uint _adresSici = IPAddressToUint(ip) & IPAddressToUint(maska);
                IPAddress adresSieci = UInt2IPAdress(_adresSici);
                var lvsiec = listView1.Items.Add("Adres sieci");
                lvsiec.SubItems.Add(adresSieci.ToString());
                lvsiec.SubItems.Add(IPAddressToBin(adresSieci));// ip to bin


                uint _adresRozgloszeniowy = _adresSici | ~IPAddressToUint(maska);
                IPAddress adresRozgloszeniowy = UInt2IPAdress(_adresRozgloszeniowy);
                var lvrozgloszeniowy = listView1.Items.Add("Adres rozgłoszeniowy");
                lvrozgloszeniowy.SubItems.Add(adresRozgloszeniowy.ToString());
                lvrozgloszeniowy.SubItems.Add(IPAddressToBin(adresRozgloszeniowy));// ip to bin


                IPAddress mimHost = UInt2IPAdress(_adresSici + 1);
                var lvadmresmin = listView1.Items.Add("Minimalny Adres sieci");
                lvadmresmin.SubItems.Add(mimHost.ToString());
                lvadmresmin.SubItems.Add(IPAddressToBin(mimHost));// ip to bin

                IPAddress maxHost = UInt2IPAdress(_adresRozgloszeniowy - 1);
                var lvadmresmax = listView1.Items.Add("Makrymalny Adres sieci");
                lvadmresmax.SubItems.Add(maxHost.ToString());
                lvadmresmax.SubItems.Add(IPAddressToBin(maxHost));// ip to bin


                // (2^32-maska)-2
                uint liczbaHostow = (uint)Math.Pow(2, (32 - comboBox1.SelectedIndex)) - 2;
                listView1.Items.Add("Ile hostów")
                            .SubItems.Add(liczbaHostow.ToString());

                listView1.Items.Add("Klasa adresowa")
                            .SubItems.Add(IPAddress2Class(ip));

                listView1.Items.Add("Typ adresu")
                            .SubItems.Add(IPAddress2Typ(ip));

            }
            catch (Exception ex) { 
                listView1.Items.Add("info")
                      .SubItems.Add("Podałeś zły adres ip");
            }
        }

        private void zmiana_ip(object sender, EventArgs e)
        {
            zmiana();
        }

        private void zmiana_maski(object sender, EventArgs e)
        {
            zmiana();
        }
    }
}
