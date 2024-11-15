using UnityEngine;
using UnityEngine.Events;

public class TriggerEnterAction : MonoBehaviour
{
    public UnityEvent ingress;
    public UnityEvent egress;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            ingress.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            egress.Invoke();
    }
}
