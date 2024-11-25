using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using JSAM;


public class VentFeedback : MonoBehaviour
{
    public VentFeedback linkedVent;
    public Light ventLight;
    Light exitLight;
    public Transform linkTarget;
    Collider ventCollider;
    public float flickerDuration = 2;
    public float minFlickerInterval = 0.05f;
    public float maxFlickerInterval = 0.2f;
    public SoundFileObject rumblingSound;

    private bool isExitVent; // Tracks if the vent is being used as an exit
    public bool isVenting; // true when the monster is in the vent

    [SerializeField]
    private SoundFileObject ventEnter;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ventLight = GetComponentInChildren<Light>();
        ventLight.enabled = false;
        exitLight = linkedVent.GetComponentInChildren<Light>();
        ventCollider = GetComponent<Collider>();
    }
    /// <summary>
    /// Is called when the monster triggers the vent collider
    /// </summary>
    public void OnMonsterEnter()
    {
        if (isExitVent) return;

        isExitVent = false;
        isVenting = true;
        linkedVent.isVenting = true; // letting ya boi know that the vent is being used
        linkedVent.isExitVent = true;
        StartCoroutine(FlickerLight());
        //Debug.Log("Monster is in the vent");
        AudioManager.PlaySound(ventEnter, GameManager.Instance.monsterObject.transform);
        AudioManager.PlaySound(rumblingSound, GameManager.Instance.monsterObject.transform);
}
    /// <summary>
    /// is called when the trigger is entered and isVenting is true
    /// </summary>
    public void OnMonsterExit()
    {
        //Debug.Log("Monster is out of the vent");
        isExitVent = true;
        isVenting = false;
        StopCoroutine(FlickerLight());
        ventLight.enabled = false;
        linkedVent.isVenting = false;
        AudioManager.StopSound(rumblingSound);
    }

    private IEnumerator FlickerLight()
    {
        if (exitLight == null) yield break;

        //float elapsedTime = 0;
        while (isVenting)
        {
            exitLight.enabled = !exitLight.enabled;
            yield return new WaitForSeconds(Random.Range(minFlickerInterval, maxFlickerInterval));

            
        }
        
        exitLight.enabled = false;
        //StartCoroutine(FlickerLight()); // recursive call 😳
    }
    /*
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered");
    }
    */

}
