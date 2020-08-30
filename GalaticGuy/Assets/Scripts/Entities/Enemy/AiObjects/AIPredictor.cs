using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPredictor : EnemyAI
{
    readonly float moveDownDistance = 0.6f;
    [SerializeField]
    float lowestPosition = 0f;
    bool movingDown = true;
    bool movingRight = true;

    Vector3 lastPlayerPos;

    Vector3 dir;

    public void Awake()
    {
        if (transform.position.y < lowestPosition)
            movingDown = false;

        lastPlayerPos = GameManager.INST.GetPlayerPos();
        dir = Vector3.down;
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

        Vector3 velocity = (GameManager.INST.GetPlayerPos() - lastPlayerPos) / Time.fixedDeltaTime;
        if (velocity.magnitude > 0f)
        {
            var player = GameManager.INST.GetPlayerPos();
            var ourPos = transform.position;
            float angle = Mathf.Acos((player.x * ourPos.x + player.y * ourPos.y) / (velocity.magnitude * shoot.ProjectileSpeed));
            float x = Mathf.Cos(angle);
            float y = -Mathf.Sin(angle);
            dir = new Vector3(x, y).normalized;

            //get player direction
            //dir = (( + velocity * projectionFactor) - transform.position).normalized;
            lastPlayerPos = GameManager.INST.GetPlayerPos();
        }
        Shoot(dir);
    }

    private static Vector2 CalculateCollision(Vector3 playerVelocity, float projectileSpeed, Vector3 position, Vector3 playerPos)
    {
        Vector2 direction;
        if (playerVelocity.magnitude > 0f)
        {
            float angle = Mathf.Acos((playerPos.x * position.x + playerPos.y * position.y) / (playerVelocity.magnitude * projectileSpeed));
            float x = Mathf.Cos(angle);
            float y = -Mathf.Sin(angle);
            direction = new Vector2(x, y).normalized;
        }
        else
        {
            direction = (playerPos - position).normalized;
        }
        return direction;
    }

    [ContextMenu("TestCollisions")]
    public void Test()
    {
        Vector2 test1 = CalculateCollision(
            playerVelocity: new Vector2(2f, 0f),
            projectileSpeed: 5f,
            position:  new Vector2(2f, 0f),
            playerPos: new Vector2(2f, -5f));
        Debug.Log($"{test1} = {new Vector2(-3f, -5f)}");
    }

    private float DistanceToXBound()
    {
        return Mathf.Abs(Mathf.Abs(transform.position.x) - (Movement.xBound));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, dir * shoot.ProjectileSpeed);
    }
}
