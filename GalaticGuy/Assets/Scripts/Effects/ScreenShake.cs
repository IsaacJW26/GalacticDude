using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;

public class ScreenShake : MonoBehaviour
{
    public static ScreenShake INSTANCE = null;

    /*
    [SerializeField]
    float ShakeDuration = 0.3f;          // Time the Camera Shake effect will last
    [SerializeField]
    float ShakeAmplitude = 1.2f;         // Cinemachine Noise Profile Parameter
    [SerializeField]
    float ShakeFrequency = 2.0f;         // Cinemachine Noise Profile Parameter
    */

    private float ShakeElapsedTime = 0f;

    // Cinemachine Shake
    private CinemachineVirtualCamera VirtualCamera;
    private CinemachineBasicMultiChannelPerlin virtualCameraNoise;

    // Use this for initialization
    void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(this);

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
            if (ShakeElapsedTime > 0)
            {
                ShakeDecay();
                // Update Shake Timer
                ShakeElapsedTime -= Time.deltaTime;
            }
            else
            {
                // If Camera Shake effect is over, reset variables
                EndShake();
                //ShakeElapsedTime = 0f;
            }
        }
    }

    private void ShakeDecay()
    {
        virtualCameraNoise.m_AmplitudeGain *= 0.90f;
        virtualCameraNoise.m_FrequencyGain *= 0.90f;
    }


    private void StartShake(float frequency, float amplitude, float duration)
    {
        ShakeElapsedTime = duration;
        // Set Cinemachine Camera Noise parameters
        virtualCameraNoise.m_AmplitudeGain += amplitude;
        virtualCameraNoise.m_FrequencyGain += frequency;
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
        StartShake(2.0f, 4f, 0.5f);
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
