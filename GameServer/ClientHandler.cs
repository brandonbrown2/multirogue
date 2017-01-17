using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace GameServer
{
    class ClientHandler
    {
        private NetServer server;
        private int mapSeedValue;

        public ClientHandler (NetServer newServer, int mapSeed)
        {
            server = newServer;
            mapSeedValue = mapSeed;
        }

        public void ClientMessageRecieverThread()
        {
            while (true)
            {
                server.MessageReceivedEvent.WaitOne();

                var msg = server.ReadMessage();
                if (msg.MessageType == NetIncomingMessageType.StatusChanged)
                {
                    if (msg.SenderConnection.Status == NetConnectionStatus.Connected)
                    {
                        NetOutgoingMessage mapSeed = server.CreateMessage();
                        mapSeed.Write("Map Seed Value");
                        mapSeed.Write(mapSeedValue);
                        server.SendMessage(mapSeed, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
        }


    }
}
