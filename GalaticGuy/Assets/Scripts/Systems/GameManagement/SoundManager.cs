using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField]

    private FMOD.Studio.EventInstance gameMusicState;

    [FMODUnity.EventRef]
    public string defaultShot = "";

    [FMODUnity.EventRef]
    public string music = "";

    public void Initialise(AudioEventHandler audioEventHandler)
    {
        // Example use of setting an event listener
        audioEventHandler.SetListener(AudioEventNames.PlayerFireSmallBullet, OnBossEnter);
    }

    [ContextMenu("Test oneShot")]
    public void TestDefaultShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(defaultShot, transform.position);

    }

    [ContextMenu("Test music")]
    public void TestMusic()
    {
        GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

/*    [FMODUnity.EventRef]
    public string chargingShot = "";

    [FMODUnity.EventRef]
    public string semiChargedShot = "";

    [FMODUnity.EventRef]
    public string fullyChargedShot = "";

    void Initialise()
    {
        gameMusicState = FMODUnity.RuntimeManager.CreateInstance(MusicStateEvent);
        gameMusicState.start();
    }
*/
    public void OnBossEnter()
    {
         FMODUnity.RuntimeManager.PlayOneShot(defaultShot, transform.position);       
    }

    public void OnBossDeath()
    {

    }
}