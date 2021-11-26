using System.Diagnostics;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class CarController : MonoBehaviour
{
    NavMeshAgent agent;

    public Transform objective;
    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        // agent.Move();
    }

    public void Move(Vector3 position)
    {
        agent.Move(position);
    }
}
