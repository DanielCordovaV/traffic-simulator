using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class CarController : MonoBehaviour
{
    NavMeshAgent agent;
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Vector3 position)
    {
        agent.Move(position);
    }
}