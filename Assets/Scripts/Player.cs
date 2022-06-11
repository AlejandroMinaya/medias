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
[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    private const float MOVEMENT_THRESHOLD = 0.01f;
    private const int MAX_HP = 3;
    public float speed = 10.0f;
    public float jumpForce = 1.0f;

    private Rigidbody rb;
    private Animator anim;
    private Vector3 camRelativePos;

    private float outOfScreenTime = 0.0f;
    private float outOfScreenTolerance = 3.0f;
    private bool isGrounded = false;
    private bool isLookingForward = true;

    // Movement controles
    private float xForce = 0.0f;
    private float yForce = 0.0f;
    private float upForce = 0.0f;

    // Health
    public int hp {private set; get; }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        hp = MAX_HP;
    }

    // Update is called once per frame
    void Update()
    {
        AvoidGettingAhead();
        CheckOutOfScreen();
        UpdateAnimParameters();
        GetInput();
        SoundFX();
    }

    void FixedUpdate()
    {
        Move();
    }

    void SoundFX()
    {
        if (xForce != 0) {
            FindObjectOfType<AudioManager>().Play("Ped Walking");
        }
    }

    void UpdateAnimParameters()
    {
        anim.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        if (rb.velocity.x >= MOVEMENT_THRESHOLD && !isLookingForward)
        {
            isLookingForward = true;
        }

        if (rb.velocity.x < -1 * MOVEMENT_THRESHOLD && isLookingForward)
        {
            isLookingForward = false;
        }
        anim.SetBool("Backwards", !isLookingForward);
        anim.SetInteger("Health", hp);
    }

    void GetInput()
    {
        upForce = 0;
        xForce = Input.GetAxis("Horizontal");
        yForce = Input.GetAxis("Vertical");


        // GUARDS
        if (!isGrounded) return;
        if (rb.velocity.y > 0) return;
        upForce = Input.GetAxis("Jump");
    }

    void Move()
    {
        rb.AddForce(xForce*Vector3.right*speed);
        rb.MovePosition(new Vector3(
            transform.position.x,
            transform.position.y,
            yForce
        ));
        rb.AddForce(upForce * Vector3.up * jumpForce, ForceMode.Impulse);
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Vacuum")
        {
            if (hp >= MAX_HP)
                anim.SetTrigger("Lose First Eye");
            if (hp >= MAX_HP/2)
                anim.SetTrigger("Lose Second Eye");
            if (hp <= 0)
                anim.SetTrigger("Die");
            hp--;
        }
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
