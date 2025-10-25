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

        private IPAddress num2maska(uint maskaid)
        {
            uint maskawartosc = 0;
            for (byte i = 0; i <= maskaid; i++)
            {
                maskawartosc |= (uint)(1 << (32 - i));
                if (i == 0) maskawartosc = 0;
            }
            var tabBajtow = BitConverter.GetBytes(maskawartosc);
            Array.Reverse(tabBajtow);
            return new IPAddress(tabBajtow);
        }

        private string IPAddressToBin(IPAddress address) {
            byte[] adres = address.GetAddressBytes();

            return string.Join(".", adres.Select(i => Convert.ToString(i, 2).PadLeft(8, '0')));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            uint maska = 0;
            // generowanie adresów Maski 0-32
            for (byte i = 0; i <= 32; i++) {
               var maskaIp = num2maska((uint)i);
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

                var maska = num2maska((uint)comboBox1.SelectedIndex);
                var lvmaska = listView1.Items.Add("Wybrana maska");
                lvmaska.SubItems.Add(maska.ToString());
                lvmaska.SubItems.Add(IPAddressToBin(maska));// ip to bin
                
                // (2^32-maska)-2
                uint liczbaHostow = (uint)Math.Pow(2, (32 - comboBox1.SelectedIndex)) - 2;
                listView1.Items.Add("Ile hostów")
                            .SubItems.Add(liczbaHostow.ToString());

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
