using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Concurrent;

// Singleton that contains the concurrent queue that is used to store requests
public sealed class Singleton  
{
    private readonly static Singleton _instance = new Singleton();
    
    private Singleton() { }
    
    public ConcurrentQueue<Data> objectQueue = new ConcurrentQueue<Data>();
    
    public static Singleton Instance
    {
        get
        {
            return _instance;
        }
    }
    
    
}
