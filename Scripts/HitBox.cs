using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage;
    public float thrust;
    public string owner;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hitTarget = collision.gameObject;

        if (hitTarget.CompareTag("Player") && owner != "Player")
        {
            Player playerScript = hitTarget.GetComponent<Player>();

            if (playerScript.isDied)
                return;

            playerScript.GetDamage(damage);
        }     
        else if (hitTarget.CompareTag("Enemy") && owner != "Enemy")
        {
            Debug.Log(hitTarget.tag);
            Debug.Log(owner);
            Enemy enemyScript = hitTarget.GetComponent<Enemy>();

            if (enemyScript.isDied)
                return;

            enemyScript.GetDamage(damage);
            /*Rigidbody2D rbEnemy = hitTarget.GetComponent<Rigidbody2D>();
            Vector2 difference = rbEnemy.transform.position - transform.position;
            difference = difference.normalized * thrust;
            rbEnemy.AddForce(difference, ForceMode2D.Impulse);
            Debug.Log("Get Impulse!!!!");*/
        }
            

        //Debug.Log("¿“¿ ”≈“" + collision.gameObject.name);
        //Debug.Log(damage);
    }
}
