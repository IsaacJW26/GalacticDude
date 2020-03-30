using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // keep pool of audio objects
    // allows a 
    AudioSource[] audioPool;
    const int maxPool = 15;

    int currentIdx;

    void Awake()
    {
        audioPool = new AudioSource[maxPool];
        for (int i = 0; i < audioPool.Length; i++)
        {
            audioPool[i] = gameObject.AddComponent<AudioSource>();
            audioPool[i].playOnAwake = false;
            audioPool[i].loop = false;
            audioPool[i].spatialBlend = 0.0f;
        }

        currentIdx = 0;
    }

    void Update()
    {
        
    }

    public AudioSource CreateAudio(AudioClip clip)
    {
        currentIdx = (currentIdx + 1) % maxPool;
        
        audioPool[currentIdx].clip = clip;
        audioPool[currentIdx].Play();

        return audioPool[currentIdx];
    }
}
