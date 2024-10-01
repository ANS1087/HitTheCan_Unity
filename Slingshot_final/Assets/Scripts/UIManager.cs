using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public ScoreManager scoreM;
    public SlingShot func;

    public TMP_Text Shots;
    public TMP_Text NoShots;
    public TMP_Text Vangle;
    public TMP_Text Hangle;
    public TMP_Text Distance;
    public TMP_Text FinalScore;
    public TMP_Text GameOver;

    public int shotsLeft;
    private float V_angle;
    private float H_angle;
    private float distance;

    private bool areAnglesAndDistanceVisible = false; 

    void Start()
    {
        V_angle = 0f;
        H_angle = 0f;
        distance = 0f;

        UpdateUI();
        NoShots.gameObject.SetActive(false); 
        FinalScore.gameObject.SetActive(false);
        GameOver.gameObject.SetActive(false);
    }

    void Update()
    {
        if (shotsLeft <= 0)
        {
            Shots.gameObject.SetActive(false);
            NoShots.gameObject.SetActive(true);
        }
    }
    
    void UpdateUI()
    {
        Shots.text = "Shots Left: " + shotsLeft.ToString();
        Vangle.text = "Vertical angle: " + V_angle.ToString("F2") + "°";
        Hangle.text = "Horizontal Angle: " + H_angle.ToString("F2") + "°";
        Distance.text = "Distance: " + distance.ToString("F2") + "m";
        FinalScore.text = "your Final score is:" + scoreM.score.ToString()+"/150";
        scoreM.UpdateScoreUI();
        //scoreM.SaveHighScore();

        Vangle.gameObject.SetActive(areAnglesAndDistanceVisible);
        Hangle.gameObject.SetActive(areAnglesAndDistanceVisible);
        Distance.gameObject.SetActive(areAnglesAndDistanceVisible);

        if (shotsLeft > 0)
        {
            Shots.gameObject.SetActive(true);
            NoShots.gameObject.SetActive(false);
        }
    }

    public void SetShotsLeft()
    {
        UpdateUI();
    }

    public void SetVAngle(float newVangle)
    {
        V_angle = newVangle;
        UpdateUI();
    }

    public void SetHAngle(float newHangle)
    {
        H_angle = newHangle;
        UpdateUI();
    }

    public void SetDistance(float newDistance)
    {
        distance = newDistance;
        UpdateUI();
    }
    public void ToggleAnglesAndDistanceVisibility()
    {
        areAnglesAndDistanceVisible = !areAnglesAndDistanceVisible;
        UpdateUI();
    }
}
