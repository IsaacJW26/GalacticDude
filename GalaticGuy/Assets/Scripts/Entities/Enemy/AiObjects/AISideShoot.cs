using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISideShoot : AISideBasic
{
    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        base.UpdateFrame(currentPosition);

        Shoot(Vector3.down);
    }
}
