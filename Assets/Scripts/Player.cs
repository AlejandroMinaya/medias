/**
 * Player.cs
 *
 * Contains all the basic player behavior.
 * The main focus of this script is to react
 * to the user input.
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float speed = 3.0f;

    private Rigidbody rb;
    private Vector3 camRelativePos;

    private float outOfScreenTime = 0.0f;
    private float outOfScreenTolerance = 3.0f;
    private bool isGrounded = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        AvoidGettingAhead();
        CheckOutOfScreen();
    }

    void Move()
    {
        rb.AddForce(Input.GetAxis("Horizontal") * Vector3.right * speed);
        rb.MovePosition(new Vector3(
            transform.position.x,
            transform.position.y,
            Input.GetAxis("Vertical")
        ));
        if (isGrounded)
            rb.AddForce(Input.GetAxis("Jump") * Vector3.up, ForceMode.Impulse);
    }

    void AvoidGettingAhead()
    {
        if (camRelativePos.x > 1)
            rb.Sleep();
        else
            rb.WakeUp();
    }

    void CheckOutOfScreen()
    {
        camRelativePos = Camera.main.WorldToViewportPoint(transform.position);
        if (camRelativePos.x < 0)
            outOfScreenTime += Time.deltaTime;
        else
            outOfScreenTime = 0;

        if (outOfScreenTime >= outOfScreenTolerance)
            Destroy(gameObject);
    }

    void OnCollisionStay()
    {
        // TODO: Specify that we are talking about collisions with the ground
        isGrounded = true;
    }

    void OnCollisionExit()
    {
        // TODO: Specify that we are talking about collisions with the ground
        isGrounded = false;

    }
}
