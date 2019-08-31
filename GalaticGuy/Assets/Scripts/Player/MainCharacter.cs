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
    Animator anim;

    int chargeMeter = 0;
    //a second and a half
    const int chargeMax = 900;

    const int invulDuration = 30;
    int timeTillInvulnerable = 0;
    IEnumerator invulCoroutine;
    Collider2D hitbox;

    [SerializeField]
    ParticleSystem particles;
    ParticleSystem.EmissionModule emission;
    ParticleSystem.VelocityOverLifetimeModule particleVelocity;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        shoot = GetComponent<CharacterShoot>();
        health = new CharacterHealth(3, OnDeath);
        anim = GetComponent<Animator>();
        hitbox = GetComponent<Collider2D>();

        emission = particles.emission;
        particleVelocity = particles.velocityOverLifetime;
        emission.enabled = false;
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
            emission.enabled = false;
            particleVelocity.orbitalZ = 0f;

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
            emission.enabled = true;
            particleVelocity.orbitalZ = 2f;
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
        anim.SetTrigger("Damage");
        health.TakeDamage(inDamage);
        UIHp.INSTANCE.RemoveHP(health.GetHealth());
        //set invul
        if (invulCoroutine != null)
            StopCoroutine(invulCoroutine);
        invulCoroutine = SetInvulnerable();
        StartCoroutine(invulCoroutine);
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

    public IEnumerator SetInvulnerable()
    {
        //disable hitbox
        hitbox.enabled = false;

        //wait for frame duration
        timeTillInvulnerable = invulDuration;
        while(timeTillInvulnerable >= 0)
        {
            yield return null;
            timeTillInvulnerable--;
        }

        //enable hitbox
        hitbox.enabled = true;
    }
}
