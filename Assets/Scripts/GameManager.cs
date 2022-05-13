using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public const float DECIMAL_PLACES = 1e2f;
    public static GameManager Instance { get; private set; }

    /* UI Components */
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;

    private GameObject player;
    private float score = 0.0f;
    private string highScore;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0).ToString();
        highScoreText.SetText($"High score: {highScore}m");
    }

    void Update()
    {
        if (player != null)
        {
            score = TruncateFloat(player.transform.position.x);
            scoreText.SetText($"Score: {score}m");

            if (score > PlayerPrefs.GetFloat("HighScore", 0))
            {
                PlayerPrefs.SetFloat("HighScore", score);
                highScore = score.ToString();
                highScoreText.SetText($"High score: {highScore}m");
            }
        }
    }


    float TruncateFloat(float f)
    {
        return Mathf.Round(f * DECIMAL_PLACES)/DECIMAL_PLACES;
    }
}
