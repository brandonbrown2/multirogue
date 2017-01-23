﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Lidgren.Network;
using Microsoft.Xna.Framework;

namespace TechDemo1.NetworkClient
{
    class NetworkClient
    {
        private NetClient connection;
        private Thread clientReciever;
        public delegate void seedReceivedHandler(object sender, int seed);
        public seedReceivedHandler seedReceived;
        public delegate void playerLocalSpawnHandler(object sender, int entityID, Point spawnLocation);
        public playerLocalSpawnHandler spawnMe;
        public delegate void setGameTypeHandler(object sender, bool status);
        public setGameTypeHandler gameType;

        public void ConnectToServer(string ip, int port)
        {
            var config = new NetPeerConfiguration("Multirogue");
            connection = new NetClient(config);
            clientReciever = new Thread(this.ClientRecieverThread);
            clientReciever.Start();
            connection.Start();
            connection.Connect(ip, port);
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
                        gameType(this, false);
                        int mapSeed = msg.ReadInt32();
                        msg.ReadString();
                        spawnMe(this, msg.ReadInt32(), new Point(msg.ReadInt32(), msg.ReadInt32()));
                        seedReceived(this, mapSeed);
                    }
                }
            }
        }
    }
}
