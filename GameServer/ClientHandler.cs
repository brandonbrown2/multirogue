using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace GameServer
{
    class ClientHandler
    {
        private NetServer server;
        private int mapSeedValue;
        private Simulation game;
        private IDictionary<NetConnection, int> PlayerDictionary;
        private int playerCount;

        public ClientHandler (NetServer newServer, Simulation newGame)
        {
            server = newServer;
            game = newGame;
            mapSeedValue = game.getMapSeed();
            playerCount = 0;
            PlayerDictionary = new Dictionary<NetConnection, int>();
        }

        public void ClientMessageRecieverThread()
        {
            while (true)
            {
                server.MessageReceivedEvent.WaitOne();

                var msg = server.ReadMessage();
                if (msg.MessageType == NetIncomingMessageType.StatusChanged)
                {
                    System.Console.WriteLine("Client associated " + msg.SenderConnection.Status);
                    if (msg.SenderConnection.Status == NetConnectionStatus.Connected)
                    {
                        PlayerDictionary.Add(msg.SenderConnection, playerCount);
                        Point start = game.spawnNewPlayer(playerCount);
                        NetOutgoingMessage mapSeed = server.CreateMessage();
                        mapSeed.Write("Map Seed Value");
                        mapSeed.Write(mapSeedValue);
                        mapSeed.Write("Local Player Start");
                        mapSeed.Write(playerCount);
                        mapSeed.Write(start.X);
                        mapSeed.Write(start.Y);
                        playerCount = playerCount++;
                        server.SendMessage(mapSeed, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    }
                }
                else if (msg.MessageType == NetIncomingMessageType.Data)
                {
                    string packetType = msg.ReadString();
                    if(packetType == "Load Entities")
                    {
                        NetOutgoingMessage response = server.CreateMessage();
                        game.GenerateEntityList(response);
                        server.SendMessage(response, msg.SenderConnection, NetDeliveryMethod.ReliableOrdered);
                    }
                }
            }
        }


    }
}
