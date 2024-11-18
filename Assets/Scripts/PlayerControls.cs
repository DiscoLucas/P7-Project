using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public GameObject Hand;
    public float interactCoolDown;
    public float interactCoolDownTarget = 0.5f;
    // Update is called once per frame
    void Update()
    {
        if(interactCoolDown<interactCoolDownTarget)
        interactCoolDown+=Time.deltaTime;
        if (Input.GetKeyDown("e") && interactCoolDown>interactCoolDownTarget)
        {
            RaycastHit hit;
            float rayDistance = 5f; // Set a reasonable ray distance (you can adjust this value)

            // Shoot a ray from the camera's position in the forward direction
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayDistance) && hit.collider.CompareTag("Trigger"))
            {
                interactCoolDown=0;
                // If we hit something with the trigger tag
                Clicker clicker = hit.collider.gameObject.GetComponent<Clicker>();
                if (clicker != null)
                    clicker.sexualStyle();
            
            }//If we miss & is holding something
            else if(Hand.transform.childCount!=0)
            {   //Drop item
                GameObject HeldItem = Hand.transform.GetChild(0).gameObject;
                Hand.transform.GetChild(0).gameObject.GetComponent<Clicker>().Egress.Invoke();
                HeldItem.gameObject.layer = 7;
                HeldItem.transform.rotation = Quaternion.identity;
                HeldItem.transform.position = Camera.main.transform.position;
                HeldItem.transform.parent = null;
                if (HeldItem.GetComponent<Rigidbody>() != null && HeldItem.GetComponent<Collider>() != null)
                {
                    HeldItem.GetComponent<Rigidbody>().isKinematic = false;
                    HeldItem.GetComponent<Collider>().enabled = true;
                }
            }
            else
            {
                //dont do much ?? Toggles flashlight maybe???
            }
        }
    }

    public void destoryCurrentHeldItem() {
        GameObject HeldItem = Hand.transform.GetChild(0).gameObject;
        if(HeldItem != null)
            Destroy(HeldItem);
    }
}
