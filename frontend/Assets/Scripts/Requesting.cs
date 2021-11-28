using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Networking;

public class Requesting : MonoBehaviour
{
    [SerializeField] private float requestDelay = 500.0f;

    public ConcurrentQueue<Root> objectQueue = new ConcurrentQueue<Root>();
    public TextAsset jsonFile;
    public void Initialize()
    {
        StartCoroutine(GetPositions( (i) => { objectQueue.Enqueue(i); } ));
        
        /*Root positions = JsonUtility.FromJson<Root>("{\"data\":" + jsonFile.text + "}");
        print(jsonFile.text);
        print(positions.data.Count);*/
    }

    IEnumerator GetPositions(System.Action<Root> callback)
    {
        while (true)
        {
            // yield return new WaitForSeconds(requestDelay);
            UnityWebRequest www = UnityWebRequest.Get("https://traffic-simulator.us-south.cf.appdomain.cloud/");
            yield return www.SendWebRequest();

            Root positions = JsonUtility.FromJson<Root>("{\"data\":" + www.downloadHandler.text + "}");
            callback(positions);
            yield return new WaitForSeconds(requestDelay);
        }
    }
}