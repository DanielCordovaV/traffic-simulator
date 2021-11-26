using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Networking;

public class Requesting : MonoBehaviour
{
    // [SerializeField] private float requestDelay = 3000.0f;

    public ConcurrentQueue<Data> positionsQueue = new ConcurrentQueue<Data>();
    void Start()
    {
        
        // StartCoroutine(GetText( (i) => { positionsQueue.Enqueue(i); } ));
    }

    IEnumerator GetText(System.Action<Data> callback)
    {
        while (true)
        {
            // yield return new WaitForSeconds(requestDelay);
            
            UnityWebRequest www = UnityWebRequest.Get("https://traffic-simulator.us-south.cf.appdomain.cloud/");
            yield return www.SendWebRequest();

            Data positions = JsonUtility.FromJson<Data>(www.downloadHandler.text);
            Debug.Log(positions.data);
            callback(positions);
        }
    }
}
