using UnityEngine;
using UnityEngine.Events;

public class Clicker : MonoBehaviour
{

    //[SerializeField] public GameObject[] OpenDoors;
    //[SerializeField] public GameObject[] CloseDoors;
    //[Space]
    [SerializeField] public GameObject Player;
    [SerializeField] UnityEvent Event;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void sexualStyle() { 
        Event.Invoke();
    }
}
