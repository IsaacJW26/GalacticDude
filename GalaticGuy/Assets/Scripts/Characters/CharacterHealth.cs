using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth
{
    public delegate void OnDeath();

    private int maxHP;
    private int currentHP;
    OnDeath deathMethod;

    public CharacterHealth(int maxHP, OnDeath deathMethod)
    {
        this.maxHP = maxHP;
        this.currentHP = maxHP;
        this.deathMethod = deathMethod;
    }

    public CharacterHealth(int maxHP, int currentHP, OnDeath deathMethod)
    {
        this.maxHP = maxHP;
        this.currentHP = currentHP;
        this.deathMethod = deathMethod;
    }

    public void TakeDamage(int damage)
    {
        int newDamage = Mathf.Abs(damage);
        if ((currentHP - newDamage) <= 0)
        {
            currentHP = 0;
            deathMethod();
        }
        else
        {
            currentHP -= newDamage;
        }
    }
}
