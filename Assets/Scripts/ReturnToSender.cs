using UnityEngine;

public class ReturnToSender : MonoBehaviour
{
    public Transform player;
    public float bounds;

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.y > bounds)
            gameObject.transform.position = player.transform.position;
    }
}
