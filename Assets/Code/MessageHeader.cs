using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Networking.Transport;

namespace Assets.Code
{
    public interface IMessage
    {
        void SerializeObject(ref DataStreamWriter writer);
        void DeserializeObject(ref DataStreamReader reader);
    }

    public struct MessageHeader : IMessage
    {
        private static uint nextID = 0;
        private static uint NextID => ++nextID;

        public IMessage Message { get; set; }

        public enum MessageType
        {
            None = 0,
            NewPlayer,
            Welcome,
            SetName,
            RequestDenied,
            PlayerLeft,
            StartGame,
            Count
        }

        public MessageType Type { get; private set; }
        public uint ID { get; private set; }

        public void SerializeObject(ref DataStreamWriter writer)
        {
            writer.WriteUShort((ushort)Type);
            writer.WriteUInt(ID);
            Message.SerializeObject(ref writer);
        }

        public void DeserializeObject(ref DataStreamReader reader)
        {
            ID = reader.ReadUInt();
            Message.DeserializeObject(ref reader);
        }
    }
}
