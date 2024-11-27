using UnityEngine;

public class LocalReflectionValues : MonoBehaviour
{
    public Material[] shaderMats;

    // Update is called once per frame
    void Update()
    {
        foreach(Material material in shaderMats){
            material.SetVector("_LightPos", new Vector4(transform.position.x, transform.position.y, transform. position.z, 0f));
        }
    }
}
