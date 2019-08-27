using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISideBasic : EnemyAI
{
    readonly float moveDownDistance = 0.2f;
    [SerializeField]
    float lowestPosition = 0f;
    bool movingDown = true;
    bool movingRight = true;

    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        Vector3 direction;
        if (movingRight)
            direction = Vector3.right;
        else
            direction = Vector3.left;

        //near edges move down
        if(distanceToXBound() < moveDownDistance)
        {
            //check if it is above the lowest position
            if (transform.position.y >= lowestPosition)
                movingDown = true;
            else
                movingDown = false;

            //if it reaches right bound turn around
            if (transform.position.x >= CharacterMovement.xBound && movingRight)
            {
                movingRight = false;
            }
            //when left bound is reached turn around
            else if(transform.position.x <= -CharacterMovement.xBound && !movingRight)
            {
                movingRight = true;
            }
        }
        else
        {
            movingDown = false;
        }

        if (movingDown)
            direction += Vector3.down;


        Move(direction);
        //Shoot(Vector3.down);
    }

    private float distanceToXBound()
    {
        return Mathf.Abs(Mathf.Abs(transform.position.x) - (CharacterMovement.xBound));
    }
}
