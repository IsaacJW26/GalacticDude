using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Labels;
using UnityTools.Maths;

public class CharacterAnimator : MonoBehaviour, ICharacterAnimation
{
    [SerializeField]
    Animator animator;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    void ICharacterAnimation.Charge(bool isCharging)
    {
        animator.SetBool(AnimProperties.CHARGING, isCharging);
    }

    void ICharacterAnimation.Move(Vector2 velocity)
    {
        switch(Comparitors.FloatCompare(velocity.y, 0f))
        {
            //moving down
            case -1:
                animator.SetBool(AnimProperties.MOVE_DOWN, true);
                animator.SetBool(AnimProperties.MOVE_UP, false);
                break;
            //not moving
            case 0:
                animator.SetBool(AnimProperties.MOVE_DOWN, false);
                animator.SetBool(AnimProperties.MOVE_UP, false);
                break;
            //moving up
            case 1:
                animator.SetBool(AnimProperties.MOVE_DOWN, false);
                animator.SetBool(AnimProperties.MOVE_UP, true);
                break;
        }

        // left and right movement
        switch(Comparitors.FloatCompare(velocity.x, 0f))
        {
            //velocity is less than 0, moving left
            case -1:
                animator.SetBool(AnimProperties.MOVE_RIGHT, false);
                animator.SetBool(AnimProperties.MOVE_LEFT, true);
                break;
            //velocity is 0, not moving
            case 0:
                animator.SetBool(AnimProperties.MOVE_RIGHT, false);
                animator.SetBool(AnimProperties.MOVE_LEFT, false);
                break;
            //velocity is more than 0, moving right
            case 1:
                animator.SetBool(AnimProperties.MOVE_RIGHT, true);
                animator.SetBool(AnimProperties.MOVE_LEFT, false);
                break;
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
