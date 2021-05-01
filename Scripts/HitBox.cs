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
            hitTarget.GetComponent<Player>().GetDamage(damage);
        else if (hitTarget.tag == "Enemy")
            hitTarget.GetComponent<Enemy>().GetDamage(damage);

        Debug.Log("Враг атакует" + collision.gameObject.name);
    }
}
