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

    private GameObject player;
    private float score = 0.0f;

    private void Awake()
    {
        Instance = this;
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        score = TruncateFloat(player.transform.position.x);
        scoreText.SetText($"{score}m");
    }


    float TruncateFloat(float f)
    {
        return Mathf.Round(f * DECIMAL_PLACES)/DECIMAL_PLACES;
    }
}
