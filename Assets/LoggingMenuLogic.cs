using UnityEngine;
using TMPro;
using UnityEditor;

public class LoggingMenuLogic : MonoBehaviour
{
    public TMP_InputField nameInput;

    public void createNewSessionLog()
    {
        SessionLog sl = SessionLogTracker.Instance.createSessionLog();
        if (sl != null)
        {
            nameInput.text = sl.name;
            SessionLogTracker.Instance.sessionLog = sl;
            Debug.Log("New SessionLog created with name: " + sl.name);
        }
    }

    public void updateNameSessionName()
    {
        if (SessionLogTracker.Instance.sessionLog != null)
        {
            SessionLogTracker.Instance.sessionLog.name = nameInput.text;
            Debug.Log("Updated SessionLog name to: " + nameInput.text);
        }
    }
}

