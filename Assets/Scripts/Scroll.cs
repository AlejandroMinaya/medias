using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll : MonoBehaviour
{
    public float scrollAcceleration = 1.0f;

    private const float ACCELERATION_SCALE = 1e4f;
    private float scrollSpeed = 0.3f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * scrollSpeed * Time.deltaTime);
        scrollSpeed *= (1 + scrollAcceleration/ACCELERATION_SCALE);
    }
}
