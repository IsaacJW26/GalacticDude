using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScatterShotWeapon : Weapon
{
    protected override void ShootDefault(Vector3 direction, Vector3 pos)
    {
        //base.ShootDefault(direction, pos);
        Vector3 directionL, directionR;
        directionL = (Vector3.up * 2f + Vector3.left).normalized;
        directionR = (Vector3.up * 2f + Vector3.right).normalized;

        CreateProjectile(projectileDefault, directionL, pos);
        CreateProjectile(projectileDefault, directionR, pos);
    }

    protected override void ShootMedium(Vector3 direction, Vector3 pos)
    {
        base.ShootMedium(direction, pos);
        ShootDefault(direction, pos);
    }


    protected override void ShootMax(Vector3 direction, Vector3 pos)
    {
        Vector3 directionL, directionR;
        directionL = (Vector3.up * 2f + Vector3.left).normalized;
        directionR = (Vector3.up * 2f + Vector3.right).normalized;

        CreateProjectile(projectileMedium, directionL, pos);
        CreateProjectile(projectileMedium, direction, pos);
        CreateProjectile(projectileMedium, directionR, pos);

        //base.ShootMax(direction, pos);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

}
