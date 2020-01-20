using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidAI : EnemyAI
{
    Vector2 direction;
    Vector3 rotationAxis;
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    Transform model;
    Enemy asteroid;
    CharacterMovement move;

    int frame;
    const int rotateUpdateTime = 10;

    private void Awake()
    {
        frame = -rotateUpdateTime;

        asteroid = GetComponentInParent<Enemy>();
        move = GetComponent<CharacterMovement>();
        rotationAxis = GetRandomAxis();

        float xVal = Random.Range(-2.5f, 2.5f);

        direction = new Vector2(xVal, -1f).normalized;

        float scale = Random.Range(0.8f, 1.2f);
        transform.localScale *= scale;

        move.speed *= Random.Range(0.8f, 1.2f);
    }

    private Vector3 GetRandomAxis()
    {
        float x, y, z;
        x = Random.Range(-1f, 1f);
        y = Random.Range(-1f, 1f);
        z = Random.Range(-1f, 1f);

        return new Vector3(x, y, z);
    }

    public override void UpdateFrame(Vector3 currentPosition)
    {
        float bound = CharacterMovement.xBound - 0.01f;
        bool collideRight = ((bound) < transform.position.x);
        bool movingRight = (direction.x > 0f);
        bool collideLeft = (-bound > transform.position.x);
        bool movingLeft = (direction.x < 0f);
        if ((collideRight && movingRight) || (collideLeft && movingLeft))
        {
            direction = new Vector2(-direction.x, direction.y);
        }

        Move(direction);

        //only execute physics every third frame
        if (frame >= rotateUpdateTime)
        {
            frame = 0;
            model.RotateAround(model.position, rotationAxis, rotationSpeed * Time.fixedDeltaTime*10f);
        }
        else
        {
            frame++;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Labels.Tags.ENEMY)
        {
            AsteroidAI asteroid = collision.GetComponentInParent<AsteroidAI>();
            if (asteroid != null)
            {
                Redirect(collision.transform.position);
            }
            else
            {
                Redirect(collision.transform.position);
                Enemy enemy = collision.GetComponent<Enemy>();
                enemy?.OnDamage(2);
            }
        }

        else if(collision.tag == Labels.Tags.PLAYER)
        {
            MainCharacter main = collision.GetComponentInParent<MainCharacter>();
            main.OnDamage(1);
            asteroid.OnDeath();
        }
        else if(collision.tag == Labels.Tags.PROJECTILE)
        {
            Projectile projectile = collision.GetComponentInParent<Projectile>();
            projectile?.OnDamage(1);
        }
    }

    public void Redirect(Vector3 collisionPoint)
    {
        bool collideRight = (collisionPoint.x > transform.position.x);
        bool movingRight = (direction.x > 0f);
        bool collideLeft = (collisionPoint.x < transform.position.x);
        bool movingLeft = (direction.x < 0f);

        //flip direction
        if((collideRight && movingRight) || (collideLeft && movingLeft))
        {
            float yval = direction.y;
            //collides above
            if(collisionPoint.y > direction.y)
            {
                //speed up
                yval = direction.y * 1.5f;
                move.speed *= 1.15f;
            }
            else
            {
                move.speed *= 0.95f;

                //slow down
                //yval = direction.y * 0.8f;
            }

            direction = new Vector3(-direction.x, yval).normalized;
        }
    }
}
