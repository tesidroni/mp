using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class TCPServerTest
    {

        public string ipAddr { get; set; }
        public int port { get; set; }

        TcpListener server = null;

        public string logText { get; set; }
        public string receivedData { get; set; }


        //costruttore
        public TCPServerTest(string ip, int port)
        {
            ipAddr = ip;
            this.port = port;
        }


        //crea un server e si mette in ascolto
        public void startServer()
        {
            logText = "Avvio server in corso...\n";
            logText = "IP = " + ipAddr + ":" + port.ToString();

            IPAddress localAddr = IPAddress.Parse(ipAddr);

            server = new TcpListener(localAddr, port);

            //fa partire server
            server.Start();

            Byte[] bytes = new Byte[1024];
            String data = null;

            //loop di ascolto
            while (true)
            {

                Console.WriteLine("aspetto connessione...");
                logText += "aspetto connessione...";

                //chiamata blocking fino all arrivo di una connessione
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connesso.");
                logText += "Connesso\n";


                NetworkStream stream = client.GetStream();

                int i;
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    //converte dati binari in stringa ASCII
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                    //stampa dati ricevuti
                    Console.WriteLine("Dati ricevuti: {0}", data);
                    receivedData = data;

                    byte[] responseMsg = System.Text.Encoding.ASCII.GetBytes("Ho ricevuto i tuoi dati.");

                    stream.Write(responseMsg, 0, responseMsg.Length);
                    Console.WriteLine("Risposta inviata: {0}", responseMsg.ToString());
                    logText += "Risposta inviata: " + responseMsg.ToString();
                }

                //chiusura connessione
                //...

            }


        } //void startServer

    }
}
