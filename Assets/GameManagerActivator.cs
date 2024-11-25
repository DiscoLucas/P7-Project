using UnityEngine;
[DefaultExecutionOrder(-2)]
public class GameManagerActivator : MonoBehaviour
{
    public GameObject monster_Goap, monster_Next;
    void Start()
    {
        if (GameManager.Instance.usingNextBot)
        {
            GameManager.Instance.monsterObject = monster_Next;
            Debug.Log("Change to the nextbot");
        }
        else {
            GameManager.Instance.monsterObject = monster_Goap;
            Debug.Log("Change to the Goap");
        }

        
        GameManager.Instance.onGameStart();
        Debug.Log("Protection areas object is: " + GameManager.Instance.protectionAreaObject + " Player is: " + GameManager.Instance.playerObject + " Monster is: " + GameManager.Instance.monsterObject);
        monster_Goap.SetActive(false);
        monster_Next.SetActive(false);



    }
}
