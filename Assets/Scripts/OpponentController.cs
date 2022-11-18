using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MoveController
{
    Vector2 opponentInput;
    int lastReceivedInputVersion = 0;

    [SerializeField]
    Textroom textroom;

    void Start()
    {
        textroom.OnInput += HandleInput;
    }

    void HandleInput(object sender, (int version, Vector2 input, Vector2 position) e)
    {
        if (e.version > lastReceivedInputVersion)
        {
            lastReceivedInputVersion = e.version;
            opponentInput = e.input;
            transform.position = e.position;
        }
    }

    float lastY = 0;

    protected override void GatherInput()
    {
        var JumpDown = false;
        var JumpUp = false;

        if (opponentInput.y != lastY)
        {
            JumpDown = opponentInput.y > 0;
            JumpUp = opponentInput.y == 0;

            lastY = opponentInput.y;
        }

        Input = new FrameInput
        {
            JumpDown = JumpDown,
            JumpUp = JumpUp,
            X = opponentInput.x
        };

        if (Input.JumpDown)
        {
            _lastJumpPressed = Time.time;
        }
    }
}
