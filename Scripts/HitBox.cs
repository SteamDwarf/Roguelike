using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject hitTarget = collision.gameObject;

        if (hitTarget.tag == "Player")
        {
            Player playerScript = hitTarget.GetComponent<Player>();

            if (playerScript.isDied)
                return;

            playerScript.GetDamage(damage);
        }     
        else if (hitTarget.tag == "Enemy")
        {
            Enemy enemyScript = hitTarget.GetComponent<Enemy>();

            if (enemyScript.isDied)
                return;

            enemyScript.GetDamage(damage);
        }
            

        Debug.Log("¿“¿ ”≈“" + collision.gameObject.name);
        Debug.Log(damage);
    }
}
