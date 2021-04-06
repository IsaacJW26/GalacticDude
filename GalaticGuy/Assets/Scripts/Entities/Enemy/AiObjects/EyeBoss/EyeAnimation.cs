using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    const int DurationBetweenFranticEyeTrack = 5;
    Quaternion eyeTargetRotation;
    Quaternion startRotationFranticEye;
    int maxDuration = DurationBetweenFranticEyeTrack;
    // Start is called before the first frame update
    void Start()
    {
        currentState = AnimationState.Wandering;
        rb3d = GetComponentInChildren<Rigidbody>();
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
                var directionToPlayerPosition = (GameManager.INST.GetPlayerPos() - transform.position + (Vector3.back * 8f)).normalized;
                rb3d.rotation = Quaternion.LookRotation(-directionToPlayerPosition, Vector3.down);
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
}
