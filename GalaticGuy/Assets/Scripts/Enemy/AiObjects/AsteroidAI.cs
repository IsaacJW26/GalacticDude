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

    private void Awake()
    {
        rotationAxis = GetRandomAxis();

        float xVal = Random.Range(-3f, 3f);

        direction = new Vector2(xVal, -1f).normalized;

        float scale = Random.Range(0.8f, 1.2f);
        transform.localScale *= scale;
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
        Move(direction);
        model.RotateAround(model.position ,rotationAxis, rotationSpeed * Time.fixedDeltaTime);
    }
}
