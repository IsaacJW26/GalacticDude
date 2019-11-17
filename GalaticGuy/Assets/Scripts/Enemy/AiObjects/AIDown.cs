using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDown : EnemyAI
{
    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        Move(Vector3.down);
    }
}
