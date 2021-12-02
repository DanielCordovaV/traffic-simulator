using UnityEngine;

public class RotateWheels : MonoBehaviour
{
    public GameObject wheels;
    private int amountChilds = 4;
    [SerializeField] private int spinSpeed = 5;

    void SpinWheels()
    {
        amountChilds = wheels.transform.childCount;
        for (int i = 0; i < amountChilds; i++)
        {
            var child = wheels.transform.GetChild(i).gameObject;
            child.transform.Rotate(new Vector3(360 * Time.deltaTime * spinSpeed,0,0));
        }   
    }

    private void Update()
    {
        SpinWheels();
    }
}
