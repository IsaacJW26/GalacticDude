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
    const int MAXTIME = 450;
    const int MINTIME = 10;
    const int REROLL_REDUCER = 13;

    const int MAX_REROLLS = 10;


    const int DIFF_MULTI = 10;

    [SerializeField]
    Enemy[] enemiesPrefabs = null;
    [SerializeField]
    Enemy[] bossPrefabs = null;

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
        //boss levels
        if (levels[currentLevel].containsBoss)
        {
            if(spawnedCount <= 0)
                SpawnRandomBoss(levels[currentLevel].difficulty);
            else if(killedCount > 0 && !ended)
            {
                ended = true;
                OnLevelEnd();
                Debug.Log("spawning stopped");
            }
        }
        //other level
        else
        {
            if (spawnedCount < levels[currentLevel].length)
            {
                if (timeTillNext <= 0)
                {
                    SpawnRandomEnemy(levels[currentLevel].difficulty);
                    //get next spawn time
                    if (spawnedCount < levels[currentLevel].length)
                    {
                        timeTillNext = GetNextTime(levels[currentLevel]);
                        Debug.Log("time till next spawn " + timeTillNext);
                    }
                }
                else
                {
                    timeTillNext--;
                }
            }
            else if (!ended && killedCount >= levels[currentLevel].length)
            {
                ended = true;
                OnLevelEnd();
                Debug.Log("spawning stopped");
            }
        }
    }

    private int GetNextTime(Level level)
    {
        int unclampedTime = (int)Random.Range((float)level.averageTime * 0.5f, (float)level.averageTime * 1.5f);
        unclampedTime = unclampedTime - Random.Range(0, level.difficulty + 1) * DIFF_MULTI;
        //re roll if it above max
        for (int ii = 0; ii < MAX_REROLLS; ii++)
        {
            //redo calculations to prevent squishing
            if (unclampedTime > MAXTIME)
            {
                unclampedTime = (int)Random.Range((float)level.averageTime * 0.5f, (float)level.averageTime * 1.5f) - ii * REROLL_REDUCER;
                unclampedTime = unclampedTime - Random.Range(0, level.difficulty + 1) * DIFF_MULTI;
            }
        }
        //remove difficulty

        int nextTime = Mathf.Clamp(unclampedTime, MINTIME, MAXTIME);

        return nextTime;
    }

    //Level information container
    [System.Serializable]
    private struct Level
    {
        public int difficulty;
        public int length;
        public bool containsBoss;
        public int averageTime;

        public Level(int difficulty, int length, bool containsBoss, int averageTime)
        {
            this.difficulty = difficulty;
            this.length = length;
            this.containsBoss = containsBoss;
            this.averageTime = averageTime;
        }
    }

    private Enemy SpawnRandomBoss(int difficulty)
    {
        Enemy enemy = null;
        spawnedCount++;

        Vector3 spawnPosition = new Vector3(0f, spawnPositionY);

        enemy = Instantiate(bossPrefabs[difficulty], spawnPosition, Quaternion.identity);

        return enemy;
    }

    private Enemy SpawnRandomEnemy(int difficulty)
    {
        Enemy enemy;
        Vector3 spawnPosition = new Vector3(Random.Range(-4f, 4f), spawnPositionY);
        spawnedCount++;
        int difficultyIndex = Random.Range(0, 10) * DIFF_MULTI * difficulty + spawnedCount;

        Debug.Log("diff " + difficultyIndex);
        //spawn easy enemy
        if(difficultyIndex < 45)
        {
            enemy = Instantiate(enemiesPrefabs[0], spawnPosition, Quaternion.identity);
        }
        //spawn med
        else if(difficultyIndex < 100)
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
        ended = false;
    }

    public void EnemyDied()
    {
        killedCount++;
    }
}
