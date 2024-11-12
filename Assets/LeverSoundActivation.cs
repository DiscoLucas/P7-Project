using UnityEngine;
using JSAM;

public class LeverSoundActivation : MonoBehaviour
{
    [SerializeField] private SoundFileObject leverAct;
    [SerializeField] private Transform LeverPosition;

    public void ActivateLeverSound()
    {
        AudioManager.PlaySound(leverAct, LeverPosition.position);
        Debug.Log("This sound " + leverAct + " is playing at " + LeverPosition);
    }

}
