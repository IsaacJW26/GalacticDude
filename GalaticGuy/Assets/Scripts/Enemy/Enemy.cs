using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    CharacterHealth health;

    void Start()
    {
        health = new CharacterHealth(5, OnDeath);
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals(Labels.TAGS.PROJECTILE))
        {
            health.TakeDamage(1);
        }
    }

    public void OnDeath()
    {
        Destroy(gameObject);
    }
}
