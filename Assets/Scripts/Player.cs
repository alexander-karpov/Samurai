using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Character chr;
    Rigidbody2D rb;

    void Start()
    {
        chr = GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        var h = Input.GetAxisRaw("Horizontal");
        var v = Input.GetAxisRaw("Vertical");
        var input = new Vector2(h, v);

        if (input != Vector2.zero)
        {
            rb.MovePosition(rb.position + input.normalized * (chr.speed * Time.fixedDeltaTime));
        }
    }
}
