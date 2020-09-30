using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioEventHandler : MonoBehaviour
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
    PlayerFireSmallBullet,
    PlayerFireMediumMissile,
    PlayerFireLargeLaser,
    PlayerHurt,
    EnemyHit,


}