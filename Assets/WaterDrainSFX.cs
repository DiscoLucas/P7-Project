using UnityEngine;
using JSAM;

public class WaterDrainSFX : MonoBehaviour
{
    [SerializeField] private SoundFileObject waterDrainSFX;
    [SerializeField] private Transform waterDrainTransform;

    public void PlayWaterDrainSFX()
    {
        AudioManager.PlaySound(waterDrainSFX, waterDrainTransform.position);
        Debug.Log("Water drain sound effect played");
    }
}
