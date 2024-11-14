using UnityEngine;
using TMPro;
using UnityEditor;
[DefaultExecutionOrder(10)]
public class LoggingMenuLogic : MonoBehaviour
{
    public TMP_InputField nameInput;
    public Transform textInputBox;

    private void Start()
    {
        if(SessionLogTracker.Instance.sessionLog == null)
            createNewSessionLog();
    }

    public void createNewSessionLog()
    {
        SessionLog sl = SessionLogTracker.Instance.createSessionLog();
        Debug.Log("Are the sesionLog null?: " + (sl == null));
        if (sl != null)
        {
            nameInput.text = sl.name;
            SessionLogTracker.Instance.sessionLog = sl;
            textInputBox.gameObject.SetActive(true);
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

