using UnityEngine;
using UnityEngine.Events;

public class Clicker : MonoBehaviour
{
    [SerializeField] UnityEvent Event;
    [Space]
    [SerializeField] GameObject KeyHolder;
    [SerializeField] GameObject Key;

    public void sexualStyle() {
        if (Key != null)
        {
            if (Key.transform.parent == KeyHolder.transform)
            {
                Event.Invoke();
            }
            else
            {
                //display some error message
            }
        }
        else 
        {
            Event.Invoke();
        }
    }
}
