using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIUpDownShoot : EnemyAI
{
    [SerializeField]
    int MoveDurationMin = 10;
    [SerializeField]
    int MoveDurationMax = 60;
    int timeUntilNextMove = 0;
    [SerializeField]
    float lowestPosition = -2f;
    bool movingDown = true;

    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        if(timeUntilNextMove <= 0)
        {
            float randomVal = Random.Range(0f, 1f);
            float normalizedy = (currentPosition.y+2f) / 5f;

            //move forward if random value is greater than y
            movingDown = (randomVal < normalizedy);

            //resetnext moves
            timeUntilNextMove = Random.Range(MoveDurationMin, MoveDurationMax);
        }
        timeUntilNextMove--;

        if (movingDown)
        {
            movingDown = movingDown && (currentPosition.y > lowestPosition);
            Move(Vector3.down);
        }
        else
            Move(Vector3.up);

        Shoot(Vector3.down);
    }
}
