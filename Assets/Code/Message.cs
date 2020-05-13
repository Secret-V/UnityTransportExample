using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Networking.Transport;

namespace Assets.Code
{
    public class Message
    {
        private static uint nextID = 0;
        private static uint NextID => ++nextID;

        public enum MessageType
        {
            None = 0,
            NewPlayer,
            Welcome,
            SetName,
            RequestDenied,
            PlayerLeft,
            StartGame
        }

        public MessageType Type { get; protected set; }
        public uint ID { get; private set; } = NextID;

        public virtual void SerializeObject(ref DataStreamWriter writer)
        {
            writer.WriteUShort((ushort)Type);
            writer.WriteUInt(ID);
        }

        public virtual void DeserializeObject(ref DataStreamReader reader)
        {
            ID = reader.ReadUInt();
        }
    }
}
