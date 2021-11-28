using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject requestManager;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject streetPrefab;
    [SerializeField] private GameObject trafficLightPrefab;
    [SerializeField] private NavMeshSurface mesh;
    
    private Requesting requestingScript;
    private Root objects;

    private List<GameObject> cars = new List<GameObject>();
    private List<GameObject> streets = new List<GameObject>();
    private List<GameObject> trafficLights = new List<GameObject>();
    
    [SerializeField] private int scaleMultiplier = 30;
    private bool initialized = false;

    void Start()
    {
        print("Game Manager Start");
        requestingScript = requestManager.GetComponent<Requesting>();
        requestingScript.Initialize();
    }

    void Update()
    {
        print(requestingScript.objectQueue.Count);
        if (initialized == false)
        {
            Initialize();
        }
    }

    void Initialize()
    {
        if (requestingScript.objectQueue.TryDequeue(out objects))
        {
            GameObject newGO;
            for (int i = 0; i < objects.data.Count; i++)
            { 
                print("For " + i);
                float x = objects.data[i].pos[0] * scaleMultiplier;
                float z = objects.data[i].pos[1] * scaleMultiplier;
                
                switch (objects.data[i].type)
                {
                    case "car":
                        print("type == car");
                        newGO = Instantiate(carPrefab, new Vector3(x, 0, z), Quaternion.identity);
                        newGO.transform.parent = GameObject.Find("Cars").transform;
                        if (x == 0 && z == 4)
                        {
                            newGO.transform.Rotate(new Vector3 (0, -90, 0));
                        }
                        else if (x == 4 && z == 0)
                        {
                            newGO.transform.Rotate(new Vector3 (0, 180, 0));
                        }
                        cars.Add(newGO);
                        break;
                    case "street":
                        print("type == street");
                        newGO = Instantiate(streetPrefab, new Vector3(x, 0, z), Quaternion.identity);
                        newGO.transform.parent = GameObject.Find("Road").transform;
                        streets.Add(newGO);
                        break;
                    case "trafficLight":
                        print("type == trafficLight");
                        newGO = Instantiate(trafficLightPrefab, new Vector3(x, 0, z), Quaternion.identity);
                        newGO.transform.parent = GameObject.Find("TrafficLights").transform;
                        trafficLights.Add(newGO);
                        break;
                }
            }
            
            mesh = streets[0].GetComponent<NavMeshSurface>();
            mesh.BuildNavMesh();
            initialized = true;
        }
    }
}