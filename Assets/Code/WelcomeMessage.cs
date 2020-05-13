﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Networking.Transport;

namespace Assets.Code
{
    public class WelcomeMessage : Message
    {
        public int PlayerID { get; set; }
        public uint Colour { get; set; }

        public WelcomeMessage()
        {
            Type = MessageType.Welcome;
        }

        public override void SerializeObject(ref DataStreamWriter writer)
        {
            base.SerializeObject(ref writer);

            writer.WriteInt(PlayerID);
            writer.WriteUInt(Colour);
        }

        public override void DeserializeObject(ref DataStreamReader reader)
        {
            base.DeserializeObject(ref reader);
            
            PlayerID = reader.ReadInt();
            Colour = reader.ReadUInt();
        }
    }
}
