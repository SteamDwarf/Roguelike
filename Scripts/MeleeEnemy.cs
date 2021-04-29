using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAttack = true;
            MakeAttack();
            isAttack = false;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isAttack = true;
            MakeAttack();
            isAttack = false;

        }
    }

    protected override void MakeAttack()
    {
        attackTime = 1f;
        int attackInd = Random.Range(0, enemyAttacks.Count);
        Attack attack = enemyAttacks[attackInd];
        currentAnimation = attack.name;
        anim.Play(currentAnimation + enemyName);
        Debug.Log(isAttack);
        Debug.Log(currentAnimation );
        Debug.Log("Где анимация!!!!");
    }


    /* protected override void DefaultBehavior()
     {
         if (sawPlayer)
             target = currentPlayerPosition;
     }*/
}
