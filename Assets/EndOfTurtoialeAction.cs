using UnityEngine;

public class EndOfTurtoialeAction : MonoBehaviour
{
    public Rigidbody[] rbs;
    public float force = 7;
    void Start()
    {
        foreach (Rigidbody rigidbody in rbs) {
            rigidbody.Sleep();
        }
    }
    
    public void endTurtoiale() {
        Debug.Log("Turtoiale Endede");
        foreach (Rigidbody rigidbody in rbs)
        {
            rigidbody.WakeUp();
            rigidbody.AddForce((-rigidbody.gameObject.transform.up + rigidbody.gameObject.transform.forward) * force);
        }

        GameManager.Instance.ChangeState(GameState.Starting);

        gameObject.SetActive(false);
    }
}
