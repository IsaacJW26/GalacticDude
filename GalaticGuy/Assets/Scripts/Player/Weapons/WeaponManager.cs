using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public const int MAX_CHARGE = 1800;

    [SerializeField]
    Weapon current;
    [SerializeField]
    Transform projectilePointDefault;
    bool held = false;
    
    // Start is called before the first frame update
    void Start()
    {
        current.Awake();
        current.SetMovement(GetComponent<CharacterMovement>());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        current.FixedUpdate();
        if(held)
        {
            current.OnShootButtonHold();
        }
    }

    public void OnShootButtonDown()
    {
        held = true;

        current.OnShootButtonDown();
    }

    public void OnShootButtonHold()
    {
        //current.OnShootButtonHold();
    }

    public void OnShootButtonRelease()
    {
        held = false;

        current.OnShootButtonRelease(this);
    }

    public Vector2 GetPlayerDirection()
    {
        return Vector2.up;
    }

    public Vector3 GetLaunchPosition()
    {
        return projectilePointDefault.position;
    }
}
