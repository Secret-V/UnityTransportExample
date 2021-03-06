﻿using Assets.Code;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Networking.Transport;
using UnityEngine;
using UnityEngine.Events;

public class ServerBehaviour : MonoBehaviour
{
    private NetworkDriver networkDriver;
    private NativeList<NetworkConnection> connections;
    private JobHandle jobHandle;

    private NativeQueue<MessageHeader> messagesQueue;

    public UnityEvent<MessageHeader>[] ServerCallbacks = new UnityEvent<MessageHeader>[(int)MessageHeader.MessageType.Count - 1];

    void Start()
    {
        networkDriver = NetworkDriver.Create();

        var endpoint = NetworkEndPoint.AnyIpv4;
        endpoint.Port = 9000;

        if(networkDriver.Bind(endpoint) != 0)
        {
            Debug.Log("Failed to bind to port 9000");
        }
        else
        {
            networkDriver.Listen();
        }

        connections = new NativeList<NetworkConnection>(16, Allocator.Persistent);
    }

    void Update()
    {
        jobHandle.Complete();

        while(messagesQueue.Count > 0)
        {
            MessageHeader message = messagesQueue.Dequeue();

            ServerCallbacks[(int)message.Type].Invoke(message);
        }

        var connectionJob = new ServerUpdateConnectionsJob
        {
            networkDriver = networkDriver,
            connections = connections
        };

        var serverUpdateJob = new ServerUpdateJob
        {
            networkDriver = networkDriver.ToConcurrent(),
            connections = connections.AsDeferredJobArray(),
            messagesQueue = messagesQueue.AsParallelWriter()
        };

        jobHandle = networkDriver.ScheduleUpdate();
        jobHandle = connectionJob.Schedule(jobHandle);
        jobHandle = serverUpdateJob.Schedule(connections, 1, jobHandle);
    }

    private void OnDestroy()
    {
        jobHandle.Complete();
        networkDriver.Dispose();
        connections.Dispose();
    }
}
