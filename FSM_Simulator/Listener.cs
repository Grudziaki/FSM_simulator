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
                IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 2222);
                IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
                udpServer.JoinMulticastGroup(multicastaddress);
                udpServer.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                udpServer.Client.Bind(localEp);   
                
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
                byte[] serverReadBuffer = new byte[16384];
                IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 2222);
                Byte[] data = udpServer.EndReceive(ar, ref localEp);

                string message = Encoding.UTF8.GetString(data);
                Message msg = (Message)Utils.DeserializeObject(message, typeof(Message));
                if ((msg.from != Form1.FSM_name) && (msg.type_of_information == true))
                {
                    Utils.message_handler(msg);
                    MessageBox.Show("otrzymano od: " + msg.from + " sygnał: " + msg.signal);
                }

            }
            catch (Exception e)
            {
                MessageBox.Show("błąd odbierania: " + e);
            }
            StartListening();
        }
        public void Send(Message msg)
        {
            try
            {
                UdpClient udpclient = new UdpClient();
                IPAddress multicastaddress = IPAddress.Parse("239.0.0.222");
                IPEndPoint remoteep = new IPEndPoint(multicastaddress, 2222);

                Byte[] buffer = null;

                string str = (string)Utils.SerializeObject(msg);
                buffer = Encoding.UTF8.GetBytes(str);
                udpclient.Send(buffer, buffer.Length, remoteep);
                udpclient.Close();

                //StartListening();
            }
            catch (Exception e)
            {
                MessageBox.Show("błąd wysyłania: " + e);
            }
        }
    }
}
