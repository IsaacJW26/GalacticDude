﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : Projectile
{
    //
    Collider2D col;
    [SerializeField]
    int damageFrameGap;
    int timeLeft;
    int loopCount;

    public void OnEnable()
    {
        col = GetComponent<Collider2D>();

        EffectManager.INSTANCE?.ScreenShakeBig();
        EffectManager.INSTANCE?.SlowLong();

        timeLeft = damageFrameGap;
        loopCount = stats.maxLifeTime;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        //do Damage over time
        if (loopCount > 0)
        {
            //disable frame when each cycle ends
            if (timeLeft <= 0)
            {
                timeLeft = damageFrameGap;
                col.enabled = false;
            }
            else
            {
                timeLeft--;
                //enable when after disabled and cycle begins
                if (!col.enabled && timeLeft <= 0)
                {
                    col.enabled = true;
                }
            }
            loopCount--;
        }
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
