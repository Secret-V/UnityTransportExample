using UnityEngine;
using System.Collections;
using Unity.Networking.Transport;
using Unity.Collections;
using Unity.Jobs;
using Unity.Networking.Transport.Utilities;

public class ClientBehaviour : MonoBehaviour
{
    private NetworkDriver networkDriver;
    private NativeArray<NetworkConnection> connection;
    private NativeArray<byte> done;

    private JobHandle jobHandle;

    // Use this for initialization
    void Start()
    {
        networkDriver = NetworkDriver.Create();
        connection = new NativeArray<NetworkConnection>(1, Allocator.Persistent);
        done = new NativeArray<byte>(1, Allocator.Persistent);

        var endpoint = NetworkEndPoint.LoopbackIpv4;
        endpoint.Port = 9000;

        connection[0] = networkDriver.Connect(endpoint);
    }

    // Update is called once per frame
    void Update()
    {
        jobHandle.Complete();

        var job = new ClientUpdateJob
        {
            networkDriver = networkDriver,
            connection = connection,
            done = done
        };

        jobHandle = networkDriver.ScheduleUpdate();
        jobHandle = job.Schedule(jobHandle);
    }

    private void OnDestroy()
    {
        jobHandle.Complete();

        connection.Dispose();
        networkDriver.Dispose();
        done.Dispose();
    }
}
