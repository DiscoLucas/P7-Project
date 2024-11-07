using UnityEngine;

public class Glow : MonoBehaviour
{
    public Material[] glowMats;
    public float glowAmount;
    public float glowRate;
    float glow;

    // Update is called once per frame
    void Update()
    {
        glow+= glowRate*Time.deltaTime;
        foreach(var mat in glowMats)
        {
            mat.SetFloat("_GlowAmount", Mathf.Sin(glow)*glowAmount);
        }
        
    }
}
