using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour, ISpeed
{
    Rigidbody2D rb;
    [SerializeField]
    public float speed = 2.5f;
    private float currentSpeed;
    private int timeTillSpeedReset;
    private const int speedreset = 5;

    public const float xBound = 4f;
    int xdirection = 0, ydirection = 0;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        currentSpeed = speed;
    }

    void FixedUpdate()
    {
        int actualX = 0;
        //outside of bound but moving to centre
        if ((transform.position.x > xBound && xdirection <= 0) || (transform.position.x < -xBound && xdirection >= 0))
        {
            actualX = xdirection;
        }
        //within bounds
        else if ((transform.position.x < xBound) && (transform.position.x > -xBound))
        {
            actualX = xdirection;
        }
        rb.velocity = new Vector3(actualX, ydirection) * currentSpeed;

        if (timeTillSpeedReset > 0)
        {
            float perc = timeTillSpeedReset / (float)speedreset;
            currentSpeed = Mathf.Lerp(speed, currentSpeed, perc);

            timeTillSpeedReset--;
        }
        else
        {
            currentSpeed = speed;
        }
    }

    public void InputDirectionX(int value)
    {
        xdirection = Mathf.Clamp(value, -1, 1);
    }

    public void InputDirectionY(int value)
    {
        ydirection = Mathf.Clamp(value, -1, 1);
    }

    public void SlowDown(float slowPercent)
    {
        currentSpeed = speed * slowPercent;
        timeTillSpeedReset = speedreset;
    }

    public void SlowDown(float slowPercent, int duration)
    {
        currentSpeed = speed * slowPercent;
        timeTillSpeedReset = duration;
    }


    public void SpeedUp(float speedupPercent)
    {
        currentSpeed = speed * speedupPercent;
        timeTillSpeedReset = speedreset;
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
}

public interface ISpeed
{
    void SlowDown(float slowPercent);
    void SlowDown(float slowPercent, int duration);

    void SpeedUp(float speedupPercent);
}