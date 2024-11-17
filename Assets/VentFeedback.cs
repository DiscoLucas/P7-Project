using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;


public class VentFeedback : MonoBehaviour
{
    public VentFeedback linkedVent;
    public Light ventLight;
    Light exitLight;
    public Transform linkTarget;
    public float flickerDuration = 2;
    public float minFlickerInterval = 0.05f;
    public float maxFlickerInterval = 0.2f;

    private bool isExitVent; // Tracks if the vent is being used as an exit
    public bool isVenting; // true when the monster is in the vent

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ventLight = GetComponentInChildren<Light>();
        ventLight.enabled = false;
        exitLight = linkedVent.GetComponentInChildren<Light>();
    }
    /// <summary>
    /// Is called when the monster triggers the vent collider
    /// </summary>
    public void OnMonsterEnter()
    {
        isExitVent = false;
        isVenting = true;
        linkedVent.isVenting = true; // letting ya boi know that the vent is being used
        StartCoroutine(FlickerLight());
        Debug.Log("Monster is in the vent");
        /*if (linkedVent != null)
        {
            linkedVent.OnMonsterExit();
        }*/
}
    /// <summary>
    /// is called when the trigger is entered and isVenting is true
    /// </summary>
    public void OnMonsterExit()
    {
        Debug.Log("Monster is out of the vent");
        isExitVent = true;
        StopCoroutine(FlickerLight());
        ventLight.enabled = false;
        isVenting = false;
        linkedVent.isVenting = false;
    }

    private IEnumerator FlickerLight()
    {
        if (exitLight == null) yield break;

        float elapsedTime = 0;
        while (elapsedTime < flickerDuration)
        {
            exitLight.enabled = !exitLight.enabled;
            yield return new WaitForSeconds(Random.Range(minFlickerInterval, maxFlickerInterval));

            elapsedTime += Random.Range(minFlickerInterval, maxFlickerInterval); // Might be a bit too random, we'll see.
        }
        
        exitLight.enabled = false;
        StartCoroutine(FlickerLight()); // recursive call 😳
    }

}
