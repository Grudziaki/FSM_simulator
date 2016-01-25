using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace FSM_Simulator
{
    public class Listener
    {
        UdpClient udpServer = new UdpClient();
        public void CreateServer()
        {
            try
            {
                IPEndPoint localpt = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);

                udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpServer.Client.Bind(localpt);   
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception 3 : " + e);
            }
        }
        public void StartListening()
        {
            this.udpServer.BeginReceive(Receive, new object());
        }
        public void Receive(IAsyncResult ar)
        {
            try
            {
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
                byte[] bytes = udpServer.EndReceive(ar, ref ip);
                string message = Encoding.ASCII.GetString(bytes);
                Message msg = (Message)Utils.DeserializeObject(message, typeof(Message));
                if ((msg.from != Form1.FSM_name) && (msg.type_of_information == true))
                {
                    Utils.message_handler(msg);
                    MessageBox.Show("otrzymano od: " + msg.from + " sygnał: " + msg.signal);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("błąd wysyłania: " + e);
            }
            StartListening();
            Form1.listen = false;
        }
        public void Send(Message msg)
        {
            try
            {
                UdpClient client = new UdpClient();
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 6000);
                string str = Utils.SerializeObject(msg);
                byte[] bytes = Encoding.ASCII.GetBytes(str);
                client.Send(bytes, bytes.Length, ip);
                client.Close();
                MessageBox.Show("Wyslano");
                //StartListening();
            }
            catch (Exception e)
            {
                MessageBox.Show("błąd wysyłania: " + e);
            }
        }
    }
}
