using System.Collections;
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
        Debug.Log("Start purchase phase");
        //
        UIManager.INSTANCE.PurchasePhase();
        //
        gameState = GameState.purchaseUpgrade;

        StopCoroutine(waitingFunction);
        waitingFunction = null;
    }

    [ContextMenu("end purchase phase")]
    public void EndPurchasePhase()
    {
        //activate start game ui
        UIManager.INSTANCE.StartGame();

        Debug.Log("end playing phase");
        gameState = GameState.playing;
        spawner.StartLevel();

        //re enable player
        playerActive(true);
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
    playing, ended, purchaseUpgrade
}

public delegate void BasicMethod();
