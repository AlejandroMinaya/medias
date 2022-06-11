using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flip : MonoBehaviour
{
    public float prob = 0.05f;

    private const int INTERVAL = 5;
    private const float DIFF_INCREASE = 1.15f;

    private float lastChangeDist = 0;
    private Animator anim;
    private float rand;

    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("Rotate", 0.0f, 1.5f);
        prob *= DIFF_INCREASE * (int)GameManager.score/INTERVAL;
        lastChangeDist = GameManager.score;
    }

    void Update() {
        if ((int)GameManager.score % INTERVAL == 0 && GameManager.score > lastChangeDist) {
            lastChangeDist = GameManager.score + INTERVAL - 0.1f;
            prob *= DIFF_INCREASE;
        }
    }

    // Update is called once per frame
    void Rotate()
    {
        float rand = Random.Range(0f, 1f);
        if (rand <= prob) {
            anim.SetTrigger("Flip");
        }
    }
}
