using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, Shooter, IWeapon
{
    protected int timeUntilNextShot
    { get; private set; }

    [Serializable]
    protected class Stats
    {
        [Header("Main")]
        [SerializeField]
        [Range(1,150)]
        public int minShootInterval = 10;

        [SerializeField]
        [Tooltip("rate of 3 is 10 second charge time")]
        [Range(1, WeaponManager.MAX_CHARGE/9)]
        public int chargeRate = WeaponManager.MAX_CHARGE / 300;

        [SerializeField]
        [Range(1, WeaponManager.MAX_CHARGE)]
        public int chargeTier = WeaponManager.MAX_CHARGE / 2;

        [Header("Speed up slow, down")]
        [SerializeField]
        [Tooltip("0 is 0% of original speed, 1 is 100% of original speed")]
        [Range(0f, 1f)]
        public float shootSlowMax = 1f;

        [SerializeField]
        public int slowDuration = 0;

        [SerializeField]
        public float chargeSlowDown = 1f;

        public Stats()
        {
            this.minShootInterval = 10;
            this.chargeRate = WeaponManager.MAX_CHARGE / 300;
            this.chargeTier = WeaponManager.MAX_CHARGE / 2;
            this.shootSlowMax = 1f;
            this.slowDuration = 0;
            this.chargeSlowDown = 1f;
        }

        public Stats(Stats stats)
        {
            this.minShootInterval = stats.minShootInterval;
            this.chargeRate = stats.chargeRate;
            this.chargeTier = stats.chargeTier;
            this.shootSlowMax = stats.shootSlowMax;
            this.slowDuration = stats.slowDuration;
            this.chargeSlowDown = stats.chargeSlowDown;
        }
    }

    //stores current weapon state
    protected struct State
    {
        //
        public int currentCharge;
    }

    [SerializeField]
    Stats baseStats = new Stats();
    Stats currentStats = null;
    State state;

    [SerializeField]
    Vector2 relativeShoot = Vector2.up;
    [Header("Projectiles")]
    [SerializeField] 
    protected Projectile projectileDefault = null;
    [SerializeField]
    protected Projectile projectileMedium = null;
    [Header("Max Projectile")]

    [SerializeField]
    protected Projectile projectileMax = null;

    protected ISpeed movement;
    protected IWeaponManager manager;
    protected int heldDuration;
    protected bool beingHeld;

    public void Awake()
    {
        if (baseStats.minShootInterval <= 0)
            Debug.LogError("shoot interval is 0 or less");
        if(baseStats.chargeRate <= 0)
            Debug.LogError("charge rate is 0 or less");
        if(projectileDefault == null)
            Debug.LogError("No default projectile selected");

        this.timeUntilNextShot = 0;
        ResetAllStats();
        SetCharge(0);
    }

    private void ResetAllStats()
    {
        currentStats = new Stats(baseStats);
    }

    public void Initialise(ISpeed movement, IWeaponManager manager)
    {
        this.movement = movement;
        this.manager = manager;
    } 

    // Update is called once per frame
    public void FixedUpdate()
    {
        if (timeUntilNextShot > 0)
            timeUntilNextShot--;

        if (beingHeld)
        {
            heldDuration++;
        }
    }

    private void SetCharge(int charge)
    {
        float percent;
        if (charge < currentStats.chargeTier)
        {
            percent = (float)charge / (float)currentStats.chargeTier;
            UIManager.INSTANCE.UpdateChargeL(percent);
            state.currentCharge = charge;
        }
        else
        {
            if (charge >= WeaponManager.MAX_CHARGE)
                state.currentCharge = WeaponManager.MAX_CHARGE;
            else
                state.currentCharge = charge;

            percent = (float)(charge - currentStats.chargeTier) /
                (float)(WeaponManager.MAX_CHARGE - currentStats.chargeTier);
            UIManager.INSTANCE.UpdateChargeR(percent);

        }
    }

    public void OnDisable()
    {
        GameManager.AudioEvents.PlayAudio(AudioEventNames.PlayerStopCharge);
    }

    public virtual void OnShootButtonDown()
    {
        beingHeld = true;
    }

    public virtual void OnShootButtonHold()
    {
        SetCharge(state.currentCharge + currentStats.chargeRate);
        //dont slow down until significantly held
        //TODO replace magic number :/
        if (heldDuration > 20)
        {
            movement.SlowDown(slowPercent: currentStats.chargeSlowDown);
            manager.OnCharge();
            float percent = state.currentCharge / (float) WeaponManager.MAX_CHARGE;
            manager.SetEmissionLevel(percent);
        }

        //TODO replace magic number :/
        if (heldDuration == 10)
        {
            GameManager.AudioEvents.PlayAudio(AudioEventNames.PlayerStartCharge);
        }
    }

    public virtual void OnShootButtonRelease()
    {
        GameManager.AudioEvents.PlayAudio(AudioEventNames.PlayerStopCharge);
        //successfully shoot
        //frame limit exists to stop spamming
        if (timeUntilNextShot <= 0)
        {
            timeUntilNextShot = currentStats.minShootInterval;

            if (state.currentCharge >= WeaponManager.MAX_CHARGE)
            {
                ShootMax(manager.GetPlayerDirection(), manager.GetLaunchPosition());
                movement.SlowDown(currentStats.shootSlowMax, currentStats.slowDuration);
            }
            else if (state.currentCharge >= currentStats.chargeTier)
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
        beingHeld = false;
        heldDuration = 0;
        manager.OnChargeEnd();
    }

    //
    public int GetCurrentCharge()
    {
        return state.currentCharge;
    }

    protected virtual Projectile ShootDefault(Vector3 direction, Vector3 pos)
    {
        return CreateProjectile(projectileDefault, direction, pos);
    }

    protected virtual Projectile ShootMedium(Vector3 direction, Vector3 pos)
    {
        return CreateProjectile(projectileMedium, direction, pos);
    }

    protected virtual Projectile ShootMax(Vector3 direction, Vector3 pos)
    {
        return CreateProjectile(projectileMax, direction, pos);
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

    float rapidDuration;
    int startInterval;
    public void RapidFire(float duration)
    {
        startInterval = currentStats.minShootInterval;
        currentStats.minShootInterval = 2;
        rapidDuration = duration;
    }

    IEnumerator rapidFireDuration()
    {
        yield return rapidDuration;
        currentStats.minShootInterval = startInterval;
    }

    public void ForceShot()
    {
        //stub
        throw new NotImplementedException();
    }

    public void FirerateBuffPercent(float percent)
    {
        //stub
        throw new NotImplementedException();
    }

    public void DecreaseDebuffFireratePercent(float percent)
    {
        //stub
        throw new NotImplementedException();
    }

    public void DamageBuffPercent(float percent)
    {
        //stub
        throw new NotImplementedException();
    }

    public void DamageDebuffPercent(float percent)
    {
        //stub
        throw new NotImplementedException();
    }

    public void BoostDamageFixed(int increase)
    {
        //stub
        throw new NotImplementedException();
    }
}