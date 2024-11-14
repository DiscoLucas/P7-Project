using UnityEngine;
[DefaultExecutionOrder(10)]
public class SessionLogState : MonoBehaviour
{
    //
    public bool sessionStarted = false;
    public bool onSessionStart = false;

    public void Start()
    {
        Debug.Log("Trying To change the state variable in the logger");
        if(SessionLogTracker.Instance.state == null)
            SessionLogTracker.Instance.state = this;
        Debug.Log(SessionLogTracker.Instance.state.gameObject.name);
    }
}
