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
    }

    /*private void OnCollisionEnter2D(Collision2D collision)
    {
        if(stamina < 15f)
        {
            Debug.Log("�� �� ������ ���������");
        }
        if (collision.gameObject.tag == "Player" && stamina >= 15f)
        {
            StartCoroutine(Attacking());
            MakeAttack();
            Debug.Log(stamina);
           
        }
    }*/

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (stamina < 30f)
        {
            //Debug.Log("�� �� ������ ���������");
            //Debug.Log(currentState);
        }
        if (collision.gameObject.tag == "Player" && stamina >= 30f)
        {
            StartCoroutine(Attacking());
            MakeAttack();
            //Debug.Log(stamina);
        }
    }

    protected override void MakeAttack()
    {
        int attackInd = Random.Range(0, enemyAttacks.Count);
        Attack attack = enemyAttacks[attackInd];
        attackTime = 1f;
        stamina -= 30;
        curAttack = attack.name;
        //anim.Play(currentAnimation + enemyName);
        //Debug.Log(isAttack);
        /*Debug.Log(curAttack);
        Debug.Log(enemyName);*/
        //Debug.Log("���� �������");
    }


    /* protected override void DefaultBehavior()
     {
         if (sawPlayer)
             target = currentPlayerPosition;
     }*/
}
