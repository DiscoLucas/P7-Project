using UnityEngine;

public class GameHudMenu : MenuBase
{
    [SerializeField]
    Transform pauseMenu;
    [SerializeField]
    Animation  deathScreen, winScreen;
    public void openPauseMenu() {
        changeToMenuState();
        Time.timeScale = 0;
        pauseMenu.gameObject.SetActive(true);
        
    }

    protected override void closeMenus()
    {
        closeAllMenus();
    }

    public void startGameWinAction() {
        GameManager.Instance.ChangeState(GameState.Win);
    }

    public void endTurtoiale() {
        GameManager.Instance.ChangeState(GameState.Game);
    }

    public void closePauseMenu() {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        GameManager.Instance.ChangeState(GameState.Game);
    }

    public void openDeathScreen() {
        Time.timeScale = 0.5f;
        changeToMenuState();
        deathScreen.gameObject.SetActive(true);
        deathScreen.Play();
    }

    public void openWinScreen() {
        changeToMenuState();
        winScreen.gameObject.SetActive(true);
        winScreen.Play();
    }

    void changeToMenuState() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void closeAllMenus() {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        pauseMenu.gameObject.SetActive(false);
        deathScreen.gameObject.SetActive(false);
        winScreen.gameObject.SetActive(false);
    }
}
