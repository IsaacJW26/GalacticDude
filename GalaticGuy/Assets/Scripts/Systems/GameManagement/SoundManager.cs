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
    public string semiShot = "";

    [FMODUnity.EventRef]
    public string fullShot = "";

    [FMODUnity.EventRef]
    public string bossSpawn = "";

    [FMODUnity.EventRef]
    public string semiCharged = "";

    [FMODUnity.EventRef]
    public string fullyCharged = "";

    [FMODUnity.EventRef]
    public string music = "";

    [FMODUnity.EventRef]
    public string bossMusic = "";

    [FMODUnity.EventRef]
    public string asteroidHit = "";

    [FMODUnity.EventRef]
    public string asteroidDestroyed = "";
    
    [FMODUnity.EventRef]
    public string coin = "";

    [FMODUnity.EventRef]
    public string charging = "";

    [FMODUnity.EventRef]
    public string enemyShot = "";

    [FMODUnity.EventRef]
    public string predictorHurt = "";    

    [FMODUnity.EventRef]
    public string predictorDeath = "";

    [FMODUnity.EventRef]
    public string muncherHurt = "";

    [FMODUnity.EventRef]
    public string muncherDeath = "";

    [FMODUnity.EventRef]
    public string raisinHurt = "";

    [FMODUnity.EventRef]
    public string raisinDeath = "";
    

    [SerializeField]
    private FMODUnity.StudioEventEmitter chargingEmitter;

    [SerializeField]
    private FMODUnity.StudioEventEmitter bossEmitter;

    [SerializeField]
    private FMODUnity.StudioEventEmitter defaultMusicEmitter;

    public void Initialise(AudioEventHandler audioEventHandler)
    {
        // Example use of setting an event listener
        audioEventHandler.SetListener(AudioEventNames.PlayerFireSmallBullet, PlayDefaultShot);
        audioEventHandler.SetListener(AudioEventNames.PlayerFireMediumMissile, PlaySemiShot);
        audioEventHandler.SetListener(AudioEventNames.PlayerFireLargeLaser, PlayFullShot);
        audioEventHandler.SetListener(AudioEventNames.PlayerStartCharge, StartCharging);
        audioEventHandler.SetListener(AudioEventNames.PlayerStopCharge, StopCharging);
        audioEventHandler.SetListener(AudioEventNames.PlayerFullyCharged, FullyCharged);
        audioEventHandler.SetListener(AudioEventNames.AsteroidHit, PlayAsteroidHit);
        audioEventHandler.SetListener(AudioEventNames.AsteroidDestroyed, PlayAsteroidDestroyed);
        audioEventHandler.SetListener(AudioEventNames.CoinPickup, PlayCoin);

        audioEventHandler.SetListener(AudioEventNames.EnemyShot, PlayEnemyShot);
        audioEventHandler.SetListener(AudioEventNames.EnemyHurt, PlayPredictorHurt);
        audioEventHandler.SetListener(AudioEventNames.EnemyDeath, PlayPredictorDeath);
        audioEventHandler.SetListener(AudioEventNames.MuncherHurt, PlayMuncherHurt);
        audioEventHandler.SetListener(AudioEventNames.MuncherDeath, PlayMuncherDeath);
        audioEventHandler.SetListener(AudioEventNames.RaisinHurt, PlayRaisinHurt);
        audioEventHandler.SetListener(AudioEventNames.RaisinDeath, PlayRaisinDeath);
        
        audioEventHandler.SetListener(AudioEventNames.BossEnter, StartBoss);        
        audioEventHandler.SetListener(AudioEventNames.BossDeath, StopBoss); 
        audioEventHandler.SetListener(AudioEventNames.IncreaseThreatLevel, IncreaseThreat);
        audioEventHandler.SetListener(AudioEventNames.DecreaseThreatLevel, DecreaseThreat);
    }

    [ContextMenu("DefaultShot")]
    public void PlayDefaultShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(defaultShot, transform.position);

    }

    [ContextMenu("Semi-Charged Shot")]
    public void PlaySemiShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(semiShot, transform.position);

    }

    [ContextMenu("Fully-Chraged Shot")]
    public void PlayFullShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(fullShot, transform.position);

    }
    
    [ContextMenu("Semi-Charged")]
    public void SemiCharged()
    {
        FMODUnity.RuntimeManager.PlayOneShot(semiCharged, transform.position);

    }

    [ContextMenu("Fully-Charged")]
    public void FullyCharged()
    {
        FMODUnity.RuntimeManager.PlayOneShot(fullyCharged, transform.position);

    }

    [ContextMenu("Asteroid Hit")]
    public void PlayAsteroidHit()
    {
        FMODUnity.RuntimeManager.PlayOneShot(asteroidHit, transform.position);

    }

    [ContextMenu("Asteroid Destroyed")]
    public void PlayAsteroidDestroyed()
    {
        FMODUnity.RuntimeManager.PlayOneShot(asteroidDestroyed, transform.position);

    }

    [ContextMenu("Coin")]
    public void PlayCoin()
    {
        FMODUnity.RuntimeManager.PlayOneShot(coin, transform.position);
    }

    [ContextMenu("Enemy Shot")]
    public void PlayEnemyShot()
    {
        FMODUnity.RuntimeManager.PlayOneShot(enemyShot, transform.position);
    }

    [ContextMenu("Predictor Hurt")]
    public void PlayPredictorHurt()
    {
        FMODUnity.RuntimeManager.PlayOneShot(enemyShot, transform.position);
    }

    [ContextMenu("Predictor Death")]
    public void PlayPredictorDeath()
    {
        FMODUnity.RuntimeManager.PlayOneShot(predictorDeath, transform.position);
    }


    [ContextMenu("Muncher Hurt")]
        public void PlayMuncherHurt()
        {
            FMODUnity.RuntimeManager.PlayOneShot(muncherHurt, transform.position);
        }


    [ContextMenu("Muncher Death")]
        public void PlayMuncherDeath()
        {
            FMODUnity.RuntimeManager.PlayOneShot(muncherDeath, transform.position);
        }


    [ContextMenu("Raisin Hurt")]
        public void PlayRaisinHurt()
        {
            FMODUnity.RuntimeManager.PlayOneShot(raisinHurt, transform.position);
        }


    [ContextMenu("Raisin Death")]
        public void PlayRaisinDeath()
        {
            FMODUnity.RuntimeManager.PlayOneShot(raisinDeath, transform.position);
        }


    [ContextMenu("bossSpawn")]
    public void PlayBossSpawn()
    {
        FMODUnity.RuntimeManager.PlayOneShot(bossSpawn, transform.position);
    }

    /*
    [ContextMenu("")]
    public void ()
    {
        FMODUnity.RuntimeManager.Play(, transform.position);

    }
    */

    [ContextMenu("Test music")]
    public void TestMusic()
    {
        GetComponent<FMODUnity.StudioEventEmitter>().Play();
    }

    [ContextMenu("Start Charging")]
    public void StartCharging()
    {
        if(!chargingEmitter.IsPlaying())
        {
            chargingEmitter.Play();
        }
    }

    [ContextMenu("Stop Charging")]
    public void StopCharging()
    {
        if(chargingEmitter.IsPlaying())
        {
            chargingEmitter.Stop();
        }
    }

    [ContextMenu("Start Boss Music")]
    public void StartBoss()
    {
        StopDefaultMusic();
        StartCoroutine(BossSpawnEnum()); 
    }

    IEnumerator BossSpawnEnum()
    {
        PlayBossSpawn();
        yield return new WaitForSeconds(6);
        bossEmitter.Play();
    }

    [ContextMenu("Stop Boss Music")]
    public void StopBoss()
    {
        Debug.Log("Boss Music Stopped");
        bossEmitter.Stop();
        StartDefaultMusic();
    }

    int threatlevel = 0;
    float threatLevelInterpolated = 0f;
    [ContextMenu("increase threat")]
    public void IncreaseThreat()
    {
        threatlevel = (threatlevel >= 4) ? 5 : threatlevel + 1;

        //Debug.Log("Sound threat: " + threatlevel);
    }

    public void DecreaseThreat()
    {
        threatlevel = (threatlevel <= 1) ? 0 : threatlevel - 1;

        //Debug.Log("Sound threat: " + threatlevel);
    }

    [ContextMenu("Start Default Music")]
    public void StartDefaultMusic()
    {
        defaultMusicEmitter.Play();
    }

    [ContextMenu("Stop Default Music")]
    public void StopDefaultMusic()
    {
        Debug.Log("Default Music stopped");
        defaultMusicEmitter.Stop();
    }

    void FixedUpdate()
    {
        //threat level
        threatLevelInterpolated = Mathf.Lerp(threatLevelInterpolated, threatlevel, 0.01f);
        defaultMusicEmitter.SetParameter("Threat", threatLevelInterpolated);
    }
}