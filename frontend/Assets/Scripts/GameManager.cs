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
    private Car tmpCar;
    private TrafficLight tmpTrafficLight;
    private Street tmpStreet;
    private Connection socket;

    private List<Car> cars = new List<Car>();
    private List<Street> streets = new List<Street>();
    private List<TrafficLight> trafficLights = new List<TrafficLight>();
    
    [SerializeField] private int scaleMultiplier = 30;
    private bool initialized = false;

    void Start()
    {
        Debug.Log("Game Manager Start");
        /*requestingScript = requestManager.GetComponent<Requesting>();
        requestingScript.Initialize();*/
        socket.Start();
    }

    void Update()
    {
        if (!initialized)
        {
            Initialize();
        }
        else if (Singleton.Instance.objectQueue.TryDequeue(out objects))
        {
            for (int i = 0; i < objects.cars.Count; i++) 
            {
                    float x = objects.cars[i].pos[0] * scaleMultiplier;
                    float z = objects.cars[i].pos[1] * scaleMultiplier;
                    if (objects.cars[i].id == cars[i].id)
                    {
                        cars[i].vehicle.GetComponent<NavMeshAgent>().Move(new Vector3(x, 0 ,z));
                        ObjectRotation(objects.cars[i].direction, cars[i].vehicle);
                        cars[i].direction = objects.cars[i].direction;
                    }
                    
                    Debug.Log("Moved car");
            }
    
            for (int i = 0; i < objects.trafficLights.Count; i++)
            {
                if (objects.trafficLights[i].id == trafficLights[i].id)
                {
                    shader = trafficLights[i].light.GetComponentInChildren<AccessShaderProperties>();
                    shader.ChangeLight(objects.trafficLights[i].color);
                    trafficLights[i].color = objects.trafficLights[i].color;
                }
                
                Debug.Log("Changed Traffic Light Color");
            }
        }
    }

    // Places the cars, traffic lights and streets in their initial positions
    void Initialize()
    {
        if (Singleton.Instance.objectQueue.TryDequeue(out objects))
        {
            GameObject newGO;
            
            // Adds the cars
            for (int i = 0; i < objects.cars.Count; i++)
            {
                float x = objects.cars[i].pos[0] * scaleMultiplier;
                float z = objects.cars[i].pos[1] * scaleMultiplier;
                newGO = Instantiate(carPrefab, new Vector3(x, 0.66f, z), Quaternion.identity);
                newGO.transform.parent = GameObject.Find("Cars").transform;
                newGO.GetComponentInChildren<AccessCarShader>().ChangeColor();
                ObjectRotation(objects.cars[i].direction, newGO);
                tmpCar = objects.cars[i];
                tmpCar.vehicle = newGO;
                cars.Add(tmpCar);
            }
            
            // Adds the traffic lights
            for (int i = 0; i < objects.trafficLights.Count; i++)
            {
                float x = objects.trafficLights[i].pos[0] * scaleMultiplier;
                float z = objects.trafficLights[i].pos[1] * scaleMultiplier;
                newGO = Instantiate(trafficLightPrefab, new Vector3(x, 0, z), Quaternion.identity);
                shader = newGO.GetComponentInChildren<AccessShaderProperties>();
                shader.ChangeLight(objects.trafficLights[i].color);
                newGO.transform.parent = GameObject.Find("TrafficLights").transform;
                // ObjectRotation(objects.trafficLights[i].direction, newGO);
                tmpTrafficLight = objects.trafficLights[i];
                tmpTrafficLight.light = newGO;
                trafficLights.Add(tmpTrafficLight);
            }
            
            // Adds the streets
            for (int i = 0; i < objects.streets.Count; i++)
            {
                float x = objects.trafficLights[i].pos[0] * scaleMultiplier;
                float z = objects.trafficLights[i].pos[1] * scaleMultiplier;
                newGO = Instantiate(streetPrefab, new Vector3(x, 0, z), Quaternion.identity);
                newGO.transform.parent = GameObject.Find("Road").transform;
                tmpStreet = objects.streets[i];
                tmpStreet.street = newGO;
                streets.Add(tmpStreet);
                Debug.Log("Added street");
            }
            
            // Creates the nav mesh if the streets have been added
            if (streets.Count > 0)
            {
                mesh = streets[0].street.GetComponent<NavMeshSurface>();
                mesh.BuildNavMesh();
            }
            initialized = true;
        }
    }

    // Rotates objects
    void ObjectRotation(List<int> tmp, GameObject curr)
    {
        if (tmp[0] == 1 && tmp[1] == 0)
        {
            curr.transform.Rotate(new Vector3 (0, 90, 0));
        }
        else if (tmp[0] == -1 && tmp[1] == 0)
        {
            curr.transform.Rotate(new Vector3 (0, -90, 0));
        }
        else if (tmp[0] == 0 && tmp[1] == 1)
        {
            curr.transform.Rotate(new Vector3 (0, 0, 0));
        }
        else if (tmp[0] == 0 && tmp[1] == -1)
        {
            curr.transform.Rotate(new Vector3 (0, 180, 0));
        }
    }
}