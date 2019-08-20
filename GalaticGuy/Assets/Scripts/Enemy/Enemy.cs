using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterShoot))]
[RequireComponent(typeof(CharacterMovement))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    CharacterHealth health = null;
    CharacterShoot shoot;
    CharacterMovement movement;

    protected EnemyAI Ai;

    void Awake()
    {
        health?.InitialiseMethods(OnDeath);
        shoot = GetComponent<CharacterShoot>();
        movement = GetComponent<CharacterMovement>();
        Ai = new BasicAI(movement, null, shoot);
    }

    void FixedUpdate()
    {
        Ai.UpdateFrame(transform.position);
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }

    public void OnDamage(int damage)
    {
        health.TakeDamage(damage);

        if(damage <= 2)
            ScreenShake.INSTANCE.SmallShake();
        else if(damage <= 4)
            ScreenShake.INSTANCE.MediumShake();
        else if(damage >= 5)
            ScreenShake.INSTANCE.BigShake();

        //Stub
    }
}
