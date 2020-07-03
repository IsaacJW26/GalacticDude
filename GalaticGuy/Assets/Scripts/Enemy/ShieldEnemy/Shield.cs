using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IDamageable
{
    [SerializeField]
    int baseHP = 5;

    int hp = 5;
    float percentHP;
    SpriteRenderer sprite;
    Rigidbody2D rb;
    Rigidbody2D enemyRb;

    private void OnEnable()
    {
        hp = baseHP;
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    public void Initialise(Rigidbody2D inEnemyRB)
    {
        enemyRb = inEnemyRB;
    }

    private void LateUpdate()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody2D>();
        Vector3 pos = enemyRb.position;

        rb.MovePosition(pos);
    }

    void IDamageable.OnDamage(int inDamage)
    {
        hp -= inDamage;

        if(hp <= 0)
        {
            Destroy(gameObject);
        }
        else
        {
            percentHP = (float)hp / (float)baseHP;
            sprite.color = new Color(1f, 1f, 1f, percentHP);
        }
    }
}
