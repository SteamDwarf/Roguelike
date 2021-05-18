using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        /*if (sawPlayer)
        {
            if (stamina >= 30f && !anim.isAttacking && Vector2.Distance(transform.position, target) <= attackRadius)
            {
                currentAgroTime = startAgroTime;
                StartCoroutine(Attacking());
                MakeAttack();
            }
        }*/
    }


    protected override void MakeAttack()
    {
        int attackInd = Random.Range(0, enemyAttacks.Count);
        Attack attack = enemyAttacks[attackInd];
        attackTime = 1f;
        stamina -= 30;
        curAttack = attack.name;
        currentAgroTime = startAgroTime;
        //attackPoses[attackInd].GetComponent<HitBox>().damage = attack.damage;
    }


    protected override void DefaultBehavior()
    {
        if (sawPlayer)
        {
            target = currentPlayerPosition;
            //currentState = EnemyState.run;
            anim.curState = "Run";
            speed = defaultSpeed * 2;
            currentAgroTime -= Time.deltaTime;

            if (stamina >= 30f && !anim.isAttacking && Vector2.Distance(transform.position, target) <= attackRadius)
            {
                StartCoroutine(Attacking());
                MakeAttack();
            }
        }
        else
        {
            target = startPosition;
            //currentState = EnemyState.walk;
            anim.curState = "Walk";
            speed = defaultSpeed;

            if (Vector2.Distance(transform.position, target) < 1)
                anim.curState = "Idle";
            //currentState = EnemyState.idle;
        }

        //anim.Play(currentAnimation + enemyName);

        if (currentAgroTime <= 0)
        {
            sawPlayer = false;
        }
    }
}
