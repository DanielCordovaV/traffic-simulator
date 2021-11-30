using System;
using System.Collections.Generic;

[Serializable]
public class Data
{
    public List<Car> cars;
    public List<TrafficLight> trafficLights;
}

[Serializable]
public class Car
{
    public int id;
    public List<int> pos;
    public List<int> direction;
}

[Serializable]
public class TrafficLight
{
    public int id;
    public List<int> pos;
    public string color;
}