using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [FMODUnity.EventRef]
    [SerializeField]
    private string MusicStateEvent = "";

    private FMOD.Studio.EventInstance gameMusicState;

    public void Initialise(AudioEventHandler audioEventHandler)
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