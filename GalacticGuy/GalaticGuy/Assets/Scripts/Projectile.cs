using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    int maxLifeTime = 300;
    int timeSinceBirth = 0;
    int index;

    Rigidbody2D rb;
    Vector2 velocity;
    CharacterShoot parent;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
        timeSinceBirth++;
        if (timeSinceBirth > maxLifeTime)
            parent.DisableProjectile(index);
    }

    public void Initialise(int index, Vector2 velocity, CharacterShoot parent)
    {
        this.index = index;
        this.velocity = velocity;
        this.parent = parent;
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
        timeSinceBirth = 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //todo
    }
}
