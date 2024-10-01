using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CanScore : MonoBehaviour
{
    private bool isStanding = true; // Flag to track if the can is standing

    private void Start()
    {
        // Assuming the can starts in an upright position
        isStanding = true;
    }

    private void Update()
    {
        // Check if the can's state has changed
        bool previousState = isStanding;
        isStanding = IsStanding();

        if (previousState != isStanding)
        {
            // State has changed, update score
            if (isStanding)
            {
                // Can is now standing
                ScoreManager.Instance.AddScore(-10); // Subtract points
            }
            else
            {
                // Can has fallen down
                ScoreManager.Instance.AddScore(10); // Add points
            }
        }
    }

    // Check if the can is standing straight up
    public bool IsStanding()
    {
        // Calculate the rotation threshold based on your game's setup
        float uprightThreshold = 5f;

        // Check the rotation of the can around its up axis (Y axis)
        float angle = Vector3.Angle(transform.up, Vector3.up);

        // Return true if standing straight up, false otherwise
        return angle <= uprightThreshold;
    }
}
