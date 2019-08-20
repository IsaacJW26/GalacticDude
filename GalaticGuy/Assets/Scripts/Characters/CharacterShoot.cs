﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour
{
    const int MAX_PROJECTILES = 20;

    int timeUntilNextShot = 0;
    [SerializeField]
    int shootInterval = 35;
    [SerializeField]
    int projectileDamage = 1;
    [SerializeField]
    float projectileSpeed = 3f;

    [SerializeField]
    Projectile projectile = null;
    [SerializeField]
    Transform projectilePoint = null;

    Projectile[] objectPool;
    bool[] activeElements;

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
        if(timeUntilNextShot > 0)
            timeUntilNextShot--;
    }

    public void TryShoot(Vector3 direction)
    {
        //successfully shoot
        if(timeUntilNextShot <= 0)
        {
            timeUntilNextShot = shootInterval;
            Shoot(direction.normalized * projectileSpeed);
        }
    }

    private void Shoot(Vector3 velocity)
    {
        Projectile nextProj = EnableNextProjectile(velocity);
    }

    //overload of enable next projectile
    private Projectile EnableNextProjectile(Vector3 velocity)
    {
        for (int ii = 0; ii < MAX_PROJECTILES; ii++)
        {
            if (!activeElements[ii])
            {
                activeElements[ii] = true;
                objectPool[ii].Activate(projectilePoint.position, velocity);
                return objectPool[ii];
            }
        }

        return null;
    }

    public void DisableProjectile(int index)
    {
        activeElements[index] = false;
        objectPool[index].gameObject.SetActive(false);
    }
}
