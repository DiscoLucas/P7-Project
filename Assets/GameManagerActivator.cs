using UnityEngine;
[DefaultExecutionOrder(-2)]
public class GameManagerActivator : MonoBehaviour
{
    void Start()
    {
        GameManager.Instance.onGameStart();
    }
}
