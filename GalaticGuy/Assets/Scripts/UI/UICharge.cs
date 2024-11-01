﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Labels;

public class UICharge : MonoBehaviour
{
    [SerializeField]
    Slider chargebarL = null;
    [SerializeField]
    Slider chargebarR = null;

    [SerializeField]
    Image chargeBarLFilled = null;
    [SerializeField]
    Image chargeBarRFilled = null;

    private AudioEventNames filledClip = AudioEventNames.PlayerFullyCharged;

    private AudioEventNames chargingClip = AudioEventNames.PlayerStartCharge;


    Animator anim;
    bool isCharging, chargingR;

    float timeSinceLastCharge = 0f;
    float chargeDuration = 0f;

    const float MIN_CHARGE_DURATION = 0.25f;
    const float CHARGE_TIME_THRESHOLD = 0.03f;
    const float PITCH_INCREASE_RATE = 0.25f;
    const float BASE_PITCH = 1.0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        chargeBarLFilled.enabled = false;
        chargeBarRFilled.enabled = false;
        isCharging = false;
    }

    private void InitialiseCharge(float percentOfMax)
    {
        //stub
    }

    // Update is called once per frame
    public void UpdateChargeL(float percent)
    {
        chargeBarLFilled.enabled = false;
        chargeBarRFilled.enabled = false;

        chargingR = false;
        isCharging = true;
        chargebarR.value = 0f;
        chargebarL.value = percent;
    }

    public void UpdateChargeR(float percent)
    {
        chargeBarLFilled.enabled = true;

        isCharging = true;
        chargebarL.value = 1f;
        chargebarR.value = percent;

        if(chargebarR.value >= 1f && !chargeBarRFilled.enabled)
        {
            GameManager.AudioEvents.PlayAudio(filledClip);

            chargeBarRFilled.enabled = true;
        }

        if(!chargingR)
        {
            GameManager.AudioEvents.PlayAudio(filledClip);

            chargingR = true;
        }
    }

    private void Update()
    {
        chargeDuration += Time.deltaTime;

        if (isCharging)
        {
            timeSinceLastCharge = 0f;

            isCharging = false;
        }
        else
        {
            timeSinceLastCharge += Time.deltaTime;

            if (timeSinceLastCharge > CHARGE_TIME_THRESHOLD)
            {
                chargeDuration = 0f;
            }
        }

    }
}

