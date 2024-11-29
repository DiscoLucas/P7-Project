using UnityEngine;
using JSAM;

public class AudioHelpScript : MonoBehaviour
{
    [SerializeField] MusicFileObject Music;
    [SerializeField] SoundFileObject Sound;
    [SerializeField] private Transform soundTransform;
    [SerializeField] SoundFileObject waterFall;
    [SerializeField] private Transform waterFallPos;

    void Start()
    {
        AudioManager.PlayMusic(Music);
        AudioManager.PlaySound(Sound, soundTransform);
        AudioManager.PlaySound(waterFall, waterFallPos);
    }

}
