using UnityEngine;
using UnityEngine.Events;

public class Clicker : MonoBehaviour
{
    public bool switched;
    [SerializeField] public UnityEvent Ingress; //they are now public
    public UnityEvent Egress; //This would be in a lever inheritance!
    [Space]
    [SerializeField] GameObject KeyHolder;
    [SerializeField] GameObject Key;

    public void SwitchState(){  //This would be in a lever inheritance
        if(switched){
            switched=false;
        }else{
            switched=true;
        }
    }

    public void sexualStyle() { //This is like a button, that is pressed once and then Stuff A happens. We need it to become like a lever, where you switch it and Stuff A happens. But if you switch it again the opposite of Stuff A happens.
        if (Key != null) //Do the thing if player has key
        {
            if (Key.transform.parent == KeyHolder.transform)
            {
                if(!switched){  //This would be in a lever inheritance, but will stay in a simpler form originally
                Ingress.Invoke();
                }else{
                Egress.Invoke();
                }
            }
        }
        else //Do the thing regardlessly, because no key exists
        {
            if(!switched){  //This would be in a lever inheritance, but will stay in a simpler form originally
            Ingress.Invoke();
            }else{
            Egress.Invoke();
            }
        }
    } //bruh
}
