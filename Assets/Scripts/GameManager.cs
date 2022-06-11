using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class GameManager : MonoBehaviour
{
    public const float DECIMAL_PLACES = 1e2f;
    public static GameManager Instance { get; private set; }
    public static float score { get; private set; }
    public GameObject gameOver;

    /* UI Components */
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI gameOverScoreText;
    public TMP_InputField highScoreInput;
    public Button highScoreSave;

    private GameObject player;
    private string highScore;

    private void Awake()
    {
        Instance = this;
        score = 0.0f;
        player = GameObject.FindWithTag("Player");
    }

    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScore", 0).ToString();
        highScoreText.SetText($"High score: {highScore}m");
    }

    void GameOver () {
        Time.timeScale = 0f;
        if (gameOver == null) return;
        gameOver.SetActive(true);
        FindObjectOfType<AudioManager>().StopAll();
    }

    void Update()
    {
        if (player == null) {
            GameOver();
            return;
        }
        if (player.GetComponent<Player>().hp <= 0) {
            GameOver();
            return;
        }
        score = TruncateFloat(player.transform.position.x);
        scoreText.SetText($"Score: {score}m");
        gameOverScoreText.SetText($"{score}m");

        if (score > PlayerPrefs.GetFloat("HighScore", 0))
        {
            PlayerPrefs.SetFloat("HighScore", score);
            highScore = score.ToString();
            highScoreText.SetText($"High score: {highScore}m");
        }
    }

    public static void GoToMainMenu ()
    {
        SceneManager.LoadScene(0);
    }

    public void SaveHighscore ()
    {
        Destroy(highScoreSave.gameObject);
        Destroy(highScoreInput.gameObject);
        // Create HighscoreEntry
        HighscoreEntry highscoreEntry = new HighscoreEntry {
            score = (int)score, name = highScoreInput.text
        };
 
        // Load saved Highscores
        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        if (highscores == null) {
            // There's no stored table, initialize
            highscores = new Highscores() {
                highscoreEntryList = new List<HighscoreEntry>()
            };
        }

        // Add new entry to Highscores
        highscores.highscoreEntryList.Add(highscoreEntry);

        // Save updated Highscores
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }


    float TruncateFloat(float f)
    {
        return Mathf.Round(f * DECIMAL_PLACES)/DECIMAL_PLACES;
    }

    private class Highscores {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable] 
    private class HighscoreEntry {
        public int score;
        public string name;
    }
}
