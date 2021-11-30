using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject requestManager;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject streetPrefab;
    [SerializeField] private GameObject trafficLightPrefab;
    private NavMeshSurface mesh;
    
    private Requesting requestingScript;
    private Data objects;
    private AccessShaderProperties shader;

    private List<GameObject> cars = new List<GameObject>();
    private List<GameObject> streets = new List<GameObject>();
    private List<GameObject> trafficLights = new List<GameObject>();
    
    [SerializeField] private int scaleMultiplier = 30;
    private bool initialized = false;

    void Start()
    {
        Debug.Log("Game Manager Start");
        requestingScript = requestManager.GetComponent<Requesting>();
        requestingScript.Initialize();
    }

    void Update()
    {
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
            Debug.Log("if");
            Debug.Log("Car count: " + objects.cars.Count);
            Debug.Log("Traffic Light count: " + objects.trafficLights.Count);
            for (int i = 0; i < objects.cars.Count; i++)
            {
                float x = objects.cars[i].pos[0] * scaleMultiplier;
                float z = objects.cars[i].pos[1] * scaleMultiplier;
                print("type == car");
                newGO = Instantiate(carPrefab, new Vector3(x, 0.66f, z), Quaternion.identity);
                newGO.transform.parent = GameObject.Find("Cars").transform;
                if (objects.cars[i].direction[0] == 1 && objects.cars[i].direction[1] == 0)
                {
                    newGO.transform.Rotate(new Vector3 (0, 90, 0));
                }
                else if (objects.cars[i].direction[0] == -1 && objects.cars[i].direction[1] == 0)
                {
                    newGO.transform.Rotate(new Vector3 (0, -90, 0));
                }
                else if (objects.cars[i].direction[0] == 0 && objects.cars[i].direction[1] == 1)
                {
                    newGO.transform.Rotate(new Vector3 (0, 0, 0));
                }
                else if (objects.cars[i].direction[0] == 0 && objects.cars[i].direction[1] == -1)
                {
                    newGO.transform.Rotate(new Vector3 (0, 180, 0));
                }
                cars.Add(newGO);
                Debug.Log("Added car");
            }
            for (int i = 0; i < objects.trafficLights.Count; i++)
            {
                float x = objects.trafficLights[i].pos[0] * scaleMultiplier;
                float z = objects.trafficLights[i].pos[1] * scaleMultiplier;
                print("type == trafficLight");
                newGO = Instantiate(trafficLightPrefab, new Vector3(x, 0, z), Quaternion.identity);
                // shader = newGO.GetComponent<AccessShaderProperties>();
                // shader.ChangeLight(objects.trafficLights[i].color);
                newGO.transform.parent = GameObject.Find("TrafficLights").transform;
                trafficLights.Add(newGO);
                Debug.Log("Added traffic light");
            }
            /*for (int i = 0; i < objects.cars.Count; i++)
            {
                float x = objects.trafficLights[i].pos[0] * scaleMultiplier;
                float z = objects.trafficLights[i].pos[1] * scaleMultiplier;
                print("type == street");
                newGO = Instantiate(streetPrefab, new Vector3(x, 0, z), Quaternion.identity);
                newGO.transform.parent = GameObject.Find("Road").transform;
                streets.Add(newGO);
                Debug.Log("Added street")
            }*/

            if (streets.Count != 0)
            {
                mesh = streets[0].GetComponent<NavMeshSurface>();
                mesh.BuildNavMesh();
            }
            initialized = true;
        }
    }
}