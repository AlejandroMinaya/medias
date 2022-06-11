using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VacuumGenerator : MonoBehaviour
{
    private const int minSpawnDist = 2;
    private const float initialDistToPlayer = 5.0f;
    private const int maxSpawnDist = 5;
    private const float minHeight = 0.15f;
    private int[] zValues = {-1, 0, 1};

    public GameObject vacuum;

    private Transform player;
    private int interval;
    private GameObject lastVacuum;
    private int randIdx = 0;

    void Start()
    {
        GameObject _player = GameObject.FindWithTag("Player");
        if (_player == null) return;
        player = _player.transform;
        interval = Random.Range(minSpawnDist, maxSpawnDist);
    }

    void Update()
    {
        if (vacuum == null) return;
        if (player == null) return;
        int playerPos = Mathf.RoundToInt(player.position.x);

        if (playerPos <= 0) return;
        if (playerPos % interval > 0) return;
        if (lastVacuum != null) return;
        GenerateVacuum();
    }

    void GenerateVacuum()
    {
        randIdx = Random.Range(0,3);
        lastVacuum = Instantiate(
            vacuum, new Vector3(
                player.position.x + initialDistToPlayer,
                minHeight, zValues[randIdx]
            ), Quaternion.identity
        );
    }
}
