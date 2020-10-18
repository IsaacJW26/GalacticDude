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
    public float speed = 2.5f;
    private float currentSpeed;
    private int timeTillSpeedReset;
    private const int speedreset = 5;

    public const float xBound = 4f;
    float xdirection = 0, ydirection = 0;
    bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<CharacterAnimator>();
        currentSpeed = speed;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            float actualX = 0;
            //outside of bound but moving to centre
            if ((transform.position.x > xBound && xdirection <= 0) || (transform.position.x < -xBound && xdirection >= 0))
            {
                actualX = xdirection;
            }
            //or within bounds
            else if ((transform.position.x < xBound) && (transform.position.x > -xBound))
            {
                actualX = xdirection;
            }

            var velocity = new Vector3(actualX, ydirection).normalized * currentSpeed;
            //move in animator
            anim?.Move(velocity);
            Move(velocity, Time.fixedDeltaTime);

            //if speed is modified
            if (timeTillSpeedReset > 0)
            {
                float perc = (float)timeTillSpeedReset / (float)speedreset;
                //
                if (Mathf.Abs(currentSpeed - speed) < 0.01f)
                    currentSpeed = Mathf.Lerp(speed, currentSpeed, perc);

                timeTillSpeedReset--;
            }
            else
            {
                if (Mathf.Abs(currentSpeed - speed) > 0.01f)
                    currentSpeed = speed;
                else
                    currentSpeed = Mathf.Lerp(speed, currentSpeed, 0.5f);
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
        currentSpeed = speed * slowPercent;
        timeTillSpeedReset = speedreset;
        //Debug.Log($"original {speed} perc {slowPercent} new {currentSpeed} ");
    }

    void ISpeed.SlowDown(float slowPercent, int duration)
    {
        currentSpeed = speed * slowPercent;
        timeTillSpeedReset = duration;
        //Debug.Log($"original {speed} perc {slowPercent} new {currentSpeed} ");
    }

    void ISpeed.SpeedUp(float speedupPercent)
    {
        currentSpeed = speed * speedupPercent;
        timeTillSpeedReset = speedreset;
    }

    void ISpeed.SpeedUp(float speedupPercent, int duration)
    {
        currentSpeed = speed * speedupPercent;
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