using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.UI;
public class MainMenuLogic : MenuBase
{
    public TMP_InputField datalogName_InputField;


    

    public void updateDataloggerName() { 
        string name = datalogName_InputField.text;
        Debug.Log("Name change to: " + name);
    }

    public void createNewDataloggerCollection() {
        Debug.Log("Create a new datalogger collection");
    }
    





}
