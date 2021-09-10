using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Projectile
{
    
    [SerializeField]
    private Projectile explosion;

    public override void DisableObject()
    {
        Projectile projectile = Instantiate(explosion, transform.position, Quaternion.identity);
        projectile.Initialise();

        parent.DisableProjectile(this);
    }
}
