using UnityEngine;

public class Pickupable : MonoBehaviour
{
    Rigidbody Rigidbody;
    Collider MyCollider;
    public GameObject playerHand;

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        MyCollider = GetComponent<Collider>();
    }

    public void PickUp()
    {
        transform.parent = playerHand.transform;
        MyCollider.enabled = false;
        Rigidbody.isKinematic = true;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
        gameObject.layer = 5;
    }
}
