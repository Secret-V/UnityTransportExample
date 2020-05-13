using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Networking.Transport;

namespace Assets.Code
{
    public class SetNameMessage : Message
    {
        public string Name { get; set; }

        public SetNameMessage()
        {
            Type = MessageType.SetName;
        }

        public override void SerializeObject(ref DataStreamWriter writer)
        {
            base.SerializeObject(ref writer);

            writer.WriteString(Name);
        }

        public override void DeserializeObject(ref DataStreamReader reader)
        {
            base.DeserializeObject(ref reader);

            Name = reader.ReadString().ToString();
        }
    }
}
