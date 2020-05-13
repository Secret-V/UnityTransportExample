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

    public void Execute(int index)
    {
        DataStreamReader reader;
        Assert.IsTrue(connections[index].IsCreated);

        NetworkEvent.Type cmd;
        while((cmd = networkDriver.PopEventForConnection(connections[index], out reader)) != NetworkEvent.Type.Empty)
        {
            if(cmd == NetworkEvent.Type.Data)
            {
                var messageType = (Message.MessageType)reader.ReadUShort();
                switch (messageType)
                {
                    case Message.MessageType.None:
                        break;
                    case Message.MessageType.NewPlayer:
                        break;
                    case Message.MessageType.Welcome:
                        break;
                    case Message.MessageType.SetName:
                        var message = new SetNameMessage();
                        message.DeserializeObject(ref reader);
                        Debug.Log($"Welcome, user: {message.Name}");
                        break;
                    case Message.MessageType.RequestDenied:
                        break;
                    case Message.MessageType.PlayerLeft:
                        break;
                    case Message.MessageType.StartGame:
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
