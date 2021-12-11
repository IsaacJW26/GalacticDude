using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Projectile
{
    [SerializeField]
    int baseHP = 5;

    float percentHP;
    SpriteRenderer sprite;
    Rigidbody2D enemyRb;
    Vector3 offset;

    private void OnEnable()
    {
        stats.hp = baseHP;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Initialise(Rigidbody2D inEnemyRB, Vector3 offset)
    {
        enemyRb = inEnemyRB;

        this.offset = offset;
    }

    protected override void FixedUpdate()
    {
        if(enemyRb)
        {
            if(rb == null)
                rb = GetComponent<Rigidbody2D>();

            Vector3 pos = enemyRb.position + (Vector2)offset;

            rb.MovePosition(pos);
        }
    }

    public override void OnDamage(int inDamage)
    {
        stats.hp -= inDamage;

        if(stats.hp <= 0)
        {
            EffectManager.INSTANCE.CreateExplosion(transform.position, 1f);
            OnDeath();
        }
        else
        {
            percentHP = (float)stats.hp / (float)baseHP;
            sprite.color = new Color(1f, 1f, 1f, percentHP);
        }
    }

    public override void OnDeath()
    {
        Destroy(gameObject);
    }


}
