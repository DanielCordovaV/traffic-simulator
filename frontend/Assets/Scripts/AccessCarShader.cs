using UnityEngine;

public class AccessCarShader : MonoBehaviour
{
    [SerializeField] private MeshRenderer renderer;

    // Function that is used to change the color of cars when they are instantiated
    public void ChangeColor()
    {
        var tmpColor = Random.ColorHSV(0f, 1f, 0f, 1f, 0f, 1f);
        renderer.materials[0].SetColor("Color_fcd43df70ecb490e974b0f0858d7a5be",tmpColor);
    }
}
