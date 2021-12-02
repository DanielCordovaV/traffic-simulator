using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Networking;

public class Requesting : MonoBehaviour
{
    [SerializeField] private float requestDelay = 0.1f;

    public TextAsset jsonFile;
    String url = "https://traffic-simulator-grouchy-bear-tt.mybluemix.net/" + DateTimeOffset.Now.ToUnixTimeSeconds();
    public void Initialize()
    {
        StartCoroutine(GetPositions());
        
        /*
        Data positions = JsonUtility.FromJson<Data>(jsonFile.text);
        Singleton.Instance.objectQueue.Enqueue(positions);*/
    }
    
    /*(i) => { objectQueue.Enqueue(i); }
     System.Action<Data> callback
     */ 
    
    IEnumerator GetPositions()
    {
        while (true)
        {
            // yield return new WaitForSeconds(requestDelay);
            Debug.Log(url);
            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            Data positions = JsonUtility.FromJson<Data>(www.downloadHandler.text);
            Singleton.Instance.objectQueue.Enqueue(positions);
            Debug.Log(url);
            // callback(positions);
            // yield return new WaitForSeconds(0.1f);
            yield return null;
        }
    }
}