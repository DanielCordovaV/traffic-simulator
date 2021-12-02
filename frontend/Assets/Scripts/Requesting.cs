using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Networking;

public class Requesting : MonoBehaviour
{
    [SerializeField] private float requestDelay = 500.0f;

    // public ConcurrentQueue<Data> objectQueue = new ConcurrentQueue<Data>();
    public TextAsset jsonFile;
    // private Connection socket;
    public void Initialize()
    {
        // StartCoroutine(GetPositions( (i) => { objectQueue.Enqueue(i); } ));
        
        // socket.Start();
        
        /*
        Data positions = JsonUtility.FromJson<Data>(jsonFile.text);
        objectQueue.Enqueue(positions);*/
    }

    IEnumerator GetPositions(System.Action<Root> callback)
    {
        while (true)
        {
            // yield return new WaitForSeconds(requestDelay);
            UnityWebRequest www = UnityWebRequest.Get("https://traffic-simulator.us-south.cf.appdomain.cloud/");
            yield return www.SendWebRequest();

            Root positions = JsonUtility.FromJson<Root>(www.downloadHandler.text);
            callback(positions);
            yield return new WaitForSeconds(requestDelay);
        }
    }
}