using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject requestManager;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject streetPrefab;
    [SerializeField] private NavMeshSurface mesh;
    
    private Requesting requestingScript;
    private Data positions;

    private List<GameObject> cars = new List<GameObject>();
    private List<GameObject> streets = new List<GameObject>();

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
                newGO.transform.parent = GameObject.Find("Cars").transform;
            }
            for (int i = 0; i < requestingScript.positionsQueue.Count; i++)
            {
                GameObject newGO = Instantiate(streetPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                streets.Add(newGO);
                newGO.transform.parent = GameObject.Find("Road").transform;
            }

            mesh = streets[0].GetComponent<NavMeshSurface>();
            mesh.BuildNavMesh();
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