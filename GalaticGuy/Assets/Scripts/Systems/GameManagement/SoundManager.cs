using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField]

    private FMOD.Studio.EventInstance gameMusicState;

    [FMODUnity.EventRef]
    public string oneShot1 = "";

    [FMODUnity.EventRef]
    public string oneShot2 = "";

    [FMODUnity.EventRef]
    public string oneShot3 = "";

    [FMODUnity.EventRef]
    public string oneShot4 = "";

    [FMODUnity.EventRef]
    public string oneShot5 = "";


    [FMODUnity.EventRef]
    public string music = "";

    [FMODUnity.EventRef]
    public string charging = "";
    
    [SerializeField]
    private FMODUnity.StudioEventEmitter chargingEmitter;

    public void Initialise(AudioEventHandler audioEventHandler)
    {
        // Example use of setting an event listener
        audioEventHandler.SetListener(AudioEventNames.PlayerFireSmallBullet, TestOneShot1);
        audioEventHandler.SetListener(AudioEventNames.PlayerFireMediumMissile, TestOneShot2);
        audioEventHandler.SetListener(AudioEventNames.PlayerFireLargeLaser, TestOneShot3);
        audioEventHandler.SetListener(AudioEventNames.PlayerStartCharge, StartCharging);
        audioEventHandler.SetListener(AudioEventNames.PlayerStopCharge, StopCharging);
        audioEventHandler.SetListener(AudioEventNames.PlayerFullyCharged, TestOneShot4);
    }

    [ContextMenu("Test oneShot1")]
    public void TestOneShot1()
    {
        FMODUnity.RuntimeManager.PlayOneShot(oneShot1, transform.position);

    }

    [ContextMenu("Test oneShot2")]
    public void TestOneShot2()
    {
        FMODUnity.RuntimeManager.PlayOneShot(oneShot2, transform.position);

    }

    [ContextMenu("Test oneShot3")]
    public void TestOneShot3()
    {
        FMODUnity.RuntimeManager.PlayOneShot(oneShot3, transform.position);

    }

    [ContextMenu("Test oneShot4")]
    public void TestOneShot4()
    {
        FMODUnity.RuntimeManager.PlayOneShot(oneShot4, transform.position);

    }

    [ContextMenu("Test oneShot5")]
    public void TestOneShot5()
    {
        FMODUnity.RuntimeManager.PlayOneShot(oneShot5, transform.position);

    }

    [ContextMenu("Test music")]
    public void TestMusic()
    {
        GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    [ContextMenu("Start Charging")]
    public void StartCharging()
    {
        chargingEmitter.Play();
    }

    [ContextMenu("Stop Charging")]
    public void StopCharging()
    {
        Debug.Log("WWWWWWWWWWW");
        chargingEmitter.Stop();
    }


    public void OnBossEnter()
    {
        //FMODUnity.RuntimeManager.PlayOneShot(defaultShot, transform.position);       
    }

    public void OnBossDeath()
    {

    }
}