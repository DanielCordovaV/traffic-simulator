using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Networking;

public class Requesting : MonoBehaviour
{
    String url = "https://traffic-simulator-grouchy-bear-tt.mybluemix.net/" + DateTimeOffset.Now.ToUnixTimeSeconds();
    public void Initialize()
    {
        StartCoroutine(GetPositions());
    }

    IEnumerator GetPositions()
    {
        while (true)
        {
            Debug.Log(url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            Data positions = JsonUtility.FromJson<Data>(www.downloadHandler.text);
            Singleton.Instance.objectQueue.Enqueue(positions);
            Debug.Log(url);
            yield return null;
        }
    }
}