using UnityEngine;

public class EndOfTurtoialeAction : MonoBehaviour
{
    void Start()
    {
        
    }
    
    public void endTurtoiale() {
        Debug.Log("Turtoial Ended");
        

        GameManager.Instance.ChangeState(GameState.Starting);

    }
}
