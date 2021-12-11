using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDown : EnemyAI
{
    [Header("Outer Shield")]
    [SerializeField]
    Shield shieldOuterPrefab = null;
    [SerializeField]
    Vector3 outerRelativePosition = Vector3.zero;

    [Header("Inner Shield")]
    [SerializeField]
    Shield shieldInnerPrefab = null;
    [SerializeField]
    Vector3 innerRelativePosition = Vector3.zero;

    private Shield innerShieldObj;
    private Shield outerShieldObj;

    public override void Initialise(Movement movement, MainCharacter player, IShooter shoot)
    {
        base.Initialise(movement, player, shoot);
        var rb = GetComponent<Rigidbody2D>();
        if(shieldInnerPrefab != null)
            {
            innerShieldObj = Instantiate(shieldInnerPrefab,transform.position + innerRelativePosition, Quaternion.identity) as Shield;
            innerShieldObj.Initialise(rb, innerRelativePosition);
            innerShieldObj.gameObject.SetActive(false);

        }

        if (shieldOuterPrefab != null)
        {
            outerShieldObj = Instantiate(shieldOuterPrefab, transform.position + outerRelativePosition, Quaternion.identity) as Shield;
            outerShieldObj.Initialise(rb, outerRelativePosition);
            shieldOuterPrefab.gameObject.SetActive(false);

        }
    }

    private void OnEnable()
    {
        innerShieldObj?.gameObject.SetActive(true);
        outerShieldObj?.gameObject.SetActive(true);
    }

    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        Move(Vector3.down);
    }

    public override void OnDeath()
    {
        if (innerShieldObj != null)
            innerShieldObj.OnDeath();

        if (outerShieldObj != null)
            outerShieldObj.OnDeath();
    }
}
