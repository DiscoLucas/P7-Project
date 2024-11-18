using UnityEngine;

public class VentManager : Singleton<VentManager>
{
    
    void Awake()
    {
        VentFeedback[] vents = FindObjectsOfType<VentFeedback>();

        foreach (VentFeedback vent in vents)
        {
            foreach (VentFeedback potentialLinkedVent in vents)
            {
                if (vent != potentialLinkedVent && Vector3.Distance(vent.linkTarget.position, potentialLinkedVent.linkTarget.position) < 1)
                {
                    vent.linkedVent = potentialLinkedVent;
                    break;
                }
            }
        }
    }

}
