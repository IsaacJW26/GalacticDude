using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShotWeapon : Weapon
{
    protected override Projectile ShootDefault(Vector3 direction, Vector3 pos)
    {
        //base.ShootDefault(direction, pos);
        Vector3 directionL, directionR;
        directionL = (Vector3.up * 2f + Vector3.left).normalized;
        directionR = (Vector3.up * 2f + Vector3.right).normalized;

        CreateProjectile(projectileDefault, directionL, pos);
        CreateProjectile(projectileDefault, directionR, pos);

        return null;
    }

    protected override Projectile ShootMedium(Vector3 direction, Vector3 pos)
    {
        base.ShootMedium(direction, pos);
        ShootDefault(direction, pos);

        return null;
    }


    protected override Projectile ShootMax(Vector3 direction, Vector3 pos)
    {
        Vector3 directionL, directionR;
        directionL = (Vector3.up * 2f + Vector3.left).normalized;
        directionR = (Vector3.up * 2f + Vector3.right).normalized;

        CreateProjectile(projectileMedium, directionL, pos);
        CreateProjectile(projectileMedium, direction, pos);
        CreateProjectile(projectileMedium, directionR, pos);

        return null;
    }
}
