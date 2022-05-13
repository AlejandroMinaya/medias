using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Vacuum : MonoBehaviour
{
    private const float speedScale = 50f;
    public float speed = 1f;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        rb.AddForce(-1 * transform.right * speed * speedScale);
    }
}
