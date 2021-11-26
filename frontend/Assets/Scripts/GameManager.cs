using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject RequestManager;
    [SerializeField] private GameObject CarPrefab;
    
    private Requesting RequestingScript;
    private Data positions;

    void Start()
    {
        RequestingScript = RequestManager.GetComponent<Requesting>();
        RequestingScript.positionsQueue.TryDequeue(out positions);

        for (int i = 0; i < positions.data.Length; i++)
        {
            Instantiate(CarPrefab, new Vector3(0, 0, i + 10), Quaternion.identity);
        }
    }

    void Update()
    {
        if (!RequestingScript.positionsQueue.IsEmpty)
        {
            RequestingScript.positionsQueue.TryDequeue(out positions);   
        }
    }
}