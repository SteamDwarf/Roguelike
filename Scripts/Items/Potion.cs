using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum typeEnum
{
    health, strength, stamina, speed
}
public class Potion : MonoBehaviour
{
    public float increase;
    public typeEnum type;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();

            switch(type)
            {
                case typeEnum.health:
                    player.UpdateHealth(increase);
                    break;
                case typeEnum.speed:
                    player.ChangeSpeed(increase);
                    break;
            }

            Destroy(this.gameObject);
            
        }
    }
}    
