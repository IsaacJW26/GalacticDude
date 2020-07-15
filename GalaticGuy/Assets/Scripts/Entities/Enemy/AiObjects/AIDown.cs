using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDown : EnemyAI
{
    [Header("Outer Shield")]
    [SerializeField]
    Projectile shieldOuterPrefab;
    [SerializeField]
    Vector3 outerRelativePosition;

    [Header("Inner Shield")]
    [SerializeField]
    Projectile shieldInnerPrefab;
    [SerializeField]
    Vector3 innerRelativePosition;

    private Projectile innerShieldObj;
    private Projectile outerShieldObj;

    public override void Initialise(Movement movement, MainCharacter player, CharacterShoot shoot)
    {
        base.Initialise(movement, player, shoot);

        if (shieldInnerPrefab == null)
            Debug.Log($"{gameObject.name} has no prefab");
        innerShieldObj = Instantiate(shieldInnerPrefab, innerRelativePosition, Quaternion.identity);
        outerShieldObj = Instantiate(shieldOuterPrefab, outerRelativePosition, Quaternion.identity);
    }

    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        Move(Vector3.down);
    }

    public override void OnDeath()
    {
        if (innerShieldObj != null)
            Destroy(innerShieldObj);

        if (outerShieldObj != null)
            Destroy(outerShieldObj);
    }
}
