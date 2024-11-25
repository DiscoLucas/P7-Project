using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using JSAM;

public class HealthSystem : MonoBehaviour
{
    public int currentHealth;
    public int maxHealth = 100;

    [Header("Damage")]
    public float damageInterval = 0.5f;
    public int steamDamage = 5;
    public float shakeDuration = 0.1f;
    public float shakeMagnitude = 0.05f;
    bool isTakingDamage;
    private bool isDead;
    [SerializeField] private SoundFileObject damageSound;

    [Header("Regen")]
    public float regenDelay = 1;
    public float regenInterval = 1f/30f;
    public int regenAmount = 2;
    public bool canRegen;

    [Header("References")]
    GameObject player;
    GameManager gameManager;
    Camera mainCamera;

    [Header("Low-pass Filter")]
    public AudioLowPassFilter lowPassfitlerAudio;
    public float minLowPassFilterCutoff = 500;
    public float maxLowPassFilterCutoff = 22000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        gameManager = GameManager.Instance;
        mainCamera = Camera.main;


        currentHealth = maxHealth;
    }

    private void OnTriggerEnter(Collider damageSource)
    {
        if (damageSource.CompareTag("Steam"))
        {
            if (!isTakingDamage)
                StartCoroutine(ApplyContinuousDamage(steamDamage));
        }
    }

    private void OnTriggerExit(Collider damageSource)
    {
        isTakingDamage = false;
        StartCoroutine(Regen()); // start regen after not being a dumbass
    }
    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (!AudioManager.IsSoundPlaying(damageSound))
        {
            AudioManager.PlaySound(damageSound);
        }
        UpdateVignetteColor();
        StopCoroutine(Regen()); // Stop regen if taking damage
        StartCoroutine(ScreenShake());
        if (currentHealth <= 0)
        {
            gameManager.ChangeState(GameState.GameOver);
            isDead = true;
        }
    }

    public IEnumerator ApplyContinuousDamage(int damage)
    {
        isTakingDamage = true;

        while (isTakingDamage && !isDead)
        {
            TakeDamage(damage);
            yield return new WaitForSeconds(damageInterval);
        }
    }

    IEnumerator Regen()
    {
        yield return new WaitForSeconds(regenDelay);

        while (currentHealth < maxHealth && !isDead && !isDead)
        {
            currentHealth += regenAmount;
            currentHealth = Mathf.Min(currentHealth, maxHealth); // no cheeky under or overflows here
            UpdateVignetteColor();
            yield return new WaitForSeconds(regenInterval);
        }

        if (currentHealth == maxHealth)
        {
            lowPassfitlerAudio.enabled = false;
        }
    }

    void UpdateVignetteColor()
    {
        float healthColor = (float)currentHealth / maxHealth;
        Color newColor = Color.Lerp(Color.red, gameManager.initialVignetteColor, healthColor);
        gameManager.SetVignetteColor(newColor);

        lowPassfitlerAudio.enabled = true;
        float cutoffFrequency = Mathf.Lerp(minLowPassFilterCutoff, maxLowPassFilterCutoff, healthColor);
        lowPassfitlerAudio.cutoffFrequency = cutoffFrequency;
    }

    IEnumerator ScreenShake()
    {
        Vector3 originalPosition = mainCamera.transform.localPosition;

        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            mainCamera.transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        mainCamera.transform.localPosition = originalPosition;
    }
}
