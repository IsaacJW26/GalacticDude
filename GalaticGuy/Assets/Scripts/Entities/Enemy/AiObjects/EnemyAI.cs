using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAI : MonoBehaviour
{
    protected Movement movement;
    protected MainCharacter player;
    protected CharacterShoot shoot;

    public virtual void Initialise(Movement movement, MainCharacter player, CharacterShoot shoot)
    {
        this.movement = movement;
        this.player = player;
        this.shoot = shoot;
    }

    //every frame update enemy, give character info and positional info
    public abstract void UpdateFrame(Vector3 currentPosition);

    public void Move(Vector3 direction)
    {
        movement.InputDirectionX(direction.x);
        movement.InputDirectionY(direction.y);
    }

    public void Shoot(Vector3 direction)
    {
        shoot.TryShoot(direction);
    }

    public virtual void OnDeath()
    { }

    public virtual void OnDamage(CharacterHealth health)
    { }
}
