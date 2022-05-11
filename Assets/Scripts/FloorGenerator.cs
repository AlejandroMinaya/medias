using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGenerator : MonoBehaviour
{
    public const float floorLength = 1.0f;
    public const int maxAdjacentFloors = 3;

    public Transform player;

    private int normPlayerPos;
    private Dictionary<int, Floor> generatedFloors;

    void Start()
    {
        generatedFloors = new Dictionary<int, Floor>();
        generatedFloors.Add(0, new Floor(0));
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
            generatedFloors.Add(nextFloor, new Floor(nextFloor));
        }
    }

    void DestroyFloors(int playerPos)
    {
        int prevFloor = playerPos - maxAdjacentFloors;
        int nextFloor = playerPos + maxAdjacentFloors + 1;
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

    public Floor(int _floorPos)
    {
        floor = GameObject.CreatePrimitive(PrimitiveType.Plane);
        floor.transform.position = new Vector3(
            _floorPos * FloorGenerator.floorLength, 0, 0
        );
        floor.transform.localScale = new Vector3(
            FloorGenerator.floorLength, 1, 0.25f
        );
    }

    public void Destroy()
    {
        GameObject.Destroy(floor);
    }
}
