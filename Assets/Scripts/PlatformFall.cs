using UnityEngine;
[RequireComponent(typeof(Rigidbody))]

public class PlatformFall : MonoBehaviour
{
    Rigidbody rb;
    public Collider platformCollider;
    public float fallDelay = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        platformCollider = GetComponent<Collider>();
        rb.useGravity = false;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player")) Invoke(nameof(CollapsePlatform), fallDelay);
    }

    void CollapsePlatform()
    {
        gameObject.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
        rb.useGravity = true;
        // TODO: play sound effect
    }
}
