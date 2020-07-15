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

    private void OnEnable()
    {
        hp = baseHP;
        sprite = GetComponent<SpriteRenderer>();
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
