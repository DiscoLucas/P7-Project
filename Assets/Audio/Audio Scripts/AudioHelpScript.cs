using UnityEngine;
using JSAM;

public class AudioHelpScript : MonoBehaviour
{
    [SerializeField] MusicFileObject Music;
    [SerializeField] SoundFileObject Sound;
    [SerializeField] private Transform soundTransform;

    void Start()
    {
        AudioManager.PlayMusic(Music);
        AudioManager.PlaySound(Sound, soundTransform);
    }

}
