using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Labels;

public class CharacterAnimator : MonoBehaviour, ICharacterAnimation
{
    [SerializeField]
    Animator animator;

    void ICharacterAnimation.Charge(bool isCharging)
    {
        animator.SetBool(AnimProperties.CHARGING, isCharging);
    }

    void ICharacterAnimation.Move(Vector2 velocity)
    {
        if (compareVelocity(velocity.y, 0f))
        {
            animator.SetBool(AnimProperties.MOVE_DOWN, false);
            animator.SetBool(AnimProperties.MOVE_UP, false);
        }
        else if(velocity.y > 0f)
        {
            animator.SetBool(AnimProperties.MOVE_DOWN, false);
            animator.SetBool(AnimProperties.MOVE_UP, true);
        }
        else if(velocity.y < 0f)
        {
            animator.SetBool(AnimProperties.MOVE_DOWN, true);
            animator.SetBool(AnimProperties.MOVE_UP, false);
        }

        // left and right movement
        if (compareVelocity(velocity.x, 0f))
        {
            animator.SetBool(AnimProperties.MOVE_RIGHT, false);
            animator.SetBool(AnimProperties.MOVE_LEFT, false);
        }
        else if (velocity.x > 0f)
        {
            animator.SetBool(AnimProperties.MOVE_RIGHT, true);
            animator.SetBool(AnimProperties.MOVE_LEFT, false);
        }
        else if (velocity.x < 0f)
        {
            animator.SetBool(AnimProperties.MOVE_RIGHT, false);
            animator.SetBool(AnimProperties.MOVE_LEFT, true);
        }

        //nested function
        bool compareVelocity(float valueX, float valueY)
        {
            return Mathf.Abs(valueX - valueY) < 0.001f;
        }
    }

    void ICharacterAnimation.OnDeath()
    {
        animator.SetTrigger(AnimProperties.DEATH_TRIG);
    }

    void ICharacterAnimation.OnDamage()
    {
        animator.SetTrigger(AnimProperties.DAMAGE_TRIG);
    }

    void ICharacterAnimation.Shoot(int level)
    {
        throw new System.NotImplementedException();
    }
}

public interface ICharacterAnimation
{
    void OnDeath();
    void OnDamage();
    void Charge(bool isCharging);
    void Shoot(int level);
    void Move(Vector2 velocity);
}
