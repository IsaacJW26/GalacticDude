using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterShoot : MonoBehaviour
{
    const int MAX_PROJECTILES = 30;

    int timeUntilNextShot = 0;
    int shootInterval = 35;
    [SerializeField]
    Projectile projectile;

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
            objectPool[ii].Initialise(ii, Vector2.up);
            DisableProjectile(ii);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timeUntilNextShot > 0)
            timeUntilNextShot--;
    }

    public void TryShoot()
    {
        //successfully shoot
        if(timeUntilNextShot <= 0)
        {
            timeUntilNextShot = shootInterval;
        }
    }

    private void Shoot()
    {

    }

    private void EnableNextProjectile()
    {
        for(int ii = 0; ii < MAX_PROJECTILES; ii++)
        {
            if(!activeElements[ii])
            {
                objectPool[ii].Activate(transform.position);
            }
        }
    }

    public void DisableProjectile(int index)
    {
        activeElements[index] = false;
        objectPool[index].gameObject.SetActive(false);
    }
}
