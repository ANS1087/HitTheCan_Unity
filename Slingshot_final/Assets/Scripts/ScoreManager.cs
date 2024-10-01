using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public int score = 0;
    public TMP_Text scoreText; // Reference to the TextMeshPro text component to display the score
    public TMP_Text HighScore;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("More than one ScoreManager instance found!");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateScoreUI();
        //DisplayHighScores();
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
    }
    /*public void SaveHighScore()
    {
        List<int> highScores = new List<int>();

        // Retrieve existing high scores
        for (int i = 1; i <= 3; i++)
        {
            highScores.Add(PlayerPrefs.GetInt("HighScore" + i, 0));
        }

        // Add the new score and sort the list in descending order
        highScores.Add(score);
        highScores.Sort((a, b) => b.CompareTo(a));

        // Remove duplicates and ensure only the top 3 scores are kept
        highScores = highScores.Distinct().Take(3).ToList();

        // Save the new high scores
        for (int i = 1; i <= highScores.Count; i++)
        {
            PlayerPrefs.SetInt("HighScore" + i, highScores[i - 1]);
        }
    }
    public void DisplayHighScores()
    {
        HighScore.text = "High Scores:\n";
        for (int i = 1; i <= 3; i++)
        {
            int highScore = PlayerPrefs.GetInt("HighScore" + i, 0);
            HighScore.text += i + ". " + highScore + "\n";
        }
    }*/
}