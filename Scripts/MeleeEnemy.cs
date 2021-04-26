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

   /* protected override void DefaultBehavior()
    {
        if (sawPlayer)
            target = currentPlayerPosition;
    }*/
}
