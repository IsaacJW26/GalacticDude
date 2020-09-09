using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField]
    private string MusicStateEvent = "";

    private FMOD.Studio.EventInstance gameMusicState;

    [FMODUnity.EventRef]
    public string defaultShot = "";

    [FMODUnity.EventRef]
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

    public void OnBossEnter()
    {

    }

    public void OnBossDeath()
    {

    }
}