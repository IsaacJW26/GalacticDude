using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerUpgrade
{
    private CharacterMovement movement;
    protected MainCharacter player;
    private CharacterShoot shoot;

    //purchase info
    [SerializeField]
    protected Sprite sprite;
    [SerializeField]
    protected int cost;

    //public getters
    public int Cost { get { return cost; } }
    public Sprite Sprite { get { return sprite; } }

    //Intitialise values of player attributes
    public virtual void Initialise(CharacterMovement movement, MainCharacter player, CharacterShoot shoot)
    {
        this.movement = movement;
        this.player = player;
        this.shoot = shoot;
    }

    //every frame update enemy, give character info and positional info
    public abstract void UpdateFrame(Vector3 currentPosition);

    //
    public virtual void OnMove(Vector3 direction)
    {
        
    }

    public virtual void OnShoot(Vector3 direction)
    {

    }

    public virtual void OnRemove()
    {

    }
}