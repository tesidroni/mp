using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MissionPlanner.Controls;
using MissionPlanner.Utilities;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace MissionPlanner.GCSViews
{
    public partial class Client : MyUserControl, IActivate
    {

        string IP_Address;
        int Port;
        string Message;
        TcpClient tcpclnt;

        public Client()
        {
            InitializeComponent();
        }

        public void Activate()
        {
        }

        private void Client_Load(object sender, EventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            IP_Address = textBox3.Text;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string prov = textBox4.Text;
            Port = Convert.ToInt32(prov);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void myButton1_Click(object sender, EventArgs e)
        {
            Client_funz();
        }

        private void BUT_updatecheck_Click(object sender, EventArgs e)
        {
            tcpclnt.Close();
        }

        private void myButton2_Click(object sender, EventArgs e)
        {
            Message = textBox2.Text;
            String str = Message;
            Stream stm = tcpclnt.GetStream();

            ASCIIEncoding asen = new ASCIIEncoding();
            byte[] ba = asen.GetBytes(str);
            textBox1.Text = "Transmitting.....";

            stm.Write(ba, 0, ba.Length);

            Byte[] bb = new byte[100];
            stm.Read(bb, 0, bb.Length);

            textBox1.Text = System.Text.Encoding.ASCII.GetString(bb, 0, bb.Length);

        }


        public void Client_funz()
        {

            try
            {
                tcpclnt = new TcpClient();
                textBox1.Text = "Connecting...";

                tcpclnt.Connect(IP_Address, Port);
                // use the ipaddress as in the server program

                textBox1.Text = "Connected";
                textBox1.Text = "Enter the string to be transmitted ";

            }

            catch (Exception e)
            {
                textBox1.Text = "Error..... " + e.StackTrace;
            }
        }

        private void mavlinkNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }


    }
}
