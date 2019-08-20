using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI
{
    private CharacterMovement movement;
    protected MainCharacter player;
    private CharacterShoot shoot;

    protected EnemyAI(CharacterMovement movement, MainCharacter player, CharacterShoot shoot)
    {
        this.movement = movement;
        this.player = player;
        this.shoot = shoot;
    }

    //every frame update enemy, give character info and positional info
    public abstract void UpdateFrame(Vector3 currentPosition);

    public void Move(Vector3 direction)
    {
        int xdir = Mathf.RoundToInt(direction.x);
        int ydir = Mathf.RoundToInt(direction.y);

        movement.InputDirectionX(xdir);
        movement.InputDirectionY(ydir);
    }

    public void Shoot(Vector3 direction)
    {
        shoot.TryShoot(direction);
    }
}
