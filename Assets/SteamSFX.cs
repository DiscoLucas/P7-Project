using UnityEngine;
using JSAM;

public class SteamSFX : MonoBehaviour
{
    public SoundFileObject steam;
    public SoundFileObject valve;
    public Transform Transform;
    public Transform valvePosition;

    private void Start()
    {
        AudioManager.PlaySound(steam, transform);
    }

    public void disableSteamSFX()
    {
        AudioManager.StopSound(steam);
    }

    public void playValveSound()
    {
        AudioManager.PlaySound(valve, valvePosition);
    }
}
