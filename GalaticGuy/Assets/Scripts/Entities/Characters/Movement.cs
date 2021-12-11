using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterAnimator))]
public class Movement : MonoBehaviour, ISpeed
{
    ICharacterAnimator anim;
    protected Rigidbody2D rb;
    [SerializeField]
    [Range(0.01f, 5f)]
    public float baseSpeed = 2.5f;
    private float currentSpeed;
    private int timeTillSpeedReset;
    private const int speedreset = 5;

    public const float xBound = 4f;
    public const float yBound = 7f;

    float xdirection = 0, ydirection = 0;
    bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<CharacterAnimator>();
        currentSpeed = baseSpeed;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            float actualX = xdirection;

            // when outside right bound, dont move right
            if (transform.position.x > xBound && xdirection >= 0f)
            {
                actualX = 0f;
            }
            // when outside left bound, dont move left
            else if(transform.position.x < -xBound && xdirection <= 0f)
            {
                actualX = 0f;
            }

            float actualY = ydirection;

            // when outside right bound, dont move right
            if (transform.position.y > yBound && ydirection >= 0f)
            {
                actualY = 0f;
            }
            // when outside left bound, dont move left
            else if(transform.position.y < -yBound && ydirection <= 0f)
            {
                actualY = 0f;
            }

            var velocity = new Vector3(actualX, actualY).normalized * currentSpeed;

            //move in animator
            anim?.Move(velocity);
            Move(velocity, Time.fixedDeltaTime);

            //if speed is modified
            if (timeTillSpeedReset > 0)
            {
                float perc = (float)timeTillSpeedReset / (float)speedreset;
                //
                if (Mathf.Abs(currentSpeed - baseSpeed) < 0.01f)
                    currentSpeed = Mathf.Lerp(baseSpeed, currentSpeed, perc);

                timeTillSpeedReset--;
            }
            else
            {
                if (Mathf.Abs(currentSpeed - baseSpeed) > 0.01f)
                    currentSpeed = baseSpeed;
                else
                    currentSpeed = Mathf.Lerp(baseSpeed, currentSpeed, 0.5f);
            }
        }
    }

    public void OnDeath()
    {
        isDead = true;
    }

    protected virtual void Move(Vector2 velocity, float deltaTime)
    {
        rb.velocity = velocity;
    }

    public void InputDirectionX(float value)
    {
        xdirection = Mathf.Clamp(value, -1f, 1f);
    }

    public void InputDirectionY(float value)
    {
        ydirection = Mathf.Clamp(value, -1f, 1f);
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = true;
        }
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.isKinematic = false;
        }
    }

    void ISpeed.SlowDown(float slowPercent)
    {
        currentSpeed = baseSpeed * slowPercent;
        timeTillSpeedReset = speedreset;
        //Debug.Log($"original {speed} perc {slowPercent} new {currentSpeed} ");
    }

    void ISpeed.SlowDown(float slowPercent, int duration)
    {
        currentSpeed = baseSpeed * slowPercent;
        timeTillSpeedReset = duration;
        //Debug.Log($"original {speed} perc {slowPercent} new {currentSpeed} ");
    }

    void ISpeed.SpeedUp(float speedupPercent)
    {
        currentSpeed = baseSpeed * speedupPercent;
        timeTillSpeedReset = speedreset;
    }

    void ISpeed.SpeedUp(float speedupPercent, int duration)
    {
        currentSpeed = baseSpeed * speedupPercent;
        timeTillSpeedReset = duration;
    }
}

public interface ISpeed
{
    void SlowDown(float slowPercent);
    void SlowDown(float slowPercent, int duration);

    void SpeedUp(float speedupPercent);
    void SpeedUp(float speedupPercent, int duration);
}