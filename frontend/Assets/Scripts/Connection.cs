/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;

using NativeWebSocket;

public class Connection : MonoBehaviour
{
    WebSocket websocket;
    private Data positions;
    
    public async void Start()
    {
        websocket = new WebSocket("ws://localhost:6789");
        Debug.Log(websocket);
        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            positions = JsonUtility.FromJson<Data>(message);
            Singleton.Instance.objectQueue.Enqueue(positions);
        };

        // Keep sending messages at every 0.3s
        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        // waiting for messages
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending plain text
            string tmp = "{\"action\": \"get_frame\"}";
            await websocket.SendText(tmp);
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }

}*/