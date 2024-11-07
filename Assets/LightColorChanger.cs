using UnityEngine;

public class LightColorChanger : MonoBehaviour
{
    Material myMaterial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMaterial = this.transform.GetComponent<Renderer>().materials[1];
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.SetColor("_EmissionColor", Color.red * 10.0f);
    }
}
