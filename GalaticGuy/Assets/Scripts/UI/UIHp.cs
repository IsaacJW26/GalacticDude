using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHp : MonoBehaviour
{
    [SerializeField]
    Animator[] HpImages = null;

    int currentHP;

    void Awake()
    {
        currentHP = 3;
    }

    public void UpdateHP(int hp)
    {
        currentHP = hp;
        /*
        for(int i = 0; i < HPimages.Length; i++)
        {
            if (i > (hp - 1))
            {
                HPimages[i].enabled = false;
            }
            else
            {
                HPimages[i].enabled = true;
            }
        }
        */
    }

    public void RemoveHP(int newhp)
    {
        for (int i = newhp; i < currentHP; i++)
        {
            HpImages[i].SetTrigger("Damage");
        }
        currentHP = newhp;
    }

    [ContextMenu("HP 0")]
    public void Hp0()
    {
        UpdateHP(0);
    }

    [ContextMenu("HP 1")]
    public void Hp1()
    {
        UpdateHP(1);
    }

    [ContextMenu("HP 2")]
    public void Hp2()
    {
        UpdateHP(2);
    }

    [ContextMenu("HP 3")]
    public void Hp3()
    {
        UpdateHP(3);
    }
}
