using UnityEngine;
using UnityEngine.Rendering;
using System;
using UnityEngine.Rendering.HighDefinition;
using System.Collections;

public class Die : Singleton<Die>
{
    // Component references
    Volume volume;
    GameObject player;
    GameObject postProcessing;
    public GameObject cameraObj;
    GameManager gameManager;
    public Transform offsetPosition;
    Rigidbody rb;
    SphereCollider sphereCollider;

    Vector3 cameraOffset;

    float vignetteStart;
    float satturationStart;
    float choromaStart;
    float filmGrainStart;

    float currentVignette;
    float currentSaturation;
    float currentChoroma;
    float currentFilmGrain;

    public float transitionTime = 1f;

    [Header("Death Effects")]
    public float vignetteEnd = 0.5f;
    public float saturationEnd = -80;
    public float choromaEnd = 1;
    public float filmGrainEnd = 1;
    public float tiltForce = 1; // Force applied to the camera when the player dies
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.Instance;
        postProcessing = GameObject.Find("Post processing");
        volume = postProcessing.GetComponent<Volume>();
        rb = cameraObj.GetComponent<Rigidbody>();
        sphereCollider = cameraObj.GetComponent<SphereCollider>();
        sphereCollider.enabled = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        cameraOffset = offsetPosition.position;

        cameraOffset = rb.transform.InverseTransformPoint(Camera.main.transform.position);

        cameraObj = GameObject.FindWithTag("MainCamera");
        player = GameObject.FindWithTag("Player");

        // save the initial values of the post processing effects
        if (volume.profile.TryGet(out Vignette vignette))
            vignetteStart = vignette.intensity.value;
        
        else Debug.LogWarning("Vignette not found in volume profile");

        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
            satturationStart = colorAdjustments.saturation.value;
        
        else Debug.LogWarning("ColorAdjustments not found in volume profile");

        if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
            choromaStart = chromaticAberration.intensity.value;
        
        else Debug.LogWarning("ChromaticAberration not found in volume profile");

        if (volume.profile.TryGet(out FilmGrain filmGrain))
            filmGrainStart = filmGrain.intensity.value;
        
        else Debug.LogWarning("FilmGrain not found in volume profile");
    }

    private void LateUpdate()
    {
        if (gameManager.State == GameState.GameOver)
        {
            Camera.main.transform.position = rb.transform.TransformPoint(cameraOffset);
            Camera.main.transform.rotation = rb.transform.rotation;
        }
    }

    /// <summary>
    /// Post processing effect for when the player is close to the bot.
    /// It will increase the intensity of the effects as the player gets closer.
    /// </summary>
    /// <param name="playerDistance">Distance from the bot to the player.</param>
    /// <param name="insanityRange">How close the player needs to be for the effect to start.</param>
    public void SanityEffect(float playerDistance, float insanityRange = 10f)
    {
        if (playerDistance < insanityRange)
        {
            float t = 1 - (playerDistance / insanityRange); // Closer distance increases effect intensity

            gameManager.SetVignetteIntensity(Mathf.Lerp(vignetteStart, vignetteEnd, t));
            gameManager.SetColorSaturation(Mathf.Lerp(satturationStart, saturationEnd, t));
            gameManager.SetChromaticAberration(Mathf.Lerp(choromaStart, choromaEnd, t));
            gameManager.SetFilmGrainIntensity(Mathf.Lerp(filmGrainStart, filmGrainEnd, t));

        }
        else
        {
            // Reset effects to starting values when player is out of range
            gameManager.SetVignetteIntensity(vignetteStart);
            gameManager.SetColorSaturation(satturationStart);
            gameManager.SetChromaticAberration(choromaStart);
            gameManager.SetFilmGrainIntensity(filmGrainStart);
        }
    }

    public void KillPlayer()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        sphereCollider.enabled = true;
        cameraObj.transform.SetParent(null);

        // kinda goofy ngl, but it works for now
        Vector3 tiltDirection = -cameraObj.transform.forward;
        rb.AddTorque(tiltDirection * tiltForce, ForceMode.Impulse);

        if (volume.profile.TryGet(out Vignette vignette))
            currentVignette = vignette.intensity.value;

        if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
            currentSaturation = colorAdjustments.saturation.value;

        if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
            currentChoroma = chromaticAberration.intensity.value;
        
        if (volume.profile.TryGet(out FilmGrain filmGrain))
            currentChoroma = filmGrain.intensity.value;


        // Transition to the death effects
        StartCoroutine(TransitionToDeathEffects());
        DisableAllComponentsButThis();
    }

    private IEnumerator TransitionToDeathEffects()
    {
        float elapsedTime = 0;

        while (elapsedTime < transitionTime)
        {
            float vignetteValue = Mathf.Lerp(currentVignette, vignetteEnd, elapsedTime / transitionTime);
            float saturationValue = Mathf.Lerp(currentSaturation, saturationEnd, elapsedTime / transitionTime);
            float choromaValue = Mathf.Lerp(currentChoroma, choromaEnd, elapsedTime / transitionTime);
            float filmGrainValue = Mathf.Lerp(currentFilmGrain, filmGrainEnd, elapsedTime / transitionTime);

            if (volume.profile.TryGet(out Vignette vignette))
                vignette.intensity.value = vignetteValue;

            if (volume.profile.TryGet(out ColorAdjustments colorAdjustments))
                colorAdjustments.saturation.value = saturationValue;

            if (volume.profile.TryGet(out ChromaticAberration chromaticAberration))
                chromaticAberration.intensity.value = choromaValue;

            if (volume.profile.TryGet(out FilmGrain filmGrain))
                filmGrain.intensity.value = filmGrainValue;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // make sure the effects are set to the final values
        if (volume.profile.TryGet(out Vignette finalVignette))
            finalVignette.intensity.value = vignetteEnd;

        if (volume.profile.TryGet(out ColorAdjustments finalColorAdjustments))
            finalColorAdjustments.saturation.value = saturationEnd;

        if (volume.profile.TryGet(out ChromaticAberration finalChromaticAberration))
            finalChromaticAberration.intensity.value = choromaEnd;

        if (volume.profile.TryGet(out FilmGrain finalFilmGrain))
            finalFilmGrain.intensity.value = filmGrainEnd;
    }

    void DisableAllComponentsButThis()
    {

        Component[] components = GetComponents<Component>();

        foreach (Component component in components)
        {
            // Skip this script
            if (component == this)
                continue;

            // Disable enabled components except this script
            if (component is Behaviour behaviour)
            {
                behaviour.enabled = false;
            }
        }
        var cc = GetComponent<CharacterController>();
        cc.enabled = false;
        var capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        var MeshRenderer = GetComponent<MeshRenderer>();
        MeshRenderer.enabled = false;
    }
}
