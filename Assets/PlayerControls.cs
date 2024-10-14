using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            RaycastHit hit;
            float rayDistance = 5f; // Set a reasonable ray distance (you can adjust this value)

            // Shoot a ray from the camera's position in the forward direction
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance))
            {
                // Check if the object hit has the "Trigger" tag
                if (hit.collider.CompareTag("Trigger"))
                {
                    Clicker clicker = hit.collider.gameObject.GetComponent<Clicker>();
                    if (clicker != null)
                        clicker.sexualStyle();
                    // Your logic when the player is looking at a GameObject tagged as "Trigger"
                    Debug.Log("Player is looking at a Trigger object." + clicker.gameObject.name);
                }
            }
        }
    }
}
