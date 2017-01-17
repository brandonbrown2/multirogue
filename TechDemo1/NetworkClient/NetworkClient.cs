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
            connection.Connect(ip, port);

            clientReciever = new Thread(this.ClientRecieverThread);
            clientReciever.Start();
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
