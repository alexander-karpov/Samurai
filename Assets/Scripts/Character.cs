using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 5f;

    Rigidbody2D rb;

    Vector2? teleportPosition = null;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 input)
    {
        if (input == Vector2.zero && teleportPosition == null)
        {
            return;
        }

        var position = rb.position;

        if (teleportPosition.HasValue)
        {
            position = teleportPosition.Value;
            teleportPosition = null;
        }

        rb.MovePosition(position + input.normalized * (speed * Time.fixedDeltaTime));
    }

    public void TeleportTo(Vector2 position)
    {
        teleportPosition = position;
    }
}


