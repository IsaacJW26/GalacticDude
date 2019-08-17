using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    CharacterMovement move;
    CharacterShoot shoot;
    CharacterHealth health;

    int chargeMeter;
    //50 seconds to charge (60 x 50)
    const int CHARGEMETER_MAX = 3000;

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
        shoot = GetComponent<CharacterShoot>();
        chargeMeter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //input direction x
        move.InputDirectionX(Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)));

    }

    private void FixedUpdate()
    {
        if (IsInput())
        {
            if(chargeMeter < CHARGEMETER_MAX)
                shoot.TryShoot();
        }
        else
        {
            //increment charge when not moving
            chargeMeter++;
        }
    }

    public bool IsInput()
    {
        return Mathf.RoundToInt(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS)) != 0;
    }
}
