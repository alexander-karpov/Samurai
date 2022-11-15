using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Character chr;

    void Start()
    {
        chr = GetComponent<Character>();
    }

    void FixedUpdate()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var input = new Vector2(horizontal, vertical);

        chr.Move(input);
    }
}
