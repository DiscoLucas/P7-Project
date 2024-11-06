using UnityEngine;

public class Glow : MonoBehaviour
{
    public Material glowMat;
    public float glowAmount;
    public float glowRate;
    float glow;

    // Update is called once per frame
    void Update()
    {
        glow+= glowRate*Time.deltaTime;
        glowMat.SetFloat("_GlowAmount", Mathf.Sin(glow)*glowAmount);
    }
}
