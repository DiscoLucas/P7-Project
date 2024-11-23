using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using System.Collections;
using JSAM;

public class PlatformFall : MonoBehaviour
{
    [SerializeField]
    List<Rigidbody> rb;
    public Collider platformCollider;
    public float fallDelay = 3f;
    public float deactivateDelay = 10f;
    [SerializeField] private SoundFileObject platformCrash;
    [SerializeField] private Transform platformCrashTransform;

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
            Debug.Log("End of turtoiale");
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
        rb.AddForce(rb.transform.forward);
        // TODO: play sound effect
        AudioManager.PlaySound(platformCrash, platformCrashTransform.position);
        return null;
    }

    IEnumerable DesiablePlatform(Rigidbody rb, float delayTime)
    {

        rb.useGravity = false;
        rb.isKinematic = true;
        return null;
    }
}
