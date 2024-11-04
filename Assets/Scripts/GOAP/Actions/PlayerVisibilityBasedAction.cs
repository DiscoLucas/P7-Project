using CrashKonijn.Goap.Interfaces;
using System.Net;
using UnityEngine;

public class PlayerVisibilityBasedAction : WanderActionM
{
    public override float GetCost(IMonoAgent agent, IComponentReference references)
    {
        float playerAwareness = references.GetCachedComponentInChildren<PlayerAwarenessSensor>().playerAwarenessLevel;
        float midpoint = (config.stalkMinPlayerAwareness + config.stalkMaxPlayerAwareness) / 2f;


        return calculateBase((int)playerAwareness,midpoint);

    }

    public virtual int calculateBase(int playerAwareness, float midpoint) {
        return (int)Mathf.Lerp(config.hideCostRange.x, config.hideCostRange.y, (playerAwareness - midpoint) / (config.stalkMaxPlayerAwareness - midpoint));
    }
}
