using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float speed = 5f;
    public float attackDelay = 0.5f;
    public float swordLength = 1f;

    Animator animator;
    float lastAttackTime = 0;
    RaycastHit2D[] attackHits = new RaycastHit2D[3];
    int attackableMask = 0;
    bool alive = true;
    Vector2 spawnPosition;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        spawnPosition = transform.position;

        attackableMask = LayerMask.GetMask("Attackable");
        Debug.Assert(attackableMask != 0);
    }

    public void Control(float direction, float attack)
    {
        if (!alive)
        {
            return;
        }

        if (direction != 0)
        {
            Move(direction);
        }

        if (attack != 0)
        {
            AttemptAttack();
        }
    }

    public void FixPosition(float x, float y)
    {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(x, y, 0),
            5f * Time.deltaTime
        );

        transform.position = new Vector3(x, y, 0);
    }

    public void Hit()
    {
        if (!alive)
        {
            return;
        }

        animator.SetTrigger("hit");
        alive = false;
        StartCoroutine(Respawn());
    }

    void Move(float direction)
    {
        var p = transform.position;
        var movement = speed * direction * Time.deltaTime;

        transform.Translate(movement, 0, 0);
        transform.localScale = movement > 0 ? Vector3.one : new Vector3(-1, 1, 1);
    }

    void AttemptAttack()
    {
        if (Time.realtimeSinceStartup < lastAttackTime + attackDelay)
        {
            return;
        }

        animator.SetTrigger("attack");
        lastAttackTime = Time.realtimeSinceStartup;

        var count = Physics2D.RaycastNonAlloc(
            transform.position,
            new Vector2(transform.localScale.x, 0),
            attackHits,
            swordLength,
            attackableMask
        );

        if (count == 0)
        {
            return;
        }

        for (int i = 0; i < count; i++)
        {
            var chr = attackHits[i].collider.GetComponent<Character>();

            if (chr != null && chr != this)
            {
                chr.Hit();
            }
        }
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1);
        animator.SetTrigger("idle");
        alive = true;
        transform.position = spawnPosition;
    }
}


