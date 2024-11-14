using UnityEngine;
using JSAM;
using System.Collections.Generic;

public class LeverSoundActivation : MonoBehaviour
{
    [SerializeField] private SoundFileObject leverAct;
    [SerializeField] private SoundFileObject doorSound;
    [SerializeField] private Transform LeverPosition;
    [SerializeField] private Transform DoorPosition;

    public void ActivateLeverSound()
    {
        AudioManager.PlaySound(leverAct, LeverPosition.position);
        //Debug.Log("This sound " + leverAct + " is playing at " + LeverPosition);
    }

    public void ActivateDoorSound()
    {
        AudioManager.PlaySound(doorSound, DoorPosition.position);
        //Debug.Log("This sound " + doorSound + " is playing at " + DoorPosition);
    }

}
