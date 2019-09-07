using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterShoot))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(EnemyAI))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    CharacterHealth health = null;
    CharacterShoot shoot;
    CharacterMovement movement;
    Animator anim;

    protected EnemyAI Ai;

    void Awake()
    {
        shoot = GetComponent<CharacterShoot>();
        movement = GetComponent<CharacterMovement>();
        Ai = GetComponent<EnemyAI>();
        anim = GetComponent<Animator>();

        health?.InitialiseMethods(OnDeath);
        Ai?.Initialise(movement, null, shoot);
    }

    void FixedUpdate()
    {
        Ai.UpdateFrame(transform.position);
    }

    public void OnDeath()
    {
        EffectManager.INSTANCE.CreateExplosion(transform.position);
        GameManager.INST.EnemyDeath();
        shoot.DestroyPool();
        Destroy(gameObject);
    }

    public void OnDamage(int damage)
    {
        anim.SetTrigger("Damage");
        health.TakeDamage(damage);

        if(damage <= 2)
            EffectManager.INSTANCE.ScreenShakeSmall();
        /*
        else if(damage <= 4)
            ScreenShake.INSTANCE.MediumShake();
        else if(damage >= 5)
            ScreenShake.INSTANCE.BigShake();
            */
        //Stub
    }
}
