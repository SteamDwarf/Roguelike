using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack
{
    public string name;
    public Vector2 position;
    public float range;
    public int damage;

    public Attack(string name, Vector2 position, float range, int damage)
    {
        this.name = name;
        this.position = position;
        this.range = range;
        this.damage = damage;
    }
}
