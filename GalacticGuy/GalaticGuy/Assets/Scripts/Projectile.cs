using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    int index;
    Rigidbody2D rb;
    Vector2 velocity;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
    }

    public void Initialise(int index, Vector2 velocity)
    {
        this.index = index;
        this.velocity = velocity;
    }

    public void Activate(Vector3 position, Vector2 velocity)
    {
        Activate(position);
        this.velocity = velocity;
    }

    public void Activate(Vector3 position)
    {
        gameObject.SetActive(this);
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //todo
    }
}
