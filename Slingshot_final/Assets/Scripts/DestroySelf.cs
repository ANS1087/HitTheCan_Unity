using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    public float destroyDelay = 5f; // Delay before destroying the game object

    public void TriggerDestroy()
    {
        Destroy(gameObject, destroyDelay);
    }
}
