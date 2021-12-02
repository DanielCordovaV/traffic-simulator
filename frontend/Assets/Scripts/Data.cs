using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Data
{
    public List<Car> cars;
    public List<TrafficLight> trafficLights;
    public List<Street> streets;
}

[Serializable]
public class Car
{
    public int id;
    public List<int> pos;
    public List<int> direction;
    public GameObject vehicle;
}

[Serializable]
public class TrafficLight
{
    public int id;
    public List<int> pos;
    public string color;
    public GameObject light;
}

[Serializable]
public class Street
{
    public int id;
    public List<int> pos;
    public GameObject street;
}