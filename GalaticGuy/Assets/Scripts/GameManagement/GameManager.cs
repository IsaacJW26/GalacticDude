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
    // Start is called before the first frame update
    void Awake()
    {
        if (INST == null)
            INST = this;
        else
            Destroy(this);
        DontDestroyOnLoad(gameObject);

        spawner = GetComponent<EnemySpawner>();
        spawner.SetListener(EndLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void EndLevel()
    {
        if (waitingFunction == null)
        {
            Debug.Log("Start ended phase");
            gameState = GameState.ended;
            UIManager.INSTANCE.EndGame();
            //
            waitingFunction = endLevelDelay();
            StartCoroutine(waitingFunction);
            EndLevelUI();
        }
    }

    private IEnumerator endLevelDelay()
    {
        yield return new WaitForSeconds(endOfLevelDelay);
        Debug.Log("Start purchase phase");
        gameState = GameState.purchaseUpgrade;
        StopCoroutine(waitingFunction);
    }

    private void EndLevelUI()
    {
        //stub
    }

    [ContextMenu("end purchase phase")]
    public void EndPurchasePhase()
    {
        UIManager.INSTANCE.StartGame();

        Debug.Log("start playing phase");
        gameState = GameState.playing;
        spawner.StartLevel();
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
