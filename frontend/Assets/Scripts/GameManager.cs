using System;
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

    private List<Car> cars = new List<Car>();
    private List<Street> streets = new List<Street>();
    private List<TrafficLight> trafficLights = new List<TrafficLight>();
    
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
        if (!initialized)
        {
            Initialize();
        }
        else if (initialized)
        {
            if (requestingScript.objectQueue.TryDequeue(out objects))
            {
                for (int i = 0; i < objects.cars.Count; i++)
                {
                    float x = objects.cars[i].pos[0] * scaleMultiplier;
                    float z = objects.cars[i].pos[1] * scaleMultiplier;
                    if (objects.cars[i].id == cars[i].id)
                    {
                        cars[i].vehicle.GetComponent<NavMeshAgent>().Move(new Vector3(x, 0 ,z));
                        CarRotation(objects.cars[i], cars[i].vehicle, objects.cars[i].direction[0], objects.cars[i].direction[0]);
                    }
                    
                    Debug.Log("Moved car");
                }

                for (int i = 0; i < objects.trafficLights.Count; i++)
                {
                    if (objects.trafficLights[i].id == trafficLights[i].id)
                    {
                        shader = trafficLights[i].light.GetComponentInChildren<AccessShaderProperties>();
                        shader.ChangeLight(objects.trafficLights[i].color);
                    }
                    
                    Debug.Log("Changed Traffic Light Color");
                }
            }
        }
    }

    void Initialize()
    {
        if (requestingScript.objectQueue.TryDequeue(out objects))
        {
            GameObject newGO;
            for (int i = 0; i < objects.cars.Count; i++)
            {
                float x = objects.cars[i].pos[0] * scaleMultiplier;
                float z = objects.cars[i].pos[1] * scaleMultiplier;
                newGO = Instantiate(carPrefab, new Vector3(x, 0.66f, z), Quaternion.identity);
                newGO.transform.parent = GameObject.Find("Cars").transform;
                newGO.GetComponentInChildren<AccessCarShader>().ChangeColor();
                CarRotation(objects.cars[i], newGO, objects.cars[i].direction[0], objects.cars[i].direction[0]);
                tmpCar = objects.cars[i];
                tmpCar.vehicle = newGO;
                cars.Add(tmpCar);
            }
            for (int i = 0; i < objects.trafficLights.Count; i++)
            {
                float x = objects.trafficLights[i].pos[0] * scaleMultiplier;
                float z = objects.trafficLights[i].pos[1] * scaleMultiplier;
                newGO = Instantiate(trafficLightPrefab, new Vector3(x, 0, z), Quaternion.identity);
                shader = newGO.GetComponentInChildren<AccessShaderProperties>();
                shader.ChangeLight(objects.trafficLights[i].color);
                newGO.transform.parent = GameObject.Find("TrafficLights").transform;
                tmpTrafficLight = objects.trafficLights[i];
                tmpTrafficLight.light = newGO;
                trafficLights.Add(tmpTrafficLight);
            }
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

            if (streets.Count > 0)
            {
                mesh = streets[0].street.GetComponent<NavMeshSurface>();
                mesh.BuildNavMesh();
            }
            initialized = true;
        }
    }

    void CarRotation(Car tmp, GameObject carCurr, float x, float z)
    {
        if (tmp.direction[0] == 1 && tmp.direction[1] == 0)
        {
            carCurr.transform.Rotate(new Vector3 (0, 90, 0));
        }
        else if (tmp.direction[0] == -1 && tmp.direction[1] == 0)
        {
            carCurr.transform.Rotate(new Vector3 (0, -90, 0));
        }
        else if (tmp.direction[0] == 0 && tmp.direction[1] == 1)
        {
            carCurr.transform.Rotate(new Vector3 (0, 0, 0));
        }
        else if (tmp.direction[0] == 0 && tmp.direction[1] == -1)
        {
            carCurr.transform.Rotate(new Vector3 (0, 180, 0));
        }
    }
}