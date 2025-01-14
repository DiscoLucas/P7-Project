using UnityEngine;

public class MemeLvlBtn : MonoBehaviour
{
    public int clickCount = 0;
    MainMenuLogic mainMenuLogic;
    GameObject debugMenu;
    GameObject loadingScreen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainMenuLogic = FindObjectOfType<MainMenuLogic>();
        debugMenu = GameObject.Find("DebugMenu");
        loadingScreen = GameObject.Find("Loadning screen");
    }

    

    public void OnClick()
    {
        clickCount++;
        if (clickCount == 5)
        {
            clickCount = 0;
            mainMenuLogic.SetBotTypeEnum(2);
            mainMenuLogic.gameObject.SetActive(false);
            debugMenu.SetActive(false);
            loadingScreen.SetActive(true);
            mainMenuLogic.loadScene(2);
        }
    }
}
