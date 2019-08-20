using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals(Labels.TAGS.PROJECTILE) ||
            collision.tag.Equals(Labels.TAGS.ENEMY))
        {
            ScreenShake.INSTANCE.SmallShake();
            parent.DisableProjectile(index);
        }
    }
}
