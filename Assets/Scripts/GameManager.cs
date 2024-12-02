using UnityEngine;
using System;
using Assets.Scripts.GOAP.Behaviors;
using Assets.Scripts.GOAP.Sensors;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
[DefaultExecutionOrder(-2)]
public class GameManager : SingletonPersistent<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    public GameObject playerObject, monsterObject, protectionAreaObject;
    public UnityEvent onGameStartEvent;
    GameObject postProcessing;
    public Volume volume;
    [Header("Menu")]
    [SerializeField]
    private GameHudMenu gameHud;
    Die die;

    [SerializeField]
    string monsterTag = "Monster", eggTag = "EGG";
    Vignette vignette;
    ColorAdjustments colorAdjustments;
    ChromaticAberration chromaticAberration;
    FilmGrain filmGrain;

    public SceneLoadPackage sceneLoadPackage = null;


    [Header("Initial post processing values")]
    [HideInInspector] public float initialVignette;
    [HideInInspector] public Color initialVignetteColor;
    [HideInInspector] public float initialSaturation;
    [HideInInspector] public float initialChromaticAberration;
    [HideInInspector] public float initialFilmGrain;
    public void setpackage() {
        if (sceneLoadPackage == null) {
            sceneLoadPackage = GetComponent<SceneLoadPackage>();
        }
    }

    public void setUsingNextBot(bool b) {
        setpackage();
        sceneLoadPackage.usingNextBot = b;
    }

    public bool getUsingNextBot() {
        setpackage();

        
        return sceneLoadPackage.usingNextBot;
    }

    public GameState State { get; private set; }

    protected override void onDuplicateInstanceDestroyed()
    {
       //GameManager.Instance.onGameStart();
    }

    public void findMonster() {
        if (monsterObject == null)
        {
            GameObject[] monsters = GameObject.FindGameObjectsWithTag(monsterTag);
            Debug.Log("mONSTERS FOUND: " + monsters.Length);
            foreach (GameObject monster in monsters)
            {
                if (getUsingNextBot())
                {
                    DumbBot bot = monster.GetComponent<DumbBot>();
                    if (bot != null)
                    {
                        Debug.Log("Nextbot found");
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
                        Debug.Log("GoapBot found");
                        monsterObject = monster;
                    }
                    else
                    {
                        monster.SetActive(false);
                    }
                }
            }
            Debug.Log("monster gameobject name " + monsterObject.name);
        }
    }
    public void onGameStart()
    {
        TeensyLogger.Instance.OnStart();
        Debug.Log("Game startede");
        if(protectionAreaObject == null)
            protectionAreaObject = GameObject.FindGameObjectWithTag(eggTag);
        Debug.Log("Tried to find protection area");
        if (gameHud == null)
            gameHud = GameObject.FindAnyObjectByType<GameHudMenu>();
        if (playerObject == null) {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("Player");
            if (objects.Length > 0) {
                playerObject = GameObject.FindGameObjectsWithTag("Player")[0];
            }
            
        }

        GameObject spawnpoint = GameObject.FindWithTag("Spawnpoint");
        if (spawnpoint != null && playerObject != null)
        {
            Debug.Log("Moving player to spawnpoint FROM: " + playerObject.transform.position + " to: " + spawnpoint.transform.position);
            playerObject.transform.position = spawnpoint.transform.position;
            Debug.Log("moved to: " + playerObject.transform.position);
          //  var newobj = Instantiate(playerObject);
           // playerObject.SetActive(false);
           // playerObject = newobj;
        }

        ChangeState(GameState.TutorialSection);
        die = Die.Instance;

        postProcessing = GameObject.Find("Post processing");
        if (postProcessing == null)
        {
            Debug.LogError("Post processing GameObject not found"); 
            return;
        }
        volume = postProcessing.GetComponent<Volume>();
        if (volume == null)
        {
            Debug.LogError("Volume component not found on the Post processing GameObject");
            return;
        }

        if (volume.profile == null)
        {
            Debug.LogError("Volume profile not found on the Volume component");
            return;
        }

        if (volume.profile.TryGet(out vignette))
        {
            initialVignette = vignette.intensity.value;
            initialVignetteColor = vignette.color.value;
        }
        //volume.profile.TryGet(out vignette.color);
        if (volume.profile.TryGet(out colorAdjustments))
        {
            initialSaturation = colorAdjustments.saturation.value;
        }
        if (volume.profile.TryGet(out chromaticAberration))
        {
            initialChromaticAberration = chromaticAberration.intensity.value;
        }
        if (volume.profile.TryGet(out filmGrain))
        {
            initialFilmGrain = filmGrain.intensity.value;
        }
        
        onGameStartEvent.Invoke();
        //findMonster();
        Debug.Log("Protection areas object is: " + protectionAreaObject + " Player is: " + playerObject + " Monster is: " + monsterObject);
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
        
        playerObject.GetComponent<PlayerMovement>().pause = false;
        gameHud.closeAllMenus();
    }

    private void HandleWin()
    {
        if (State != GameState.GameOver) {
            SessionLogTracker.Instance.sessionLog.gameCompletede();
            gameHud.openWinScreen();
        }
    }

    private void HandleGameOver()
    {
        if (State != GameState.Win) {
            SessionLogTracker.Instance.onCountDeath();
            gameHud.openDeathScreen();
            die.KillPlayer();
        }
    }

    private void HandlePause()
    {
        gameHud.openPauseMenu();
        
    }

    private void HandleStarting()
    {
        if (monsterObject != null)
            monsterObject.SetActive(true);
        else {
            Debug.LogError("The monster was not found");
            findMonster();
            monsterObject.SetActive(true);
        }
        if (SessionLogTracker.Instance.state == null) {
            SessionLogTracker.Instance.state = GameObject.FindObjectOfType<SessionLogState>();
        }
        SessionLogTracker.Instance.startLoggin();
        ChangeState(GameState.Game);
    }

    public bool inGame() { 
        return (State == GameState.Game || State == GameState.TutorialSection);
    }

    #region Sketchy post processing getter setters
    public void SetVignetteIntensity(float intensity)
    {
        if (vignette != null) vignette.intensity.value = intensity;
    }

    public void SetVignetteColor(Color color)
    {
        if (vignette != null) vignette.color.Override(color);
    }

    public void SetColorSaturation(float saturation)
    {
        if (colorAdjustments != null) colorAdjustments.saturation.value = saturation;
    }

    public void SetChromaticAberration(float intensity)
    {
        if (chromaticAberration != null) chromaticAberration.intensity.value = intensity;
    }

    public void SetFilmGrainIntensity(float intensity)
    {
       if (filmGrain != null) filmGrain.intensity.value = intensity;
    }

    public float GetVignetteIntensity()
    {
        return vignette != null ? vignette.intensity.value : 0;
    }

    public Color GetVignetteColor()
    {
        return vignette != null ? vignette.color.value : Color.black;
    }

    public float GetColorSaturation()
    {
        return colorAdjustments != null ? colorAdjustments.saturation.value : 0;
    }

    public float GetChromaticAberration()
    {
        return chromaticAberration != null ? chromaticAberration.intensity.value : 0;
    }

    public float GetFilmGrainIntensity()
    {
        return filmGrain != null ? filmGrain.intensity.value : 0;
    }
    #endregion
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
