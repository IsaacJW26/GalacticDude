using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    float spawnPositionY = 5;
    [SerializeField]
    Level[] levels;
    int currentLevel;

    int killedCount = 0;
    int spawnedCount = 0;

    int timeTillNext = 0;
    const int MAXTIME = 600;
    const int DIFF_MULTI = 10;

    [SerializeField]
    Enemy[] enemiesPrefabs;

    public delegate void EndLevelDelegate();
    private EndLevelDelegate endListener;

    private bool ended = false;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;
        killedCount = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (spawnedCount < levels[currentLevel].length)
        {
            if (timeTillNext <= 0)
            {
                SpawnRandomEnemy(levels[currentLevel].difficulty);
                timeTillNext = GetNextTime(levels[currentLevel]);
            }
            else
            {
                timeTillNext--;
            }
        }
        else if (spawnedCount > killedCount)
        {

        }
        else if(!ended)
        {
            ended = true;
            OnLevelEnd();
        }
    }

    private int GetNextTime(Level level)
    {
        int nextTime = MAXTIME - Random.Range(0, level.difficulty+1) * DIFF_MULTI;

        return nextTime;
    }

    [System.Serializable]
    private struct Level
    {
        public int difficulty;
        public int length;
        public bool containsBoss;
    }

    private Enemy SpawnRandomEnemy(int difficulty)
    {
        Enemy enemy;
        Vector3 spawnPosition = new Vector3(Random.Range(-4f, 4f), spawnPositionY);
        spawnedCount++;
        int value = Random.Range(0, 10) * DIFF_MULTI * difficulty + spawnedCount;

        Debug.Log("diff" + value);
        //spawn easy enemy
        if(value < 45)
        {
            enemy = Instantiate(enemiesPrefabs[0], spawnPosition, Quaternion.identity);
        }
        //spawn med
        else if(value < 100)
        {
            enemy = Instantiate(enemiesPrefabs[1], spawnPosition, Quaternion.identity);
        }
        //spawn hard
        else
        {
            enemy = Instantiate(enemiesPrefabs[2], spawnPosition, Quaternion.identity);
        }
        return enemy;
    }

    public void SetListener(EndLevelDelegate endListener)
    {
        this.endListener = endListener;
    }

    private void OnLevelEnd()
    {
        endListener();
    }

    public void StartLevel()
    {
        currentLevel++;
        spawnedCount = 0;
        killedCount = 0;
    }

    public void EnemyDied()
    {
        killedCount++;
    }
}
