using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos = 0.0f;
    private float length = 0.0f;
    public float parallaxEffect;

    void Start()
    {
        startPos = transform.position.x;
        length = GetComponent<MeshRenderer>().bounds.size.x;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float temp = Camera.main.transform.position.x * (1 - parallaxEffect);
        float dist = Camera.main.transform.position.x * parallaxEffect;

        transform.position = new Vector3(
            startPos + dist, transform.position.y, transform.position.z
        );

        if (temp > startPos + length/2)  startPos += length;
        else if (temp < startPos - length/2) startPos -= length;

    }
}
