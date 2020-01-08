using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterHealth))]
[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(CharacterAnimator))]

public class MainCharacter : MonoBehaviour, IDamageable
{
    CharacterMovement move;
    CharacterHealth health;
    WeaponManager weapons;
    ICharacterAnimation anim;

    const int invulDuration = 120;
    int iFramesRemaining = 0;
    IEnumerator invulCoroutine;
    int frozenTimeRemaining;
    List<Collider2D> hitboxes;

    [SerializeField]
    ParticleSystem particles;
    ParticleSystem.EmissionModule emission;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        health = new CharacterHealth(3, OnDeath);
        anim = GetComponent<ICharacterAnimation>();
        weapons = GetComponent<WeaponManager>();
        hitboxes = GetComponentsInChildren<Collider2D>().ToList();

        //initialise particles
        emission = particles.emission;
        emission.enabled = false;

        GameManager.INST.InitialisePlayer(SetEnabled);
        //GameManager.INST.SetPlayer(this);
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
        anim.OnDeath();
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
        if (iFramesRemaining <= 0)
        {
            //Damage animation
            anim.OnDamage();

            //update health value
            health.TakeDamage(inDamage);
            //update UI
            UIManager.INSTANCE.RemoveHP(health.GetHealth());

            //set invul
            SetInvulnerable();
        }
    }

    //Freeze player for given frames
    public void FreezePlayer(int freezeDuration)
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
        EnableHitboxes(false);

        //wait for frame duration
        iFramesRemaining = invulDuration;
        while(iFramesRemaining >= 0)
        {
            yield return null;
            iFramesRemaining--;
        }

        //enable hitbox
        EnableHitboxes(true);
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
        EnableHitboxes(enabled);
        weapons.enabled = enabled;
        this.enabled = enabled;
    }

    private void EnableHitboxes(bool enabled)
    {
        hitboxes.ForEach(x => x.enabled = enabled);
    }
}
