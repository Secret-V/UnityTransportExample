using UnityEngine;
using System.Collections;
using Unity.Jobs;
using Unity.Networking.Transport;
using Unity.Collections;
using Assets.Code;

public struct ClientUpdateJob : IJob
{
    public NetworkDriver networkDriver;
    public NativeArray<NetworkConnection> connection;
    public NativeArray<byte> done;

    public void Execute()
    {
        if (!connection[0].IsCreated)
        {
            if (done[0] != 1) Debug.Log("Something went wrong during connect");
            return;
        }

        DataStreamReader reader;
        NetworkEvent.Type cmd;
        while ((cmd = connection[0].PopEvent(networkDriver, out reader)) != NetworkEvent.Type.Empty)
        {
            if (cmd == NetworkEvent.Type.Connect)
            {
                
            }
            else if (cmd == NetworkEvent.Type.Data)
            {
                var messageType = (Message.MessageType)reader.ReadUShort();
                switch (messageType)
                {
                    case Message.MessageType.NewPlayer:
                        break;
                    case Message.MessageType.Welcome:
                        var welcomeMessage = new WelcomeMessage();
                        welcomeMessage.DeserializeObject(ref reader);
                        Debug.Log($"Received message, ID: {welcomeMessage.ID}, PlayerID: {welcomeMessage.PlayerID}, Colour: {welcomeMessage.Colour}");

                        var setnameMessage = new SetNameMessage
                        {
                            Name = "Vincent"
                        };
                        var writer = networkDriver.BeginSend(connection[0]);
                        setnameMessage.SerializeObject(ref writer);
                        networkDriver.EndSend(writer);
                        break;
                    case Message.MessageType.SetName:
                        break;
                    case Message.MessageType.RequestDenied:
                        break;
                    case Message.MessageType.PlayerLeft:
                        break;
                    case Message.MessageType.StartGame:
                        break;
                    case Message.MessageType.None:
                    default:
                        break;
                }
            }
            else if (cmd == NetworkEvent.Type.Disconnect)
            {
                Debug.Log("Client got disconnected from server");
                connection[0] = default;
            }
        }
    }
}
