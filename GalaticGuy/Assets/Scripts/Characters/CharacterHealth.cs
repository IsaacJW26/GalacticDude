using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterHealth
{
    public delegate void UpdateDel();

    [SerializeField]
    private int maxHP = 5;
    private int currentHP;
    UpdateDel deathMethod;

    public void InitialiseMethods(UpdateDel deathMethod)
    { 
        this.currentHP = maxHP;
        this.deathMethod = deathMethod;
    }

    public CharacterHealth(int maxHP, UpdateDel deathMethod)
    {
        this.maxHP = maxHP;
        this.currentHP = maxHP;
        this.deathMethod = deathMethod;
    }

    public CharacterHealth(int maxHP, int currentHP, UpdateDel deathMethod, UpdateDel onDamageMethod)
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
