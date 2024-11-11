using UnityEngine;
using JSAM;

public class AudioScriptTest : MonoBehaviour
{
    public KeyCode test_Key;
    public SoundFileObject test;
    public Transform pos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(test_Key))
        {
            AudioManager.PlaySound(test, pos.position);
            Debug.Log("This sound " + test + " is playing");

        }
    }
}
