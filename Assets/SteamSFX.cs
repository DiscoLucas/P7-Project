using UnityEngine;
using JSAM;

public class SteamSFX : MonoBehaviour
{
    public SoundFileObject steam;
    public Transform Transform;

    private void Start()
    {
        AudioManager.PlaySound(steam, Transform.position);
    }

    public void disableSteamSFX()
    {
        AudioManager.StopSound(steam);
    }
}
