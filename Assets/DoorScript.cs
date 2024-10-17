using UnityEngine;

public class DoorScript : MonoBehaviour
{
    // WILL MAKE ANIMATIONS WHEN WE GET DOOR ASSETS
    // WILL MAKE SOUND WHEN SOUND MANAGER IS PRESENT
    
    //Legacy code, could be used for switch states puzzle
    public void SwitchOpenState()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }else
            gameObject.SetActive(true);
    }

    public void Open()
    {
        gameObject.SetActive(false); //slå det her ihjel senere
        //Play animation
        //Play sound
    }

    public void Close()
    {
        //Play animation
        //Play sound
    }
}
