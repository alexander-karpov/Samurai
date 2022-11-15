using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Character chr;
    Vector2 input;

    void Start()
    {
        chr = GetComponent<Character>();
    }

    void FixedUpdate()
    {
        chr.Move(input);
    }

    public void Sync(Vector2 position, Vector2 input)
    {
        this.input = input;

        chr.TeleportTo(position);
    }
}
