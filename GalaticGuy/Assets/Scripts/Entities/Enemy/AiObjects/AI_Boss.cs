using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBoss : EnemyAI
{
    readonly float moveDownDistance = 0.6f;
    [SerializeField]
    float lowestPosition = 0f;
    bool movingDown = true;
    bool movingRight = true;

    [SerializeField]
    GameObject[] rings = null;

    public override void Initialise(Movement movement, MainCharacter player, CharacterShoot shoot)
    {
        base.Initialise(movement, player, shoot);
        if (transform.position.y < lowestPosition)
            movingDown = false;
    }

    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        Vector3 direction;
        if (movingRight)
            direction = Vector3.right;
        else
            direction = Vector3.left;

        //near edges move down
        if (DistanceToXBound() < moveDownDistance)
        {
            //check if it is above the lowest position
            if (transform.position.y >= lowestPosition)
                movingDown = true;
            else
                movingDown = false;

            //if it reaches right bound turn around
            if (transform.position.x >= Movement.xBound && movingRight)
            {
                movingRight = false;
            }
            //when left bound is reached turn around
            else if (transform.position.x <= -Movement.xBound && !movingRight)
            {
                movingRight = true;
            }
        }
        else
        {
            movingDown = false;
        }

        if (movingDown && (transform.position.y >= lowestPosition))
            direction += Vector3.down;

        Move(direction);

        //get player direction
        Vector3 dir = (GameManager.INST.GetPlayerPos() - transform.position).normalized;

        Shoot(dir);
    }

    private float DistanceToXBound()
    {
        return Mathf.Abs(Mathf.Abs(transform.position.x) - (Movement.xBound));
    }

    // remove a ring on hit
    public override void OnHit(CharacterHealth health)
    {
        int hpStageInterval = (health.GetMaxHealth() / 4);
        // 4 stages
        // first is 3 rings to 2 rings
        // 2 rings to 1
        // 1 ring to none
        // no rings to dead
        const int totalHpStages = 4;

        //check if hp is below any health bracket
        for(int ii = 0; ii < totalHpStages - 1; ii--)
        {
            // when first hp bracket is found, disable the rest
            if(health.GetHealth() < ii * hpStageInterval)
            {
                // disable the rings from last to current
                for(int jj = totalHpStages - 1; jj < ii; jj++)
                {
                    rings[jj].SetActive(false);
                }
                break;
            }
        }
    }
}
