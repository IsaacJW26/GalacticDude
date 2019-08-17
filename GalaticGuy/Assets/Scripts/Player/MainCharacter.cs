using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    CharacterMovement move;
    CharacterShoot shoot;
    CharacterHealth health;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        shoot = GetComponent<CharacterShoot>();
    }

    // Update is called once per frame
    void Update()
    {
        //input direction x
        move.InputDirectionX(Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)));

        if (IsInput())
            shoot.TryShoot();
    }

    public bool IsInput()
    {
        return Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)) != 0;
    }
}
