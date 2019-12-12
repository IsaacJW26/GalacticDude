using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    List<PlayerUpgrade> currentUpgrades;
    //ParticleSystem chargeParticles;
    //ParticleSystem.EmissionModule emissionModule;

    //float defaultEmission;

    bool held = false;

    // Start is called before the first frame update
    void Start()
    {
        if (currentUpgrades == null)
            currentUpgrades = new List<PlayerUpgrade>();
        InitialiseAll();
    }


    public void AddUpgrade(PlayerUpgrade newUpgrade)
    {
        newUpgrade.Initialise(GetComponent<CharacterMovement>(), 
            GetComponent<MainCharacter>(), 
            GetComponent<WeaponManager>().GetCurrentWeapon());
        currentUpgrades.Add(newUpgrade);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (held)
        {
            AllCharging();
        }

        UpdateAll();
    }


    public void OnShootButtonDown()
    {
        if (!held)
        {
            held = true;

            AllCharge();
        }
    }

    public void OnShootButtonRelease()
    {
        if (held)
        {
            held = false;

            AllShoot();
        }
    }

    private void AllShoot()
    {
        foreach(PlayerUpgrade upgrade in currentUpgrades)
        {
            upgrade.OnShoot();
        }
    }

    private void AllCharge()
    {
        foreach (PlayerUpgrade upgrade in currentUpgrades)
        {
            upgrade.OnChargeStart();
        }
    }

    private void AllCharging()
    {
        foreach(PlayerUpgrade upgrade in currentUpgrades)
        {
            upgrade.OnCharging();
        }
    }

    private void AllMove(Vector3 direction)
    {
        foreach (PlayerUpgrade upgrade in currentUpgrades)
        {
            upgrade.OnMove(direction.normalized);
        }
    }

    private void RemoveAll()
    {
        foreach (PlayerUpgrade upgrade in currentUpgrades)
        {
            upgrade.OnRemove();
        }
    }

    private void InitialiseAll()
    {
        foreach (PlayerUpgrade upgrade in currentUpgrades)
        {
            IWeapon weapon = GetComponent<WeaponManager>().GetCurrentWeapon();

            upgrade.Initialise(
                GetComponent<CharacterMovement>(),
                GetComponent<MainCharacter>(),
                weapon);
        }
    }

    private void UpdateAll()
    {
        foreach (PlayerUpgrade upgrade in currentUpgrades)
        {
            upgrade.UpdateFrame(transform.position);
        }
    }

    //testing-----------------------
    
    [ContextMenu("Add Upgrade - Test")]
    public void TestAddUpgrade()
    {
        AddUpgrade(UpgradeCollection.GetUpgrade(0));
    }
}
