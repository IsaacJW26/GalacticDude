using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField]
    float speed = 2.5f;
    float xBound = 4f;
    int xdirection = 0, ydirection = 0;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
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
        rb.velocity = new Vector3(actualX, ydirection) * speed;
    }

    public void InputDirectionX(int value)
    {
        xdirection = Mathf.Clamp(value, -1, 1);
    }

    public void InputDirectionY(int value)
    {
        ydirection = Mathf.Clamp(value, -1, 1);
    }
}
