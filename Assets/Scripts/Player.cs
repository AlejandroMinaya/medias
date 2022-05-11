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
using UnityEditor.Animations;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const float MOVEMENT_THRESHOLD = 0.01f;
    public float speed = 1.0f;
    public float jumpForce = 1.0f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 camRelativePos;

    private float outOfScreenTime = 0.0f;
    private float outOfScreenTolerance = 3.0f;
    private bool isGrounded = false;
    private bool isLookingForward = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        AvoidGettingAhead();
        CheckOutOfScreen();
        UpdateAnimParameters();
    }

    void UpdateAnimParameters()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if (rb.velocity.x >= MOVEMENT_THRESHOLD && !isLookingForward)
        {
            isLookingForward = true;
            FlipHorizontally();
        }

        if (rb.velocity.x < -1 * MOVEMENT_THRESHOLD && isLookingForward)
        {
            isLookingForward = false;
            FlipHorizontally();
        }
    }

    void FlipHorizontally()
    {
        transform.Rotate(0.0f, 0.0f, 180.0f, Space.Self);
    }

    void Move()
    {
        rb.AddForce(Input.GetAxis("Horizontal")*Vector3.right*(1+speed/10));
        rb.MovePosition(new Vector3(
            transform.position.x,
            transform.position.y,
            Input.GetAxis("Vertical")
        ));
        if (isGrounded)
            rb.AddForce(
                Input.GetAxis("Jump") * Vector3.up * jumpForce/10,
                ForceMode.Impulse
            );
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
