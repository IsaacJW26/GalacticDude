using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiralProjectile : Projectile
{
    bool wayBack;
    Vector3 targetPosition;
    int framesSinceArrival;

    [SerializeField]
    float distanceFromPlayer = 6f;

    [SerializeField]
    float velocityY = 1f;

    [SerializeField]
    float spiralSpeed = 1f;
    static bool posDir = true;
    float sign = 1f;

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        /*
         * pos.x = r * sin (t)
         * pox.y = r * sin (t)
         */
        var period = 2f * Mathf.PI;
        var radius = distanceFromPlayer / 2.0f;
        //move in circular motion
        velocity.x = Mathf.Cos(TimeSinceArrival() * period * spiralSpeed)* sign;
        velocity.y = Mathf.Abs(Mathf.Sin((TimeSinceArrival() * spiralSpeed + 0.5f) * period)) + velocityY;
        velocity *= radius * spiralSpeed;
        //
        base.FixedUpdate();
        framesSinceArrival++;
    }

    private float TimeSinceArrival()
    {
        return ((float)framesSinceArrival / 60f);
    }

    public override void OnInitialise()
    {
        framesSinceArrival = 0;
        targetPosition = new Vector3(transform.position.x, transform.position.y + distanceFromPlayer);
        if (!posDir)
            sign = -1f;
        posDir = !posDir;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void OnDamage(int inDamage)
    {
        //do nothing
    }
}
