using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public void SwitchOpenState()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }else
            gameObject.SetActive(true);
    }
}
