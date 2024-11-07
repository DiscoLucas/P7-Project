using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenuLogic : MonoBehaviour
{
    public TMP_InputField datalogName_InputField;
    public void loadScene(int num)
    {
        SceneManager.LoadScene(num); //set this to something we decide when building
    }

    public void exitApplication() {
        Debug.LogWarning("Due to being play in editor the application can not exit");
        Application.Quit();
    }

    public void updateDataloggerName() { 
        string name = datalogName_InputField.text;
        Debug.Log("Name change to: " + name);
    }

    public void createNewDataloggerCollection() {
        Debug.Log("Create a new datalogger collection");
    }
}
