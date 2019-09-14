using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerUpgrade
{
    private CharacterMovement movement;
    protected MainCharacter player;
    private CharacterShoot shoot;

    public void Initialise(CharacterMovement movement, MainCharacter player, CharacterShoot shoot)
    {
        this.movement = movement;
        this.player = player;
        this.shoot = shoot;
    }

    //every frame update enemy, give character info and positional info
    public abstract void UpdateFrame(Vector3 currentPosition);

    //
    public void OnMove(Vector3 direction)
    {
        
    }

    public void OnShoot(Vector3 direction)
    {

    }

    public void OnRemove()
    {

    }
}
