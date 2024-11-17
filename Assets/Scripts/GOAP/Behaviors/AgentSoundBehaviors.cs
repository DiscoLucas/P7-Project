using UnityEngine;
using JSAM;

public class AgentSoundBehaviors : MonoBehaviour
{
    float timeWhenScream = 0;
    public MonsterConfig config;
    public SoundFileObject scream;
    public SoundFileObject chaseTheme;
    public SoundFileObject Crageling;
    public SoundFileObject hitsound;
    bool screamActiveAction = false;
    public float getLastScreamTimeCost() {
        float cost = 0;
        float current = Time.timeSinceLevelLoad - timeWhenScream;
        cost = 1 - Mathf.Clamp01(current / config.screamInterval);
        return cost;
    }

    public void startScreamAction() {
        screamActiveAction = true;
    }

    /// <summary>
    /// Screaming sound starts playing
    /// </summary>
    public void playerScream() {
        
        timeWhenScream = Time.timeSinceLevelLoad;
        AudioManager.PlaySound(scream, transform.position);
    }

    /// <summary>
    /// Hit sound playing
    /// </summary>
    /// <param name="hitsound"></param>
    public void playHitsound(Transform hitsound) {
        AudioManager.PlaySound(this.hitsound, hitsound.position);

    }

    public bool checkIfScreamStopped() {
        bool screamDone = (screamActiveAction && AudioManager.IsSoundPlaying(scream));
        if (screamDone) {
            screamActiveAction = false ;
            return true ;
        }
        return false;
    }

    /// <summary>
    /// Play the chase theme
    /// </summary>
    public void playChaseTheme() { 
        AudioManager.PlaySound(chaseTheme, transform.position);
    }

    public void stopChaseTheme()
    {
        AudioManager.StopSound(chaseTheme);
    }

    public void playCrageling()
    {
        //Play crageling sound at a random time on the agent
        AudioManager.PlaySound(Crageling, transform.position);
    }
}
