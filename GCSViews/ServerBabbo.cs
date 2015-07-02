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
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TesiDroni;

namespace MissionPlanner.GCSViews
{
    public partial class ServerBabbo : MyUserControl, IActivate
    {

        //TCPServerTest myserver;
        System.Threading.Timer timer;
        System.Threading.Timer timer2;


        Byte[] bytes;
        String data;

        MultiThreadServer mtserver;

        //aggiorna i dati a schermo con quelli dell oggetto TCPServerTest
        private void readReceivedData() {

            textBox2.Text = data.ToString();

        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            //ricava indirizzo ip locale del pc
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            textBox3.Text = localIP;
        }

        public string ipAddr { get; set; }
        public int port { get; set; }

        TcpListener server = null;

        public string logText { get; set; }
        public string receivedData { get; set; }



        //crea un server e si mette in ascolto
        public void startServer(string ip, int port)
        {
            ipAddr = ip;
            this.port = port;

            textBox5.Text = "Avvio server in corso...\n";
            textBox5.Text = "IP = " + ipAddr + ":" + port.ToString();
            Console.WriteLine("IP = " + ipAddr + ":" + port.ToString());

            IPAddress localAddr = IPAddress.Parse(ipAddr);

            server = new TcpListener(localAddr, port);

            //fa partire server
            server.Start();

            bytes = new Byte[1024];
            data = null;

            //INIZIO NUOCO THREAD
            new Thread(() => 
            {


            //loop di ascolto
            while (true)
            {

                Console.WriteLine("aspetto connessione...");

                //cambia variabile del thread originario dal nuovo thread
                if (textBox5.InvokeRequired) //Is this method being called from a different thread
                    this.Invoke(new MethodInvoker(() => textBox5.Text = "aspetto connessione..."));
                //textBox5.Text += "aspetto connessione...";

                //chiamata blocking fino all arrivo di una connessione
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connesso.");
                if (textBox5.InvokeRequired) //Is this method being called from a different thread
                    this.Invoke(new MethodInvoker(() => textBox5.Text = "Connesso."));
                //textBox5.Text += "Connesso\n";


                NetworkStream stream = client.GetStream();

                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    //converte dati binari in stringa ASCII
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    //stampa dati ricevuti
                    Console.WriteLine("Dati ricevuti: {0}", data);
                    if (textBox2.InvokeRequired) //Is this method being called from a different thread
                        this.Invoke(new MethodInvoker(() => textBox2.Text = data.ToString()));
                    //receivedData = data;
                    //textBox2.Text = data;

                    byte[] responseMsg = System.Text.Encoding.ASCII.GetBytes("Ho ricevuto i tuoi dati.");

                    stream.Write(responseMsg, 0, responseMsg.Length);
                    Console.WriteLine("Risposta inviata: {0}", responseMsg.ToString());
                    if (textBox5.InvokeRequired) //Is this method being called from a different thread
                        this.Invoke(new MethodInvoker(() => textBox5.Text = "Risposta inviata."));
                    //textBox5.Text += "Risposta inviata: " + responseMsg.ToString();
                } 

                //chiusura connessione
                //...

            }  //while true

            }).Start(); //fine nuovo thread


            ///FINE NUOVCO THREAD

        }
        /// <summary>
        /// ////////////////
        /// </summary>


        public ServerBabbo()
        {
            InitializeComponent();


        }

        public void Activate()
        {
        }

        private void Server_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void BUT_updatecheck_Click(object sender, EventArgs e)
        {
            /* textBox5.Text = "Server avviato.";

           //update ogni deltaT ms
           int deltaT = 100;

           startServer(ipText.Text, Convert.ToInt32(portText.Text)); */

            //CODICE MT SERVER
            mtserver = new MultiThreadServer();

            mtserver.startMTServer();
            
        }



    }
}
