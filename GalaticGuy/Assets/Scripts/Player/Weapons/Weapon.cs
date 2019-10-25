using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, Shooter
{
    protected int timeUntilNextShot
    { get; private set; }

    [Serializable]
    protected class Stats
    {
        [SerializeField]
        [Range(1,150)]
        public int minShootInterval = 10;
        //[SerializeField]
        //public bool hasCharge;
        [SerializeField]
        [Tooltip("rate of 3 is 5 second charge time")]
        [Range(1, WeaponManager.MAX_CHARGE/9)]
        public int chargeRate = WeaponManager.MAX_CHARGE / 300;
        [SerializeField]
        [Range(1, WeaponManager.MAX_CHARGE)]
        public int chargeTier = WeaponManager.MAX_CHARGE / 2;
    }

    protected struct State
    {
        //
        public int currentCharge;
    }

    [SerializeField]
    Stats stats;
    State state;

    [SerializeField]
    Projectile projectileDefault = null;
    [SerializeField]
    Projectile projectileMedium = null;
    [SerializeField]
    Projectile projectileMax = null;
    [SerializeField]
    Vector2 relativeShoot = Vector2.up;

    public void Awake()
    {
        if (stats.minShootInterval <= 0)
            Debug.LogError("shoot interval is 0 or less");
        if(stats.chargeRate <= 0)
            Debug.LogError("charge rate is 0 or less");
        if(projectileDefault == null)
            Debug.LogError("No default projectile selected");

        state.currentCharge = 0;
        this.timeUntilNextShot = 0;
        Debug.Log("call me uwu");
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (timeUntilNextShot > 0)
            timeUntilNextShot--;
    }
    
    public virtual void OnShootButtonDown()
    {

    }

    public virtual void OnShootButtonHold()
    {
        if ((state.currentCharge + stats.chargeRate) >= WeaponManager.MAX_CHARGE)
            state.currentCharge = WeaponManager.MAX_CHARGE;
        else
            state.currentCharge += stats.chargeRate;
    }

    public virtual void OnShootButtonRelease(WeaponManager manager)
    {
        //successfully shoot
        //frame limit exists to stop spamming
        if (timeUntilNextShot <= 0)
        {
            timeUntilNextShot = stats.minShootInterval;

            if (state.currentCharge >= WeaponManager.MAX_CHARGE)
            {
                ShootMax(manager.GetPlayerDirection(), manager);

            }
            else if (state.currentCharge >= stats.chargeTier)
            {
                ShootMedium(manager.GetPlayerDirection(), manager);
            }
            else
            {
                ShootDefault(manager.GetPlayerDirection(), manager);
            }
            //reset charge
            state.currentCharge = 0;
        }
    }

    //
    public int GetCurrentCharge()
    {
        return state.currentCharge;
    }

    protected virtual void ShootDefault(Vector3 direction, WeaponManager manager)
    {
        Projectile proj = Instantiate(projectileDefault);
        proj.Activate(manager.GetLaunchPosition(), direction);
        proj.Initialise(0, this);
    }


    protected virtual void ShootMedium(Vector3 direction, WeaponManager manager)
    {
        Projectile proj = Instantiate(projectileMedium);
        proj.Activate(manager.GetLaunchPosition(), direction);
        proj.Initialise(0, this);
        Debug.Log("SHOOT MEDIUM");

    }

    protected virtual void ShootMax(Vector3 direction, WeaponManager manager)
    {
        Projectile proj = Instantiate(projectileMax);
        proj.Activate(manager.GetLaunchPosition(), direction);
        proj.Initialise(0, this);
        Debug.Log("SHOOT MAX");

    }

    public void DisableProjectile(Projectile proj)
    {
        Destroy(proj.gameObject);
    }
}