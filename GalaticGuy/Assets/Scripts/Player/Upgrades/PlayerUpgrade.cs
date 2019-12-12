using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerUpgrade
{
    protected ISpeed movement;
    protected IDamageable player;
    protected IWeapon weapon;

    protected Sprite sprite;
    public readonly int cost = -1;

    //public getters
    public Sprite Sprite { get { return sprite; } }
    
    public PlayerUpgrade()
    {
        Debug.LogError("Default constructor is invalid, members must be initialised");
    }

    public PlayerUpgrade(Sprite sprite)
    {
        this.sprite = sprite;
    }

    //Intitialise values of player attributes
    public virtual void Initialise(ISpeed movement, MainCharacter player, IWeapon weapon)
    {
        this.movement = movement;
        this.player = player;
        this.weapon = weapon;

        //Initialise stat boosts here<<<<?????
    }

    public void UpdateWeapon(IWeapon weapon)
    {
        this.weapon = weapon;
    }

    //every frame update enemy, give character info and positional info
    public abstract void UpdateFrame(Vector3 currentPosition);

    //
    public abstract void OnMove(Vector3 direction);


    public abstract void OnShoot();


    public abstract void OnRemove();


    public abstract void OnChargeStart();


    public abstract void OnCharging();
}