﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemySpawner))]
public class GameManager : MonoBehaviour
{
    public static GameManager INST = null;
    EnemySpawner spawner;
    GameState gameState = GameState.playing;
    IEnumerator waitingFunction = null;

    const float endOfLevelDelay = 5f;

    //
    public delegate void ActivePlayer(bool active);
    ActivePlayer playerActive;

    void Awake()
    {
        if (INST == null)
            INST = this;
        else
            Destroy(this);

        DontDestroyOnLoad(gameObject);

        spawner = GetComponent<EnemySpawner>();
        spawner.SetListener(EndLevel);
        UIManager.INSTANCE.StartGame();
    }

    private void EndLevel()
    {
        if (waitingFunction == null)
        {
            Debug.Log("Start ended phase");
            gameState = GameState.ended;

            //disable player
            playerActive(false);

            //
            waitingFunction = EndLevelDelay();
            StartCoroutine(waitingFunction);

            //activate end game ui
            UIManager.INSTANCE.EndGame();
        }
    }

    private IEnumerator EndLevelDelay()
    {
        yield return new WaitForSeconds(endOfLevelDelay);
        gameState = GameState.purchaseUpgrade;

        //
        UIManager.INSTANCE.PurchasePhase();
        //

        StopCoroutine(waitingFunction);
        waitingFunction = null;

        Debug.Log("Start purchase phase");
    }

    [ContextMenu("end purchase phase")]
    public void EndPurchasePhase()
    {
        gameState = GameState.playing;

        //activate start game ui
        UIManager.INSTANCE.StartGame();

        Debug.Log("end playing phase");
        spawner.StartLevel();

        //re enable player
        playerActive(true);
    }

    [ContextMenu("Kill Player")]
    public void PlayerDeath()
    {
        gameState = GameState.playing;

        UIManager.INSTANCE.PlayerDeath();

        Debug.Log("Player died");
    }

    public void InitialisePlayer(ActivePlayer enablePlayer)
    {
        playerActive = enablePlayer;
    }

    public void EnemyDeath()
    {
        spawner.EnemyDied();
    }

    [ContextMenu("0.25x Speed")]
    public void Speed_quarterx()
    {
        Time.timeScale = 0.25f;
    }

    [ContextMenu("0.5x Speed")]
    public void Speed_halfx()
    {
        Time.timeScale = 0.5f;
    }

    [ContextMenu("1x Speed")]
    public void Speed1x()
    {
        Time.timeScale = 1f;
    }

    [ContextMenu("2x Speed")]
    public void Speed2x()
    {
        Time.timeScale = 2f;
    }

    [ContextMenu("5x Speed")]
    public void Speed5x()
    {
        Time.timeScale = 5f;
    }

    [ContextMenu("10x Speed")]
    public void Speed10x()
    {
        Time.timeScale = 10f;
    }
}

public enum GameState
{
    playing, ended, purchaseUpgrade, dead
}

public delegate void BasicMethod();
