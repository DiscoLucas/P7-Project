using UnityEngine;
[DefaultExecutionOrder(-199)]
public class GameManagerActivator : MonoBehaviour
{
    public GameObject monster_Goap, monster_Next, monster_Meme;
    public bool enableLogging;
    void Start()
    {
        GameManager.Instance.gmLogging = enableLogging;
        switch (GameManager.Instance.getBotType())
        {
            case BotType.Goap:
                GameManager.Instance.monsterObject = monster_Goap;
                Debug.Log("Change to the bot to Goap");
                break;
            case BotType.NextBot:
                GameManager.Instance.monsterObject = monster_Next;
                Debug.Log("Change to the nextbot");
                break;
            case BotType.MemeBot:
                GameManager.Instance.monsterObject = monster_Meme;
                Debug.Log("Change to the meme bot");
                break;
        }


        GameManager.Instance.onGameStart();
        Debug.Log("Protection areas object is: " + GameManager.Instance.protectionAreaObject + " Player is: " + GameManager.Instance.playerObject + " Monster is: " + GameManager.Instance.monsterObject);
        Debug.Log("postion of player: " + GameManager.Instance.playerObject.transform.position);
        monster_Goap.SetActive(false);
        monster_Next.SetActive(false);



    }
}
