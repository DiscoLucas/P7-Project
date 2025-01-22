using UnityEngine;

public class LoggingActivator : MonoBehaviour
{
    public bool enableLogging;
    
    public void OnClick(bool input)
    {
        GameManager.Instance.gmLogging = input;
        Debug.Log("Logging is " + (GameManager.Instance.gmLogging ? "enabled" : "disabled"));
    }
}
