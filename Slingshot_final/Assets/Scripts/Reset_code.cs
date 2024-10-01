using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reset_code : MonoBehaviour
{
    public void Reset_()
    {
        SceneManager.LoadSceneAsync(
                SceneManager.GetActiveScene().buildIndex);
    }
}
