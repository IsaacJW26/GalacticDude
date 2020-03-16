using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boomerang : Weapon
{
    BoomerangProjectile projectile;

    public override void OnShootButtonDown()
    {
        projectile = ShootBoomerang(manager.GetPlayerDirection(), manager.GetLaunchPosition());
    }

    public override void OnShootButtonHold()
    {

    }

    public override void OnShootButtonRelease()
    {
        projectile.Return();
    }

    public BoomerangProjectile ShootBoomerang(Vector3 dir, Vector3 pos)
    {
        return (ShootDefault(dir, pos) as BoomerangProjectile);
    }
}
