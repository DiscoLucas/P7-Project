using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform player;
    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.transform; // Assuming your camera is the main camera.
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 lookAtPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.LookAt(lookAtPos);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }
}
