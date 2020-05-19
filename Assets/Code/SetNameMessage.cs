using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Networking.Transport;

namespace Assets.Code
{
    public struct SetNameMessage : IMessage
    {
        public string Name { get; set; }

        public void SerializeObject(ref DataStreamWriter writer)
        {
            writer.WriteString(Name);
        }

        public void DeserializeObject(ref DataStreamReader reader)
        {
            Name = reader.ReadString().ToString();
        }
    }
}
