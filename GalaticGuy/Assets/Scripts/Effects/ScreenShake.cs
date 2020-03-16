using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class ScreenShake : MonoBehaviour
{
    /*
    [SerializeField]
    float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    [SerializeField]
    float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    [SerializeField]
    float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter
    */

    private float ShakeTimeRemaining = 0f;

    // Cinemachine Shake
    private CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    // Use this for initialization
    void Awake()
    {
        VirtualCamera = GetComponent<CinemachineVirtualCamera>();
        // Get Virtual Camera Noise Profile
        if (VirtualCamera != null)
            virtualCameraNoise = VirtualCamera.GetCinemachineComponent<Cinemachine.CinemachineBasicMultiChannelPerlin>();

        EndShake();
    }

    // Update is called once per frame
    void Update()
    {
        // If the Cinemachine componet is not set, avoid update
        if (VirtualCamera != null && virtualCameraNoise != null)
        {
            // If Camera Shake effect is still playing
            if (ShakeTimeRemaining > 0)
            {
                ShakeDecay();
                // Update Shake Timer
                ShakeTimeRemaining -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                EndShake();
            }
        }
    }

    private void ShakeDecay()
    {
        virtualCameraNoise.m_AmplitudeGain *= 0.95f;
        virtualCameraNoise.m_FrequencyGain *= 0.95f;
    }


    private void StartShake(float frequency, float amplitude, float duration)
    {
        if (ShakeTimeRemaining > 0f)
            ShakeTimeRemaining = duration / 4f;
        else
            ShakeTimeRemaining = duration;
        // Set Cinemachine Camera Noise parameters
        virtualCameraNoise.m_AmplitudeGain += amplitude;

        if(virtualCameraNoise.m_FrequencyGain < 1f)
            virtualCameraNoise.m_FrequencyGain += frequency;
        else
            virtualCameraNoise.m_FrequencyGain += (frequency / virtualCameraNoise.m_FrequencyGain);

    }

    private void EndShake()
    {
        // Set Cinemachine Camera Noise parameters
        virtualCameraNoise.m_AmplitudeGain = 0f;
        virtualCameraNoise.m_FrequencyGain = 0f;
    }

    [ContextMenu("Big Shake")]
    public void BigShake()
    {
        StartShake(frequency: 3.5f, amplitude: 10f, duration: 0.65f);
    }

    [ContextMenu("Medium Shake")]
    public void MediumShake()
    {
        StartShake(1.2f, 3f, 0.4f);
    }

    [ContextMenu("Small Shake")]
    public void SmallShake()
    {
        StartShake(0.8f, 1.5f, 0.2f);
    }
}
