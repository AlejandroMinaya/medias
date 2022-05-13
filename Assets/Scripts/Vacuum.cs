using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vacuum : MonoBehaviour
{
    private const float speedScale = 50f;

    public float speed = 1f;

    private float outOfScreenTime = 0.0f;
    private float outOfScreenTolerance = 1.5f;
    private Vector3 camRelativePos;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        camRelativePos = Camera.main.WorldToViewportPoint(transform.position);
        CheckOutOfScreen();
    }

    void CheckOutOfScreen()
    {
        if (camRelativePos.x < 0)
            outOfScreenTime += Time.deltaTime;
        else
            outOfScreenTime = 0;

        if (outOfScreenTime >= outOfScreenTolerance)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rb.AddForce(-1 * transform.right * speed * speedScale);
    }
}
