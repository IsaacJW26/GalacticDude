using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{

    int timeUntilNextShot;

    [SerializeField]
    Projectile projectile;

    public void Awake()
    {
        this.timeUntilNextShot = 0;
        Debug.Log("call me uwu");
    }

    void Awake()
    {
        objectPool = new Projectile[MAX_PROJECTILES];
        activeElements = new bool[MAX_PROJECTILES];

        //intialise object pool
        for (int ii = 0; ii < MAX_PROJECTILES; ii++)
        {
            objectPool[ii] = Instantiate(projectile);
            objectPool[ii].Initialise(projectileDamage, ii, this);
            DisableProjectile(ii);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeUntilNextShot > 0)
            timeUntilNextShot--;
    }

    public void TryShoot(Vector3 direction)
    {
        //successfully shoot
        if (timeUntilNextShot <= 0)
        {
            timeUntilNextShot = shootInterval;
            Shoot(direction.normalized * projectileSpeed);
        }
    }

    private void Shoot(Vector3 velocity)
    {
        Instantiate(projectile)
    }

    public void DisableProjectile(int index)
    {
        activeElements[index] = false;
        objectPool[index].gameObject.SetActive(false);
    }
}
