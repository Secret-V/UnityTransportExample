using UnityEngine;
using System.Collections;
using Unity.Jobs;
using Unity.Networking.Transport;
using Unity.Collections;
using Assets.Code;

public struct ServerUpdateConnectionsJob : IJob
{
    public NetworkDriver networkDriver;
    public NativeList<NetworkConnection> connections;

    public void Execute()
    {
        for(int i = 0; i < connections.Length; ++i)
        {
            if(!connections[i].IsCreated)
            {
                connections.RemoveAtSwapBack(i);
                --i;
            }
        }

        NetworkConnection c;
        while((c = networkDriver.Accept()) != default)
        {
            connections.Add(c);
            Debug.Log("Accepted a connection");

            var colour = (Color32)Color.magenta;
            var message = new WelcomeMessage
            {
                PlayerID = c.InternalId,
                Colour = ((uint)colour.r << 24) | ((uint)colour.g << 16) | ((uint)colour.b << 8) | colour.a
            };

            var writer = networkDriver.BeginSend(c);
            message.SerializeObject(ref writer);
            networkDriver.EndSend(writer);
        }
    }
}
