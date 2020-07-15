using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIDown : EnemyAI
{
    [Header("Outer Shield")]
    [SerializeField]
    Shield shieldOuterPrefab;
    [SerializeField]
    Vector3 outerRelativePosition;

    [Header("Inner Shield")]
    [SerializeField]
    Shield shieldInnerPrefab;
    [SerializeField]
    Vector3 innerRelativePosition;

    private Shield innerShieldObj;
    private Shield outerShieldObj;

    public override void Initialise(Movement movement, MainCharacter player, CharacterShoot shoot)
    {
        base.Initialise(movement, player, shoot);
        var rb = GetComponent<Rigidbody2D>();
        if(shieldInnerPrefab != null)
            {
            innerShieldObj = Instantiate(shieldInnerPrefab, innerRelativePosition, Quaternion.identity) as Shield;
            innerShieldObj.Initialise(rb);
            innerShieldObj.gameObject.SetActive(false);

        }

        if (shieldOuterPrefab != null)
        {
            outerShieldObj = Instantiate(shieldOuterPrefab, outerRelativePosition, Quaternion.identity) as Shield;
            outerShieldObj.Initialise(rb);
            shieldOuterPrefab.gameObject.SetActive(false);

        }
    }

    private void OnEnable()
    {
        innerShieldObj.gameObject.SetActive(true);
        outerShieldObj.gameObject.SetActive(true);
    }

    //tries to shoot and move every frame
    public override void UpdateFrame(Vector3 currentPosition)
    {
        Move(Vector3.down);
    }

    public override void OnDeath()
    {
        Debug.Log("death");
        
        if (innerShieldObj != null)
            Destroy(innerShieldObj);

        if (outerShieldObj != null)
            Destroy(outerShieldObj);
    }
}
