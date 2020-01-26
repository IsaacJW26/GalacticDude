using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterShoot))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(Animator))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    CharacterHealth health = null;
    CharacterShoot shoot;
    Movement movement;
    Animator anim;
    [SerializeField]
    Enemy[] deathChildren;

    protected EnemyAI Ai;
    bool isDead = false;

    void Awake()
    {
        shoot = GetComponent<CharacterShoot>();
        movement = GetComponent<Movement>();
        Ai = GetComponent<EnemyAI>();
        anim = GetComponent<Animator>();

        health?.InitialiseMethods(OnDeath);
        Ai?.Initialise(movement, null, shoot);
    }

    void FixedUpdate()
    {
        Ai.UpdateFrame(transform.position);

        //destroy once end of level is reached
        if (transform.position.y < -7)
            OnDeath();
    }

    public virtual void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            EffectManager.INSTANCE.CreateExplosion(transform.position);
            shoot.DestroyPool();

            foreach (Enemy enemy in deathChildren)
            {
                float r = Random.Range(0.5f, 1.0f) * transform.localScale.magnitude, angle = Random.Range(0f, 2f * Mathf.PI);
                Vector3 location = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * r + transform.position;
                if (location.y > -1.5f)
                    EnemySpawner.INST.SpawnEnemy(enemy, location);
            }

            StartCoroutine(WaitToDie());
        }
    }

    IEnumerator WaitToDie()
    {
        yield return null;
        GameManager.INST.EnemyDeath();
        yield return null;
        Destroy(gameObject);
    }

    public virtual void OnDamage(int damage)
    {
        anim.SetTrigger(Labels.AnimProperties.DAMAGE_TRIG);
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

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == Labels.Tags.PLAYER)
        {
            MainCharacter player = other.GetComponentInParent<MainCharacter>();
            player.OnDamage();
            OnDeath();
        }
    }
}
