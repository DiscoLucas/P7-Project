using UnityEngine;

public class Pickupable : MonoBehaviour
{
    Rigidbody Rigidbody;
    Collider Collider;
    public GameObject playerHand;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
    }

    public void PickUp()
    {
        transform.parent = playerHand.transform;
        Collider.enabled = false;
        Rigidbody.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        gameObject.layer = 5;
    }

    public void Drop()
    {
        transform.parent = null;
        Collider.enabled = true;
        Rigidbody.isKinematic = false;
        gameObject.layer = 0;
    }
}
