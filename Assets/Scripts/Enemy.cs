using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    GameObject player;



    // Start is called before the first frame update
    protected override void Start()
    {
        player = GameObject.FindWithTag("Player");
        Debug.Assert(player, "Игрок найден");

        base.Start();
    }

    // Update is called once per frame
    protected override void Control()
    {
        var distance = player.transform.position.x - transform.position.x;

        if (Mathf.Abs(distance) > swordLength)
        {
            var direction = distance < 0 ? -1 : 1;
            Move(direction);
        }
        else
        {
            AttemptAttack();
        }
    }
}
