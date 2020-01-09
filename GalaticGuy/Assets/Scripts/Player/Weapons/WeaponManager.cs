using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools.MathTools;

public class WeaponManager : MonoBehaviour, IOnChargeCallback
{
    public const int MAX_CHARGE = 1800;

    [SerializeField]
    Weapon current;
    [SerializeField]
    Transform projectilePointDefault;
    [SerializeField]
    ParticleSystem chargeParticles;
    ParticleSystem.EmissionModule emissionModule;

    float defaultEmission;
    Vector3 defaultSize;

    bool held = false;
    
    // Start is called before the first frame update
    void Start()
    {
        current.Awake();
        current.Initialise(GetComponent<CharacterMovement>(), this);
        emissionModule = chargeParticles.emission;
        defaultSize = chargeParticles.transform.localScale;
        defaultEmission = emissionModule.rateOverTime.Evaluate(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current.FixedUpdate();
        if(held)
        {
            current.OnShootButtonHold();
        }
    }

    public void OnShootButtonDown()
    {
        if (!held)
        {
            held = true;

            current.OnShootButtonDown();
        }
    }

    public void OnShootButtonHold()
    {
        //current.OnShootButtonHold();
    }

    public IWeapon GetCurrentWeapon()
    {
        return current;
    }

    public void OnShootButtonRelease()
    {
        if (held)
        {
            held = false;

            current.OnShootButtonRelease(this);
        }
    }

    public Vector2 GetPlayerDirection()
    {
        return Vector2.up;
    }

    public Vector3 GetLaunchPosition()
    {
        return projectilePointDefault.position;
    }

    public void OnCharge()
    {
        emissionModule.enabled = true;
    }

    public void OnChargeEnd()
    {
        emissionModule.enabled = false;
    }

    public void SetEmissionLevel(float emissionPercent)
    {
        //
        chargeParticles.transform.localScale = emissionPercent * defaultSize;

        //

        rfloat rateRange = new rfloat(0.3f, 1.2f);

        ParticleSystem.MinMaxCurve rate = emissionModule.rateOverTime;
        rate.constant = defaultEmission * rateRange.LerpValue(emissionPercent);

        emissionModule.rateOverTime = rate;
    }
}
