using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{
    CharacterMovement move;
    

    // Start is called before the first frame update
    void Start()
    {
        move = GetComponent<CharacterMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //input direction x
        move.InputDirectionX(Mathf.RoundToInt(Input.GetAxis("Horizontal")));
    }
}
