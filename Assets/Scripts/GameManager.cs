using UnityEngine;
using System;
using Assets.Scripts.GOAP.Behaviors;
using Assets.Scripts.GOAP.Sensors;
using Unity.VisualScripting;
using UnityEngine.Events;
[DefaultExecutionOrder(-1)]
public class GameManager : SingletonPersistent<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public GameObject playerObject, monsterObject, protectionAreaObject;
    public UnityEvent onGameStartEvent;
    [Header("Menu")]
    [SerializeField]
    private GameHudMenu gameHud;
    Die die;

    public bool usingNextBot;
    [SerializeField]
    string monsterTag = "Monster", eggTag = "EGG";

    public GameState State { get; private set; }

    protected override void onDuplicateInstanceDestroyed()
    {
        GameManager.Instance.onGameStart();
    }

    private void Awake()
    {
        onGameStart();
    }


    public void onGameStart(){
        Debug.Log("Game startede");
        if(protectionAreaObject == null)
            protectionAreaObject = GameObject.FindGameObjectWithTag(eggTag);
        Debug.Log("Tried to find protection area");
        if (gameHud == null)
            gameHud = GameObject.FindAnyObjectByType<GameHudMenu>();
        if (playerObject == null)
            playerObject = GameObject.FindGameObjectsWithTag("Player")[0];

        ChangeState(GameState.Starting);
        die = Die.Instance;

        if (monsterObject == null) {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag(monsterTag);

            foreach (GameObject monster in monsters)
            {
                if (usingNextBot)
                {
                    DumbBot bot = monster.GetComponent<DumbBot>();
                    if (bot != null)
                    {
                        monsterObject = monster;
                    }
                    else
                    {
                        monster.SetActive(false);
                    }
                }
                else
                {
                    MonsterBrain bot = monster.GetComponent<MonsterBrain>();
                    if (bot != null)
                    {
                        monsterObject = monster;
                    }
                    else
                    {
                        monster.SetActive(false);
                    }
                }
            }
        }

        Debug.Log("Protection areas object is: " + protectionAreaObject + " Player is: " + playerObject + " Monster is: " + monsterObject);
        onGameStartEvent.Invoke();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        onGameStart();

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
                 HandelCloseingGameMenus();
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

    private void HandelCloseingGameMenus()
    {
        
        gameHud.closeAllMenus();
    }

    private void HandleWin()
    {
        gameHud.openWinScreen();
    }

    private void HandleGameOver()
    {
        gameHud.openDeathScreen();
        die.KillPlayer();
    }

    private void HandlePause()
    {
        gameHud.openPauseMenu();
        
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
