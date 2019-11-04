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

    //stores current weapon state
    protected struct State
    {
        //

        public int currentCharge;
    }

    [SerializeField]
    Stats stats;
    State state;

    [SerializeField]
    protected Projectile projectileDefault = null;
    [SerializeField]
    protected Projectile projectileMedium = null;
    [SerializeField]
    protected Projectile projectileMax = null;
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

        SetCharge(0);
        this.timeUntilNextShot = 0;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (timeUntilNextShot > 0)
            timeUntilNextShot--;
    }

    private void SetCharge(int charge)
    {

        float percent;
        if (charge < stats.chargeTier)
        {
            percent = (float)charge / (float)stats.chargeTier;
            UIManager.INSTANCE.UpdateChargeL(percent);
            state.currentCharge = charge;

        }
        else
        {
            percent = (float)(charge - stats.chargeTier) /
                (float)(WeaponManager.MAX_CHARGE - stats.chargeTier);
            UIManager.INSTANCE.UpdateChargeR(percent);

            if (charge >= WeaponManager.MAX_CHARGE)
                state.currentCharge = WeaponManager.MAX_CHARGE;
            else
                state.currentCharge = charge;
        }
    }

    public virtual void OnShootButtonDown()
    {

    }

    public virtual void OnShootButtonHold()
    {
        SetCharge(state.currentCharge + stats.chargeRate);
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
                ShootMax(manager.GetPlayerDirection(), manager.GetLaunchPosition());
            }
            else if (state.currentCharge >= stats.chargeTier)
            {
                ShootMedium(manager.GetPlayerDirection(), manager.GetLaunchPosition());
            }
            else
            {
                ShootDefault(manager.GetPlayerDirection(), manager.GetLaunchPosition());
            }
            //reset charge
            SetCharge(0);
        }
    }
    //
    public int GetCurrentCharge()
    {
        return state.currentCharge;
    }

    protected virtual void ShootDefault(Vector3 direction, Vector3 pos)
    {
        CreateProjectile(projectileDefault, direction, pos);
    }

    protected virtual void ShootMedium(Vector3 direction, Vector3 pos)
    {
        CreateProjectile(projectileMedium, direction, pos);
    }

    protected virtual void ShootMax(Vector3 direction, Vector3 pos)
    {
        CreateProjectile(projectileMax, direction, pos);
    }

    protected Projectile CreateProjectile(Projectile projPrefab, Vector3 direction, Vector3 pos)
    {
        Projectile proj = Instantiate(projPrefab);
        proj.Activate(pos, direction);
        proj.Initialise(0, this);

        return proj;
    }

    public void DisableProjectile(Projectile proj)
    {
        Destroy(proj.gameObject);
    }
}