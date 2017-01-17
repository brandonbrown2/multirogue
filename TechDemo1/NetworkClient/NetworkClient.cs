using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;

namespace TechDemo1.NetworkClient
{
    class NetworkClient
    {
        private NetClient connection;
        private Thread clientReciever;
        public delegate void seedReceivedHandler(object sender, int seed);
        public seedReceivedHandler seedReceived;

        public void ConnectToServer(string ip, int port)
        {
            var config = new NetPeerConfiguration("Multirogue Client");
            connection = new NetClient(config);
            connection.Start();
            connection.Connect(host: ip, port : port);

            clientReciever = new Thread(this.ClientRecieverThread);
        }

        private void ClientRecieverThread()
        {
            while (true) {
                connection.MessageReceivedEvent.WaitOne();

                var msg = connection.ReadMessage();
                if (msg.MessageType == NetIncomingMessageType.Data)
                {
                    string packetType = msg.ReadString();
                    if (packetType == "Map Seed Value")
                    {
                        seedReceived(this, msg.ReadInt32());
                    }
                }
            }
        }
    }
}
