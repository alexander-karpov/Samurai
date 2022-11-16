using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float positionErrorTolerance = .2f;

    Character chr;
    Rigidbody2D rb;
    Vector2 input;
    Vector2 predictedPosition;

    void Start()
    {
        chr = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();

        predictedPosition = rb.position;
    }

    void FixedUpdate()
    {
        if (input != Vector2.zero)
        {
            predictedPosition += input.normalized * (chr.speed * Time.fixedDeltaTime);

            rb.MovePosition(predictedPosition);
        }
    }

    public void Sync(Vector2 position, Vector2 input)
    {
        this.input = input;
        predictedPosition = position;
    }
}
