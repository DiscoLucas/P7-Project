using UnityEngine;

public class LocalReflectionValues : MonoBehaviour
{
    public Material shaderMat;

    // Update is called once per frame
    void Update()
    {
        shaderMat.SetVector("_LightPos", new Vector4(transform.position.x, transform.position.y, transform. position.z, 0f));
    }
}
