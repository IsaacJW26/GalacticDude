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
}

public enum GameState
{
    playing, ended, purchaseUpgrade
}

public delegate void BasicMethod();
