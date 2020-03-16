using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomerangProjectile : Projectile
{
    bool wayBack;
    Vector3 targetPosition;
    int framesSinceArrival;

    [SerializeField]
    float distanceFromPlayer = 6f;

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
        velocity.x = Mathf.Cos(TimeSinceArrival() * period);
        velocity.y = -Mathf.Sin((TimeSinceArrival() + 0.5f) * period);
        velocity *= radius;
        //
        base.FixedUpdate();
        framesSinceArrival++;
    }

    private float TimeSinceArrival()
    {
        return ((float)framesSinceArrival / 60f);
    }

    public void Return()
    {

    }

    public override void OnInitialise()
    {
        framesSinceArrival = 0;
        targetPosition = new Vector3(transform.position.x, transform.position.y + distanceFromPlayer);
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
