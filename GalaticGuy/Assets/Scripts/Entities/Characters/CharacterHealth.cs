using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterHealth
{
    [SerializeField]
    private int maxHP = 5;
    private int currentMaxHP;
    private int currentHP;
    BasicMethod deathMethod;

    [SerializeField]
    private AudioEventNames onDamageAudio = AudioEventNames.EnemyHurt;
    [SerializeField]
    private AudioEventNames onDeathAudio = AudioEventNames.EnemyDeath;


    public void InitialiseMethods(BasicMethod deathMethod)
    { 
        this.currentHP = maxHP;
        this.currentMaxHP = maxHP;
        this.deathMethod = deathMethod;
    }

    public CharacterHealth(int maxHP, BasicMethod deathMethod)
    {
        this.maxHP = maxHP;
        this.currentMaxHP = maxHP;
        this.currentHP = maxHP;
        this.deathMethod = deathMethod;
    }

    public CharacterHealth(int maxHP, int currentHP, BasicMethod deathMethod, BasicMethod onDamageMethod)
    {
        this.maxHP = maxHP;
        this.currentMaxHP = maxHP;
        this.currentHP = currentHP;
        this.deathMethod = deathMethod;
    }

    public void OnDamage(int damage)
    {
        int newDamage = Mathf.Abs(damage);
        if ((currentHP - newDamage) <= 0)
        {
            currentHP = 0;
            GameManager.AudioEvents.PlayAudio(onDeathAudio);
            deathMethod();
        }
        else
        {
            GameManager.AudioEvents.PlayAudio(onDamageAudio);
            currentHP -= newDamage;
        }
    }

    public int GetHealth()
    {
        return currentHP;
    }

    public int GetMaxHealth()
    {
        return currentMaxHP;
    }

    public void SetNewMaxHP(int newMaxHP)
    {
        currentMaxHP = newMaxHP;
    }
}
