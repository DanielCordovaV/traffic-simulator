using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessShaderProperties : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;
    
    void TurnYellow()
    {
        renderer.materials[0].SetColor("Color_6c804fa6c9684660857e2ea21b6d18f9",Color.yellow);
        renderer.materials[1].SetColor("Color_330d75a3e4f9473f9cf1c5c316a67af3",Color.black);
        renderer.materials[2].SetColor("Color_009dd960dc6646a6bea6e0e1b2c9c4bb",Color.black);
    }
    
    void TurnGreen()
    {
        renderer.materials[0].SetColor("Color_6c804fa6c9684660857e2ea21b6d18f9",Color.black);
        renderer.materials[1].SetColor("Color_330d75a3e4f9473f9cf1c5c316a67af3",Color.black);
        renderer.materials[2].SetColor("Color_009dd960dc6646a6bea6e0e1b2c9c4bb",Color.green);
    }
    
    void TurnRed()
    {
        renderer.materials[0].SetColor("Color_6c804fa6c9684660857e2ea21b6d18f9",Color.black);
        renderer.materials[1].SetColor("Color_330d75a3e4f9473f9cf1c5c316a67af3",Color.red);
        renderer.materials[2].SetColor("Color_009dd960dc6646a6bea6e0e1b2c9c4bb",Color.black);
    }
}
