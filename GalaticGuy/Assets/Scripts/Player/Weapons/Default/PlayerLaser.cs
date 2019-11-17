using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : Projectile
{
    //
    Collider2D col;
    [SerializeField]
    int damageInterval;
    int timeLeft;
    int loopCount;

    public void OnEnable()
    {
        col = GetComponent<Collider2D>();

        EffectManager.INSTANCE?.ScreenShakeBig();
        EffectManager.INSTANCE?.SlowLong();

        timeLeft = damageInterval;
        loopCount = stats.maxLifeTime / damageInterval;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        //do Damage over time
        if (loopCount > 0)
        {
            if (timeLeft <= 0)
            {
                loopCount--;
                timeLeft = damageInterval;
                col.enabled = false;
                Debug.Log("enabled");

            }
            else
            {
                if (!col.enabled && timeLeft < damageInterval)
                {
                    Debug.Log("disabled");
                    col.enabled = true;
                }
                timeLeft--;
            }
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
