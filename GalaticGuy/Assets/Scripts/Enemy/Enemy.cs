using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterShoot))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    CharacterHealth health = null;
    CharacterShoot shoot;
    CharacterMovement movement;
    Animator anim;
    [SerializeField]
    Enemy[] deathChildren;

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

    public virtual void OnDeath()
    {
        EffectManager.INSTANCE.CreateExplosion(transform.position);
        GameManager.INST.EnemyDeath();
        shoot.DestroyPool();
        Destroy(gameObject);

        foreach(Enemy enemy in deathChildren)
        {
            float r = Random.Range(0f, 1.5f), angle = Random.Range(0f, 2f * Mathf.PI);
            Vector3 location = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle))*r;
            EnemySpawner.INST.SpawnEnemy(enemy, location + transform.position);
        }
    }

    public virtual void OnDamage(int damage)
    {
        anim.SetTrigger(Labels.AnimProperties.DAMAGE);
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
