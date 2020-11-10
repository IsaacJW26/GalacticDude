using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventHandler : MonoBehaviour, IAudio
{
    public delegate void OnAudioEvent();

    private Dictionary<AudioEventNames, OnAudioEvent> audioEvents = null;

    // Start is called before the first frame update
    public void Initialise()
    {
        audioEvents = new Dictionary<AudioEventNames, OnAudioEvent>();
    }

    // Update is called once per frame
    public void SetListener(AudioEventNames eventId, OnAudioEvent observer)
    {
        try
        {
            audioEvents.Add(eventId, observer);
        }
        catch (ArgumentException)
        {
            Debug.LogWarning($"audio event \"{eventId}\" already exists");
        }
    }

    public void PlayAudio(AudioEventNames eventId)
    {
        Debug.Log("Event being played " +eventId);

        if(eventId == AudioEventNames.NONE)
            return;

        try
        {
            audioEvents[eventId].Invoke();
        }
        catch (KeyNotFoundException)
        {
            Debug.LogWarning($"audio event \"{eventId}\" is not set");
        }
    }
}

public enum AudioEventNames
{
    // Player
    PlayerFireSmallBullet = 0,
    PlayerFireMediumMissile = 1,
    PlayerFireLargeLaser = 2,
    PlayerHurt = 3,
    PlayerStartCharge = 4,
    PlayerStopCharge = 5,
    PlayerFullyCharged = 6,
    
    // Enemy
    EnemyHurt = 7,
    EnemyShot = 8,
    EnemyDeath = 9,
    
    // Boss
    BossEnter = 10,
    BossHurt = 11,
    BossCharge = 12,
    BossFullCharge = 13,
    BossAttack = 14,
    BossDeath = 15,
    
    // Asteroid
    AsteroidHit = 16, 
    AsteroidDestroyed = 17, 
    AsteroidMoving = 18, 
    
    // Currency
    CoinPickup = 19,

    // UI
    UiSelect = 20,
    UiNavigate = 21,
    
    // Misc
    NONE = 22,

    // Change values
    IncreaseThreatLevel = 23,
    DecreaseThreatLevel = 24,
    ResetThreatLevel = 25,

    // Muncher
    MuncherHurt = 26,
    MuncherDeath = 28,

    // Raisin
    RaisinHurt = 29,
    RaisinDeath = 30,
    //RaisinShot = 31, WE'RE USING ENEMY SHOT

}

public interface IAudio
{
    void PlayAudio(AudioEventNames eventId);
};