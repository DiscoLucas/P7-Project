using UnityEngine;
using JSAM;
using System.Collections.Generic;

public class LeverSoundActivation : MonoBehaviour
{
    [SerializeField] private SoundFileObject leverAct;
    [SerializeField] private SoundFileObject openDoorSound;
    [SerializeField] private SoundFileObject closeDoorSound;
    [SerializeField] private Transform LeverPosition;
    [SerializeField] private Transform DoorPosition;

    public void ActivateLeverSound()
    {
        AudioManager.PlaySound(leverAct, LeverPosition.position);
        //Debug.Log("This sound " + leverAct + " is playing at " + LeverPosition);
    }

    public void OpenDoorSound()
    {
        AudioManager.PlaySound(openDoorSound, DoorPosition.position);
        //Debug.Log("This sound " + doorSound + " is playing at " + DoorPosition);
    }

    public void CloseDoorSound()
    {
        AudioManager.PlaySound(closeDoorSound, DoorPosition.position);
        //Debug.Log("This sound " + doorSound + " is playing at " + DoorPosition);
    }

}
