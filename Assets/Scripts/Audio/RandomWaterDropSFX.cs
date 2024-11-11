using UnityEngine;
using JSAM;

public class RandomWaterDropSFX : MonoBehaviour
{

    [SerializeField] private SoundFileObject waterDropSound;
    [SerializeField] private float minRandomTime = 30f;
    [SerializeField] private float maxRandomTime = 180f;
    private float nextPlayTime; // Stores the next scheduled play time

    private void Start()
    {
        // Initialize the first random interval
        nextPlayTime = Time.time + Random.Range(minRandomTime, maxRandomTime);
    }

    private void FixedUpdate()
    {
        // Check if it's time to play the sound
        if (Time.time >= nextPlayTime)
        {
            AudioManager.PlaySound(waterDropSound, transform.position);

            // Schedule the next sound play after a new random interval
            nextPlayTime = Time.time + Random.Range(minRandomTime, maxRandomTime);

            //Debug.Log("Next Play Time: " + nextPlayTime);
        }
    }
}
