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

    }

    public void OnBossDeath()
    {

    }
}