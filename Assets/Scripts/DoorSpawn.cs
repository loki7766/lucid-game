using UnityEngine;
using System.Collections;

public class DoorSpawner : MonoBehaviour
{
    public DelayedActivation delayedActivation;

    public void StartSpawningDoor()
    {
        if (delayedActivation != null)
        {
            delayedActivation.ActivateDoorWithDelay(2f);
          
        }
        else
        {
            Debug.LogWarning("DelayedActivation script is not assigned.");
        }
    }

   

}