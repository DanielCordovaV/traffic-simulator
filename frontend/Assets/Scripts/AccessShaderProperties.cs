using UnityEngine;

public class AccessShaderProperties : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;

    // Changes the color of a traffic light based on the input from the backend
    public void ChangeLight(string curr)
    {
        if (curr == "green_light")
        {
            TurnGreen();
        }
        else if (curr == "red_light")
        {
            TurnRed();
        }
        else if (curr == "yellow_light")
        {
            TurnYellow();
        }
    }
    
    // Turn the traffic light yellow
    public void TurnYellow()
    {
        renderer.materials[0].SetColor("Color_6c804fa6c9684660857e2ea21b6d18f9",Color.yellow);
        renderer.materials[1].SetColor("Color_330d75a3e4f9473f9cf1c5c316a67af3",Color.black);
        renderer.materials[2].SetColor("Color_009dd960dc6646a6bea6e0e1b2c9c4bb",Color.black);
    }
    
    // Turn the traffic light green
    public void TurnGreen()
    {
        renderer.materials[0].SetColor("Color_6c804fa6c9684660857e2ea21b6d18f9",Color.black);
        renderer.materials[1].SetColor("Color_330d75a3e4f9473f9cf1c5c316a67af3",Color.black);
        renderer.materials[2].SetColor("Color_009dd960dc6646a6bea6e0e1b2c9c4bb",Color.green);
    }
    
    // Turn the traffic light red
    public void TurnRed()
    {
        renderer.materials[0].SetColor("Color_6c804fa6c9684660857e2ea21b6d18f9",Color.black);
        renderer.materials[1].SetColor("Color_330d75a3e4f9473f9cf1c5c316a67af3",Color.red);
        renderer.materials[2].SetColor("Color_009dd960dc6646a6bea6e0e1b2c9c4bb",Color.black);
    }
}
