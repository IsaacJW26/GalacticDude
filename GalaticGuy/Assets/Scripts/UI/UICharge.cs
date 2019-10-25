using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Labels;

public class UICharge : MonoBehaviour
{
    [SerializeField]
    Slider chargebarL;
    [SerializeField]
    Slider chargebarR;
    Animator anim;
    bool chargingL, chargingR;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void InitialiseCharge(float percentOfMax)
    {
        //stub
    }

    // Update is called once per frame
    public void UpdateChargeL(float percent)
    {
        chargingL = true;
        chargingR = true;
        chargebarR.value = 0f;
        chargebarL.value = percent;
    }

    public void UpdateChargeR(float percent)
    {
        chargingL = false;
        chargingR = true;
        chargebarL.value = 1f;
        chargebarR.value = percent;
    }

    private void FixedUpdate()
    {
        //anim.SetBool(AnimProperties.CHARGING, chargingL);
        chargingL = false;
    }
}
