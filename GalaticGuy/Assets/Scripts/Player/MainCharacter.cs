using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour, IDamageable
{
    CharacterMovement move;
    CharacterShoot shoot;
    CharacterHealth health;
    [SerializeField]
    CharacterShoot laserShoot;

    int chargeMeter = 0;
    //a second and a half
    int chargeMax = 900;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        shoot = GetComponent<CharacterShoot>();
        health = new CharacterHealth(3, OnDeath);
    }

    // Update is called once per frame
    void Update()
    {
        //input direction x
        move.InputDirectionX(Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)));
    }

    private void FixedUpdate()
    {
        //when moving use normal attack
        if (IsInput())
        {
            if (chargeMeter >= chargeMax)
            {
                ShootSpecial();
            }
            else
            {
                ShootNormal();
            }
        }
        //charge special when not moving
        else
        {
            ChargeMeter();
        }
    }

    public void ChargeMeter()
    {
        if (chargeMeter >= chargeMax)
        {

        }
        else
            chargeMeter++;

        UpdateCharge();
    }

    public bool IsInput()
    {
        return Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)) != 0;
    }

    public void OnDeath()
    {
        //stub
    }

    public void OnDamage(int inDamage)
    {
        health.TakeDamage(inDamage);
        UIHp.INSTANCE.UpdateHP(health.GetHealth());
    }

    //try shoot normal attack
    public void ShootNormal()
    {
        shoot.TryShoot(Vector3.up * 3f);
    }

    //try shoot special attack
    public void ShootSpecial()
    {
        chargeMeter = 0;
        UpdateCharge();
        laserShoot.TryShoot(Vector3.up);
        //stub
    }

    public void UpdateCharge()
    {
        float percent = (float)chargeMeter / (float)chargeMax;
        UICharge.INSTANCE.UpdateCharge(percent);
    }
}
