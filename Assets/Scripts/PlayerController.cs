using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MoveController
{
    Vector2 lastPlayerInput;

    [SerializeField]
    Textroom textroom;

    void FixedUpdate()
    {
        var h = UnityEngine.Input.GetAxisRaw("Horizontal");
        var v = UnityEngine.Input.GetAxisRaw("Jump");
        var input = new Vector2(h, v);

        if (input != lastPlayerInput)
        {
            lastPlayerInput = input;

            textroom.SendInput(
                Time.frameCount,
                input,
                transform.position
            );
        }
    }
}
