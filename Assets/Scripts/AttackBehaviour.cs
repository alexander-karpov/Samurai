using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehaviour : MonoBehaviour
{
    public float attackDelay = 0.5f;
    public float swordLength = 2f;
    public LayerMask attackable;
    public SpriteRenderer sword;

    float lastAttackTime;
    readonly RaycastHit2D[] attackHits = new RaycastHit2D[3];

    public bool AttemptAttack(Vector2 direction)
    {
        if (Time.realtimeSinceStartup < lastAttackTime + attackDelay)
        {
            return false;
        }

        StartCoroutine(ShowSword());

        lastAttackTime = Time.realtimeSinceStartup;

        var count = Physics2D.RaycastNonAlloc(
            transform.position,
            direction,
            attackHits,
            swordLength,
            attackable.value
        );

        if (count == 0)
        {
            return false;
        }

        for (int i = 0; i < count; i++)
        {
            var hitGameObject = attackHits[i].collider.gameObject;

            if (hitGameObject != null && hitGameObject != gameObject)
            {


                return true;
            }
        }

        return false;
    }

    IEnumerator ShowSword()
    {
        sword.enabled = true;
        yield return new WaitForSeconds(.2f);
        sword.enabled = false;
    }
}
