using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Control()
    {
        var direction = Input.GetAxisRaw("Horizontal");
        var attack = Input.GetAxisRaw("Jump");

        if (direction != 0)
        {
            Move(direction);
        }

        if (attack != 0)
        {
            AttemptAttack();
        }
    }
}
