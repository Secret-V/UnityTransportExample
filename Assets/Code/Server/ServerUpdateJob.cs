using UnityEngine;
using System.Collections;
using Unity.Jobs;
using Unity.Networking.Transport;
using Unity.Collections;
using UnityEngine.Assertions;
using Assets.Code;

public struct ServerUpdateJob : IJobParallelForDefer
{
    public NetworkDriver.Concurrent networkDriver;
    public NativeArray<NetworkConnection> connections;
    public NativeQueue<MessageHeader>.ParallelWriter messagesQueue;

    public void Execute(int index)
    {
        DataStreamReader reader;
        Assert.IsTrue(connections[index].IsCreated);

        NetworkEvent.Type cmd;
        while((cmd = networkDriver.PopEventForConnection(connections[index], out reader)) != NetworkEvent.Type.Empty)
        {
            if(cmd == NetworkEvent.Type.Data)
            {
                var messageType = (MessageHeader.MessageType)reader.ReadUShort();
                switch (messageType)
                {
                    case MessageHeader.MessageType.None:
                        break;
                    case MessageHeader.MessageType.NewPlayer:
                        break;
                    case MessageHeader.MessageType.Welcome:
                        break;
                    case MessageHeader.MessageType.SetName:
                        var header = new MessageHeader();
                        header.Message = new SetNameMessage();
                        header.DeserializeObject(ref reader);
                        messagesQueue.Enqueue(header);
                        break;
                    case MessageHeader.MessageType.RequestDenied:
                        break;
                    case MessageHeader.MessageType.PlayerLeft:
                        break;
                    case MessageHeader.MessageType.StartGame:
                        break;
                    default:
                        break;
                }
            }
            else if(cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client disconnected from server");
                connections[index] = default;
            }
        }
    }
}
