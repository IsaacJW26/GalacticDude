using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour, IDamageable
{
    CharacterMovement move;
    CharacterHealth health;
    WeaponManager weapons; 
    Animator anim;

    const int invulDuration = 120;
    int IFramesRemaining = 0;
    IEnumerator invulCoroutine;
    int frozenTimeRemaining;
    Collider2D hitbox;

    [SerializeField]
    ParticleSystem particles;
    ParticleSystem.EmissionModule emission;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        health = new CharacterHealth(3, OnDeath);
        anim = GetComponent<Animator>();
        weapons = GetComponent<WeaponManager>();
        hitbox = GetComponent<Collider2D>();
        //initialise particles
        emission = particles.emission;
        emission.enabled = false;
        //
        GameManager.INST.InitialisePlayer(SetEnabled);
        GameManager.INST.SetPlayer(this);
    }

    void Update()
    {
        //input direction x
        if (frozenTimeRemaining <= 0)
        {
            move.enabled = true;

            move.InputDirectionX(Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)));
            if (Input.GetButtonDown(Labels.Inputs.SHOOT))
            {
                weapons.OnShootButtonDown();
            }
            else if(Input.GetButton(Labels.Inputs.SHOOT))
            {
                weapons.OnShootButtonHold();
            }
            else if(Input.GetButtonUp(Labels.Inputs.SHOOT))
            {
                weapons.OnShootButtonRelease();
            }
        }
        else
        {
            move.enabled = false;
        }
    }

    private void FixedUpdate()
    {
        //count down to unfreeze character
        if (frozenTimeRemaining > 0)
            frozenTimeRemaining--;
    }

    public void OnDeath()
    {
        GameManager.INST.PlayerDeath();
        anim.SetBool(Labels.AnimProperties.DEATH, true);
        SetEnabled(false);
    }

    [ContextMenu("Take Damage")]
    public void OnDamage()
    {
        OnDamage(1);
    }

    public void OnDamage(int inDamage)
    {
        //dont do damage if invuln
        if (IFramesRemaining <= 0)
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
    }

    //try shoot normal attack
    public void ShootNormal()
    {

    }

    //Freeze player for given frames
    public void freezePlayer(int freezeDuration)
    {
        frozenTimeRemaining = freezeDuration;
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
        weapons.enabled = false;
        emission.enabled = false;
    }

    public void OnEnable()
    {
        if (move != null && weapons != null)
        {
            move.enabled = true;
            weapons.enabled = true;
        }
    }

    public void SetEnabled(bool enabled)
    {
        hitbox.enabled = enabled;
        weapons.enabled = enabled;
        this.enabled = enabled;
    }
}
