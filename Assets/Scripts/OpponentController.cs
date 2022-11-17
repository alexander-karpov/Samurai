using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentController : MoveController
{
    Vector2 opponentInput;

    public void Sync(Vector2 position, Vector2 input)
    {
        opponentInput = input;
        transform.position = position;
        // predictedPosition = position;
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
