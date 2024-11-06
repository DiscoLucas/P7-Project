using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void LoadSomething(int num)
    {
        SceneManager.LoadScene(num); //set this to something we decide when building
    }
}
