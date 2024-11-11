using UnityEngine;
using UnityEngine.Events;

public class Clicker : MonoBehaviour
{
    public bool switched;
    [SerializeField] UnityEvent Event;
    [SerializeField] UnityEvent EventOpp;
    [Space]
    [SerializeField] GameObject KeyHolder;
    [SerializeField] GameObject Key;
    [SerializeField] Animation MyAnimation;

    public void SwitchState(){
        if(switched){
            switched=false;
        }else{
            switched=true;
        }
    }

    public void sexualStyle() { //This is like a button, that is pressed once and then Stuff A happens. We need it to become like a lever, where you switch it and Stuff A happens. But if you switch it again the opposite of Stuff A happens.
        if (Key != null)
        {
            if (Key.transform.parent == KeyHolder.transform)
            {
                if(switched){
                Event.Invoke();
                }else{
                EventOpp.Invoke();
                }

                if (MyAnimation != null)
                {
                    MyAnimation.Play();
                }
            }
            else
            {
                //display some error message
            }
        }
        else 
        {
            if(switched){
            Event.Invoke();
            }else{
            EventOpp.Invoke();
            }

            if (MyAnimation != null)
            {
                MyAnimation.Play();
            }
        }
    }
}
