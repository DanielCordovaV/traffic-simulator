using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject requestManager;
    [SerializeField] private GameObject carPrefab;
    
    private Requesting requestingScript;
    private Data positions;

    private List<GameObject> cars = new List<GameObject>();

    void Start()
    {
        requestingScript = requestManager.GetComponent<Requesting>();
        if (!requestingScript.positionsQueue.IsEmpty)
        {
            requestingScript.positionsQueue.TryDequeue(out positions);   
            
            for (int i = 0; i < requestingScript.positionsQueue.Count; i++)
            {
                GameObject newGO = Instantiate(carPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                cars.Add(newGO);
            }
        }
    }

    void Update()
    {
        if (!requestingScript.positionsQueue.IsEmpty)
        {
            requestingScript.positionsQueue.TryDequeue(out positions);
            for (int i = 0; i < cars.Count; i++)
            {
                cars[i].GetComponent<CarController>().Move(Vector3.forward);
            }
        }
    }
}