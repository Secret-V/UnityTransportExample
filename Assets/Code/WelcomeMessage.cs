using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Networking.Transport;

namespace Assets.Code
{
    public struct WelcomeMessage : IMessage
    {
        public int PlayerID { get; set; }
        public uint Colour { get; set; }

        public void SerializeObject(ref DataStreamWriter writer)
        {
            writer.WriteInt(PlayerID);
            writer.WriteUInt(Colour);
        }

        public void DeserializeObject(ref DataStreamReader reader)
        {
            PlayerID = reader.ReadInt();
            Colour = reader.ReadUInt();
        }
    }
}
