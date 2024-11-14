using UnityEngine;
using System;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class GameManager : SingletonPersistent<GameManager>
{
    public static event Action<GameState> OnBeforeStateChanged;
    public static event Action<GameState> OnAfterStateChanged;
    private GameObject playerObject;
    GameObject postProcessing;
    public Volume volume;
    [Header("Menu")]
    [SerializeField]
    private GameHudMenu gameHud;
    Die die;

    Vignette vignette;
    ColorAdjustments colorAdjustments;
    ChromaticAberration chromaticAberration;
    FilmGrain filmGrain;

    [Header("Initial post processing values")]
    [HideInInspector] public float initialVignette;
    [HideInInspector] public Color initialVignetteColor;
    [HideInInspector] public float initialSaturation;
    [HideInInspector] public float initialChromaticAberration;
    [HideInInspector] public float initialFilmGrain;


    public GameState State { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerObject = GameObject.FindGameObjectsWithTag("Player")[0];
        ChangeState(GameState.Starting);
        die = Die.Instance;

        postProcessing = GameObject.Find("Post processing");
        volume = postProcessing.GetComponent<Volume>();

        volume.profile.TryGet(out vignette);
        //volume.profile.TryGet(out vignette.color);
        volume.profile.TryGet(out colorAdjustments);
        volume.profile.TryGet(out chromaticAberration);
        volume.profile.TryGet(out filmGrain);

        if (vignette != null)
        {
            initialVignette = vignette.intensity.value;
            initialVignetteColor = vignette.color.value;
        }

        if (colorAdjustments != null)
        {
            initialSaturation = colorAdjustments.saturation.value;
        }

        if (chromaticAberration != null)
        {
            initialChromaticAberration = chromaticAberration.intensity.value;
        }

        if (filmGrain != null)
        {
            initialFilmGrain = filmGrain.intensity.value;
        }
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
