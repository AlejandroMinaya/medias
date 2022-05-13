using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public const float floorLength = 10.0f;
    public const float floorScale = 10.0f;
    public const int maxAdjacentFloors = 1;

    public Material material;

    private Transform player;
    private int normPlayerPos;
    private Dictionary<int, Floor> generatedFloors;

    void Start()
    {
        GameObject _player = GameObject.FindWithTag("Player");
        if (_player == null) return;
        player = _player.transform;
        generatedFloors = new Dictionary<int, Floor>();
        generatedFloors.Add(0, new Floor(0, material));
        GenerateFloor(0);
    }

    void Update()
    {
        if (player != null) {
            normPlayerPos = Mathf.RoundToInt(player.position.x/floorLength);
            GenerateFloor(normPlayerPos);
            DestroyFloors(normPlayerPos);
        }
    }

    void GenerateFloor(int playerPos)
    {
        int nextFloor = playerPos + 1;
        if (!generatedFloors.ContainsKey(nextFloor))
        {
            generatedFloors.Add(nextFloor, new Floor(nextFloor, material));
        }
        int prevFloor = playerPos - 1;
        if (!generatedFloors.ContainsKey(prevFloor))
        {
            generatedFloors.Add(prevFloor, new Floor(prevFloor, material));
        }
    }

    void DestroyFloors(int playerPos)
    {
        int prevFloor = playerPos - (maxAdjacentFloors + 1);
        Debug.Log(prevFloor);
        int nextFloor = playerPos + (maxAdjacentFloors + 1);
        if (generatedFloors.ContainsKey(prevFloor))
        {
            generatedFloors[prevFloor].Destroy();
            generatedFloors.Remove(prevFloor);
        }
        if (generatedFloors.ContainsKey(nextFloor))
        {
            generatedFloors[nextFloor].Destroy();
            generatedFloors.Remove(nextFloor);
        }

    }
}

public class Floor
{
    private GameObject floor;

    public Floor(int _floorPos, Material material)
    {
        floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = new Vector3(
            _floorPos * FloorGenerator.floorLength,
            0, 0
        );
        floor.transform.localScale = new Vector3(
            FloorGenerator.floorLength / FloorGenerator.floorScale, 1, 0.25f
        );
        floor.GetComponent<Renderer>().material = material;
    }

    public void Destroy()
    {
        GameObject.Destroy(floor);
    }
}
