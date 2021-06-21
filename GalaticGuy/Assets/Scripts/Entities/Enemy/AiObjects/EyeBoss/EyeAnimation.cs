using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools.Maths;

public class EyeAnimation : MonoBehaviour
{
    private enum AnimationState
    {
        Wandering = 0,
        Frantic = 1,
        Tracking = 2,
    } 

    private AnimationState currentState;
    private Rigidbody rb3d;
    int timeSinceLastCheck = 0;
    const int DurationBetweenFranticEyeTrack = 3;
    Quaternion eyeTargetRotation;
    Quaternion startRotationFranticEye;
    int maxDuration = DurationBetweenFranticEyeTrack;

    [SerializeField]
    ParticleSystem chargeParticles = null;
    ParticleSystem.EmissionModule emissionModule;
    float defaultEmission;
    Vector3 defaultSize;

    public delegate Vector3 DirectionToPlayer();

    public DirectionToPlayer LookDirection;


    // Start is called before the first frame update
    void Start()
    {
        currentState = AnimationState.Wandering;
        rb3d = GetComponentInChildren<Rigidbody>();

        emissionModule = chargeParticles.emission;
        defaultSize = chargeParticles.transform.localScale;
        defaultEmission = emissionModule.rateOverTime.Evaluate(0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        switch(currentState)
        {
            case AnimationState.Wandering:

                break;

            case AnimationState.Frantic:
                if(timeSinceLastCheck <= 0)
                {
                    timeSinceLastCheck = maxDuration = Mathf.RoundToInt(Random.Range(0.8f, 1.2f) * (float)DurationBetweenFranticEyeTrack);
                    startRotationFranticEye = transform.rotation;
                    
                    eyeTargetRotation = Quaternion.Euler(new Vector3(
                        Random.Range(-60f,60f),
                        Random.Range(-60f,60f),
                        Random.Range(-60f,60f))
                    );
                }
                else
                {
                    float percent = 1f - (timeSinceLastCheck / ((float)maxDuration));
                    rb3d.rotation = Quaternion.Slerp(startRotationFranticEye, eyeTargetRotation, percent); 
                    timeSinceLastCheck--;
                }
                break;

            case AnimationState.Tracking:
                var offset = (Vector3.back);
                var directionToPlayerPosition = (LookDirection() + offset).normalized;

                rb3d.rotation = Quaternion.LookRotation(-directionToPlayerPosition, Vector3.up);
                break;
        }
    }
    Vector3 m_direction;
    private void OnDrawGizmos() {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, m_direction);
    }

    public void StartWandering()
    {
        if(currentState == AnimationState.Tracking)
        {
            currentState = AnimationState.Wandering;
            rb3d.isKinematic = false;
            emissionModule.enabled = false;
        }
    }

    public void StartFrantic()
    {
        if(currentState == AnimationState.Wandering)
        {
            currentState = AnimationState.Frantic;
            rb3d.isKinematic = true;
        }
    }

    public void StartTracking()
    {
        if(currentState == AnimationState.Frantic)
        {
            currentState = AnimationState.Tracking;
        }
    }

    public void ChargeUpdate(float emissionPercent)
    {

        if(emissionPercent <= 0f)
        {
            emissionModule.enabled = false;
        }
        else if(!emissionModule.enabled)
        {
            emissionModule.enabled = true;
        }

        // move particles to be in front of eye
        const float OFFSET = 1.85f;
        chargeParticles.transform.position = transform.position +
            (LookDirection() * OFFSET);

        // scale player effects from 35% TO 100%
        rfloat scaleRange = new rfloat(0.35f, 1.0f);
        chargeParticles.transform.localScale = scaleRange.LerpValue(emissionPercent) * defaultSize;

        // scale emission rate from 30% TO 100%
        rfloat rateRange = new rfloat(0.3f, 1.0f);
        ParticleSystem.MinMaxCurve rate = emissionModule.rateOverTime;
        rate.constant = defaultEmission * rateRange.LerpValue(emissionPercent);

        emissionModule.rateOverTime = rate;
    }
}
