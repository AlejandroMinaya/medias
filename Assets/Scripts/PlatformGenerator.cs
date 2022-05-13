using UnityEngine;
using System.Collections.Generic;

public class PlatformGenerator : MonoBehaviour {

    public const int platformInterval = 2;
    public const int maxAdjacentPlatforms = 5;
    public const float maxPlatformHeight = 2f;
    public const float noiseScale = 20f;
    public const float spawnProbability = 0.67f;

    private Transform player;
    private float seedX;
    private int playerX;

    private Dictionary<int, Platform> generatedPlatforms;

    void Start()
    {
        generatedPlatforms = new Dictionary<int, Platform>();
        player = GameObject.FindWithTag("Player").transform;
        seedX = Random.Range(0f, 1f);
    }

    void Update ()
    {
        if (player != null)
        {
            playerX = Mathf.RoundToInt(player.position.x);
            if (playerX % platformInterval > 0) return;
            GeneratePlatform();
            DestroyPlatform();
        }
    }

    void GeneratePlatform()
    {
        if (playerX < 0) return;
        if (generatedPlatforms.ContainsKey(playerX)) return;
        float coord = seedX + playerX * noiseScale;
        float height = Mathf.PerlinNoise(coord, coord);
        if (height > spawnProbability) return;

        generatedPlatforms.Add(
            playerX, new Platform(playerX, height)
        );
    }

    void DestroyPlatform()
    {
        int prevPlatform = playerX - maxAdjacentPlatforms * platformInterval;
        int nextPlatform = playerX + maxAdjacentPlatforms * platformInterval;
        if (generatedPlatforms.ContainsKey(prevPlatform))
        {
            generatedPlatforms[prevPlatform].Destroy();
            generatedPlatforms.Remove(prevPlatform);
        }
        if (generatedPlatforms.ContainsKey(nextPlatform))
        {
            generatedPlatforms[nextPlatform].Destroy();
            generatedPlatforms.Remove(nextPlatform);
        }
    }
}

public class Platform
{
    private GameObject platform;
    private float coord;
    private float height;

    public Platform(float x, float height)
    {
        platform = GameObject.CreatePrimitive(PrimitiveType.Cube);
        platform.transform.position = new Vector3(
            x + PlatformGenerator.platformInterval,
            height * PlatformGenerator.maxPlatformHeight, 0
        );
        platform.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }

    public void Destroy()
    {
        GameObject.Destroy(platform);
    }
}
