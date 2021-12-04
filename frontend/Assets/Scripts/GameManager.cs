using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject requestManager;
    [SerializeField] private GameObject carPrefab;
    [SerializeField] private GameObject streetPrefab;
    [SerializeField] private GameObject trafficLightPrefab;
    [SerializeField] private float timeLimit;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject floor;
    
    private NavMeshSurface mesh;
    
    private Requesting requestingScript;
    private Data objects;
    private AccessShaderProperties shader;
    private Car tmpCar;
    private TrafficLight tmpTrafficLight;
    private Street tmpStreet;

    private List<Car> cars = new List<Car>();
    private List<Street> streets = new List<Street>();
    private List<TrafficLight> trafficLights = new List<TrafficLight>();

    private HashSet<int> currentCars = new HashSet<int>();

    private float scaleMultiplier = 15f;
    private bool initialized = false;
    private int numDistance = 0;

    void Start()
    {
        Debug.Log("Game Manager Start");
        requestingScript = requestManager.GetComponent<Requesting>();
        requestingScript.Initialize();
    }

    void Update()
    {
        if (timeLimit > 0f)
        {
            timeLimit -= Time.deltaTime;
        }

        if (!initialized)
        {
            Initialize();
        }
        
        if (timeLimit <= 0f)
        {
            canvas.SetActive(true);
            Time.timeScale = 0;
            Application.Quit();
        }
        
        numDistance = 0;
        for (int i = 0; i < cars.Count; i++)
        {
            if (cars[i].vehicle.GetComponent<NavMeshAgent>().remainingDistance < 11f)
            {
                numDistance++;
            }
        }

        if (numDistance == cars.Count && Singleton.Instance.objectQueue.TryDequeue(out objects))
        {
            numDistance = 0;
            currentCars = new HashSet<int>();
            for (int i = 0; i < objects.cars.Count; i++)
            {
                currentCars.Add(objects.cars[i].id);
            }
            
            List<Car> toRemove = new List<Car>();
            for (int i = 0; i < cars.Count; i++)
            {
                if (!currentCars.Contains(cars[i].id))
                {
                    toRemove.Add(cars[i]);
                }
            }

            foreach (var trash in toRemove)
            {
                Destroy(trash.vehicle);
                cars.Remove(trash);
            }
            
            for (int i = 0; i < objects.cars.Count; i++) 
            {
                float zNew = objects.cars[i].pos[0] * scaleMultiplier;
                float xNew = objects.cars[i].pos[1] * scaleMultiplier;

                if (objects.cars[i].id == cars[i].id)
                {
                    cars[i].vehicle.GetComponent<NavMeshAgent>().destination = new Vector3(xNew, 0, zNew);
                    cars[i].pos = objects.cars[i].pos;
                    cars[i].direction = objects.cars[i].direction;
                }
            }

            for (int i = 0; i < objects.trafficLights.Count; i++)
            {
                if (objects.trafficLights[i].id == trafficLights[i].id)
                {
                    shader = trafficLights[i].light.GetComponentInChildren<AccessShaderProperties>();
                    shader.ChangeLight(objects.trafficLights[i].color);
                    trafficLights[i].color = objects.trafficLights[i].color;
                }
            }
        }
        if (timeLimit <= 0f)
        {
            canvas.SetActive(true);
            Time.timeScale = 0;
            Application.Quit();
        }
    }

    // Places the cars, traffic lights and streets in their initial positions
    void Initialize()
    {
        if (Singleton.Instance.objectQueue.TryDequeue(out objects))
        {
            GameObject newGO;
            
            // Adds the streets
            for (int i = 0; i < objects.streets.Count; i++)
            {
                float zStart = objects.streets[i].start[0];
                float xStart = objects.streets[i].start[1];
                float zEnd = objects.streets[i].end[0];
                float xEnd = objects.streets[i].end[1];
                
                Vector3 mid = (new Vector3(xStart, 0f, zStart) + new Vector3(xEnd, 0f, zEnd)) / 2;
                
                if (objects.streets[i].direction[0] == 0 && objects.streets[i].direction[1] == 1)
                {
                    mid += new Vector3(0,0.001f,0);
                }
                else if (objects.streets[i].direction[0] == 1 && objects.streets[i].direction[1] == 0)
                {
                    mid += new Vector3(0,0.002f,0);
                }
                else if (objects.streets[i].direction[0] == -1 && objects.streets[i].direction[1] == 0)
                {
                    mid += new Vector3(0,-0.002f,0);
                }
                else if (objects.streets[i].direction[0] == 0 && objects.streets[i].direction[1] == -1)
                {
                    mid += new Vector3(0,-0.001f,0);
                }
                
                
                newGO = Instantiate(streetPrefab, mid * scaleMultiplier, Quaternion.identity);
                ObjectRotation(objects.streets[i].direction, newGO);
                newGO.transform.Rotate(0,90,0);
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
                floor.SetActive(true);
            }
            
            // Adds the cars
            for (int i = 0; i < objects.cars.Count; i++)
            {
                float z = objects.cars[i].pos[0] * scaleMultiplier;
                float x = objects.cars[i].pos[1] * scaleMultiplier;
                newGO = Instantiate(carPrefab, new Vector3(x, 0.66f, z), Quaternion.identity);
                newGO.transform.parent = GameObject.Find("Cars").transform;
                newGO.GetComponentInChildren<AccessCarShader>().ChangeColor();
                ObjectRotation(objects.cars[i].direction, newGO);
                tmpCar = objects.cars[i];
                tmpCar.vehicle = newGO;
                currentCars.Add(tmpCar.id);
                cars.Add(tmpCar);
                Debug.Log("Added car");
            }
            
            // Adds the traffic lights
            for (int i = 0; i < objects.trafficLights.Count; i++)
            {
                float z = objects.trafficLights[i].pos[0] * scaleMultiplier;
                float x = objects.trafficLights[i].pos[1] * scaleMultiplier;
                newGO = Instantiate(trafficLightPrefab, new Vector3(x, 0, z), Quaternion.identity);
                shader = newGO.GetComponentInChildren<AccessShaderProperties>();
                shader.ChangeLight(objects.trafficLights[i].color);
                newGO.transform.parent = GameObject.Find("TrafficLights").transform;
                ObjectRotation(objects.trafficLights[i].direction, newGO);
                newGO.transform.Rotate(new Vector3(0,180,0));
                
                if (objects.trafficLights[i].direction[0] == 0 && objects.trafficLights[i].direction[1] == 1)
                {
                    newGO.transform.position = new Vector3(x - 8f,0f,z - 8f);
                }
                else if (objects.trafficLights[i].direction[0] == 1 && objects.trafficLights[i].direction[1] == 0)
                {
                    newGO.transform.position = new Vector3(x + 8f,0f,z - 8f);
                }
                else if (objects.trafficLights[i].direction[0] == -1 && objects.trafficLights[i].direction[1] == 0)
                {
                    newGO.transform.position = new Vector3(x - 8f,0f,z + 8f);
                }
                else if (objects.trafficLights[i].direction[0] == 0 && objects.trafficLights[i].direction[1] == -1)
                {
                    newGO.transform.position = new Vector3(x + 8f,0f,z + 8f);
                }
                
                tmpTrafficLight = objects.trafficLights[i];
                tmpTrafficLight.light = newGO;
                trafficLights.Add(tmpTrafficLight);
                Debug.Log("Added Traffic Light");
            }

            initialized = true;
        }
    }

    // Rotates objects
    void ObjectRotation(List<int> tmp, GameObject curr)
    {
        if (tmp[0] == 1 && tmp[1] == 0)
        {
            curr.transform.Rotate(new Vector3 (0, 0, 0));
        }
        else if (tmp[0] == -1 && tmp[1] == 0)
        {
            curr.transform.Rotate(new Vector3 (0, 180, 0));
        }
        else if (tmp[0] == 0 && tmp[1] == 1)
        {
            curr.transform.Rotate(new Vector3 (0, 90, 0));
        }
        else if (tmp[0] == 0 && tmp[1] == -1)
        {
            curr.transform.Rotate(new Vector3 (0, -90, 0));
        }
    }
}