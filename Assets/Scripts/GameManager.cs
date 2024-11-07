using UnityEngine;
using System;

public class GameManager : SingletonPersistent<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ChangeState(GameState.Starting);
    }

    public void ChangeState(GameState newState)
    {
        if (State == newState) return;

        OnBeforeStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.MainMenu:
                break;
            case GameState.PauseMenu:
                HandlePause();
                break;
            case GameState.TutorialSection:
                break;
            case GameState.Game:
                break;
            case GameState.GameOver:
                HandleGameOver();
                break;
            case GameState.Win:
                HandleWin();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }
        OnAfterStateChanged?.Invoke(newState);
        Debug.Log($"Game State changed to {newState}");
    }

    private void HandleWin()
    {
        throw new NotImplementedException();
    }

    private void HandleGameOver()
    {
        throw new NotImplementedException();
    }

    private void HandlePause()
    {
        throw new NotImplementedException();
    }

    private void HandleStarting()
    {
        throw new NotImplementedException();
    }
}

[Serializable]
public enum GameState
{
    Starting,
    MainMenu,
    PauseMenu,
    TutorialSection,
    Game,
    GameOver,
    Win
}
