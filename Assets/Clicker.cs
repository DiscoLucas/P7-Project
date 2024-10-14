using UnityEngine;
using UnityEngine.Events;

public class Clicker : MonoBehaviour
{
    [SerializeField] UnityEvent Event;

    public void sexualStyle() { 
        Event.Invoke();
    }
}
