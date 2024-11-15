using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections;

public class PlatformFall : MonoBehaviour
{
    List<Rigidbody> rb;
    public Collider platformCollider;
    public float fallDelay = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = new List<Rigidbody>();

        //rb = GetComponent<Rigidbody>();
        platformCollider = GetComponent<Collider>();
        

        // get rigidbody from all of the children
        GetComponentsInChildren<Rigidbody>(true, rb);

        foreach (var r in rb)
        {
            r.useGravity = false;
            r.isKinematic = true;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {

            GameManager.Instance.ChangeState(GameState.Starting);
            foreach (var r in rb)
            {
                CollapsePlatform(r, fallDelay);
            }
        }
            
    }

    IEnumerable CollapsePlatform(Rigidbody rb, float delayTime)
    {
        //gameObject.transform.localScale = new Vector3(0.99f, 0.99f, 0.99f);
        rb.useGravity = true;
        rb.isKinematic = false;
        // TODO: play sound effect

        return null;
    }
}
