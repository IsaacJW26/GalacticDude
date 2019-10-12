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
    int IFramesRemaining = 0;
    IEnumerator invulCoroutine;
    Collider2D hitbox;

    [SerializeField]
    ParticleSystem particles;
    ParticleSystem.EmissionModule emission;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        shoot = GetComponent<CharacterShoot>();
        health = new CharacterHealth(3, OnDeath);
        anim = GetComponent<Animator>();
        hitbox = GetComponent<Collider2D>();
        //initialise particles
        emission = particles.emission;
        emission.enabled = false;
        //
        GameManager.INST.InitialisePlayer(SetEnabled);
    }

    void Update()
    {
        //input direction x
        if (chargeCooldown <= 0)
        {
            move.enabled = true;

            move.InputDirectionX(Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)));
        }
        else
        {
            move.enabled = false;
        }
    }

    int chargeCooldown = 0;
    const int chargeCooldownMax = 15;

    private void FixedUpdate()
    {
        //when charge hasn't started in 15 frames
        if (chargeCooldown <= 0)
        {
            //when moving use normal attack
            if (IsInput())
            {
                emission.enabled = false;

                if (chargeMeter >= chargeMax)
                {
                    chargeCooldown = chargeCooldownMax;
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
            }
        }
        else
        {
            chargeCooldown--;
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
        //Damage animation
        anim.SetTrigger(Labels.AnimProperties.DAMAGE);
        
        //update health value
        health.TakeDamage(inDamage);
        //update UI
        UIManager.INSTANCE.RemoveHP(health.GetHealth());

        //set invul
        SetInvulnerable();
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

        //set invulerable while shooting
        SetInvulnerable();
    }

    public void UpdateCharge()
    {
        float percent = (float)chargeMeter / (float)chargeMax;
        UIManager.INSTANCE.UpdateCharge(percent);
    }

    private void SetInvulnerable()
    {
        if (invulCoroutine != null)
            StopCoroutine(invulCoroutine);
        invulCoroutine = SetInvulCoroutine();
        StartCoroutine(invulCoroutine);
    }

    private IEnumerator SetInvulCoroutine()
    {
        //disable hitbox
        hitbox.enabled = false;

        //wait for frame duration
        IFramesRemaining = invulDuration;
        while(IFramesRemaining >= 0)
        {
            yield return null;
            IFramesRemaining--;
        }

        //enable hitbox
        hitbox.enabled = true;
    }

    public void OnDisable()
    {
        move.enabled = false;
        shoot.enabled = false;
        emission.enabled = false;
        chargeCooldown = chargeCooldownMax;
    }

    public void OnEnable()
    {
        if (move != null && shoot != null)
        {
            move.enabled = true;
            shoot.enabled = true;
        }
    }

    public void SetEnabled(bool enabled)
    {
        if (move != null)
            move.enabled = true;
        if(shoot != null)
            shoot.enabled = true;
    }
}
