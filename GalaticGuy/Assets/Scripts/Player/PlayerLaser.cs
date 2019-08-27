using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : Projectile
{
    //
    public void OnEnable()
    {
        EffectManager.INSTANCE?.ScreenShakeBig();
        EffectManager.INSTANCE?.SlowLong();
    }

    public override void OnDamage(int damage)
    {
        //do nothing
    }

    public override void DisableObject()
    {
        
        base.DisableObject();
    }
}
