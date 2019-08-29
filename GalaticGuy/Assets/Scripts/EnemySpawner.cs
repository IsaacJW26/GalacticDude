using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    Vector2 spawnPosition = new Vector2(0, 6);
    [SerializeField]
    Level[] levels;
    int currentLevel;
    int spawnedCount = 0;
    int timeTillNext = 0;
    const int MAXTIME = 600;
    const int DIFF_MULTI = 10;
    [SerializeField]
    Enemy[] enemiesPrefabs;

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = 0;    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeTillNext <= 0)
        {
            SpawnRandomEnemy(levels[currentLevel].difficulty);
            timeTillNext = GetNextTime(levels[currentLevel]);
            Debug.Log("spawn new" + timeTillNext);
        }
        else
        {
            timeTillNext--;
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

    
}
