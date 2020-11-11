using LevelInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner INST = null;

    [SerializeField]
    LevelContainer levelInfo = null;

    [SerializeField]
    float spawnPositionY = 6;
    
    private int currentLevel;
    int killedCount = 0;
    int totalEnemies = 0;
    int threatlevel = 0;
    const int MAXTIME = 450;
    const int MINTIME = 10;
    const int REROLL_REDUCER = 13;

    const int MAX_REROLLS = 10;

    const int DIFF_MULTI = 10;

    public delegate int EndLevelDelegate();
    private EndLevelDelegate endListener;

    private bool ended = false;
    Queue<EnemyInfo> enemySpawnQueue = new Queue<EnemyInfo>();

    // Start is called before the first frame update
    void Start()
    {
        if (INST == null)
            INST = this;
        else
            Destroy(this);

        currentLevel = 0;
        killedCount = 0;

        StartLevel();
    }

    public void StartLevel()
    {    
        totalEnemies = 0;
        killedCount = 0;
        ended = false;

        CreateAllEnemies();
        SpawnNextEnemy();
    }

    public bool GameEnded()
    {
        return levelInfo.Levels.Count <= currentLevel;
    }

    private void CreateAllEnemies()
    {
        // Spawn boss on boss levels
        if (levelInfo.Levels[currentLevel].containsBoss)
        {
            SpawnRandomBoss(levelInfo.Levels[currentLevel].difficulty);
        }
        // Other levels
        else
        {
            int enemiesToSpawn = levelInfo.Levels[currentLevel].length;

            for(int ii = 0; ii < enemiesToSpawn; ii++)
            {
                Enemy enemy = GetRandomEnemyPrefab(levelInfo.Levels[currentLevel].difficulty);
                AddEnemyToQueue(enemy, GetRandomSpawnPos());
            }
        }
    }


    [ContextMenu("Spawn boss")]
    public void SpawnBossTest()
    {
        SpawnRandomBoss(levelInfo.Levels[currentLevel].difficulty);
    }

    private void AddEnemyToQueue(Enemy prefab, Vector3 position)
    {
        Enemy enemy = SpawnEnemy(prefab, position);

        enemy.gameObject.SetActive(false);
        EnemyInfo info = new EnemyInfo();

        info.enemy = enemy;
        info.delay = GetNextTime(levelInfo.Levels[currentLevel]);

        enemySpawnQueue.Enqueue(info);
    }

    private void CheckThreat()
    {
        int newThreatLevel =
            (totalEnemies -
                (enemySpawnQueue.Count + killedCount))
                    / 2;

        while(newThreatLevel != threatlevel)
        {
            if(newThreatLevel > threatlevel)
            {
                IncreaseThreat();
            }
            else if(newThreatLevel < threatlevel)
            {
                DecreaseThreat();
            }
        }

        Debug.Log("spawner threat: " + threatlevel);
    }

    private void IncreaseThreat()
    {
        threatlevel++;
        GameManager.AudioEvents.PlayAudio(AudioEventNames.IncreaseThreatLevel);
    }

    private void DecreaseThreat()
    {
        threatlevel--;
        GameManager.AudioEvents.PlayAudio(AudioEventNames.DecreaseThreatLevel);
    }

    private void SpawnNextEnemy()
    {
        if (enemySpawnQueue.Count > 0)
        {
            EnemyInfo enemyInfo = enemySpawnQueue.Dequeue();

            CheckThreat();

            StartCoroutine(WaitForNextSpawn(enemyInfo));
        }
    }

    private IEnumerator WaitForNextSpawn(EnemyInfo enemyInfo)
    {
        float delay = enemyInfo.delay / 60f;
        yield return new WaitForSeconds(delay);
        enemyInfo.enemy.gameObject.SetActive(true);
        //enemyInfo.enemy.transform.position = enemyInfo.spawnPosition;
        SpawnNextEnemy();
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

        //remove difficulty??

        int nextTime = Mathf.Clamp(unclampedTime, MINTIME, MAXTIME);

        return FrequencyDeterminer(totalEnemies, nextTime * 6, 5);
    }
    private Enemy SpawnRandomBoss(int difficulty)
    {
        GameManager.INST.OnBossEnter();

        Enemy enemy = null;
        totalEnemies++;
        int spawnIndex = Random.Range(0, levelInfo.DifficultyTiers[difficulty].bossPrefabs.Length);

        Vector3 spawnPosition = new Vector3(0f, spawnPositionY);

        enemy = SpawnEnemy(levelInfo.DifficultyTiers[difficulty].bossPrefabs[spawnIndex], spawnPosition);

        enemy.InitialiseProperties(isBoss: true);
        return enemy;
    }

    private Enemy GetRandomEnemyPrefab(int difficulty)
    {
        int spawnIndex = Random.Range(0, levelInfo.DifficultyTiers[difficulty].enemyPrefabs.Length);

        return levelInfo.DifficultyTiers[difficulty].enemyPrefabs[spawnIndex];
    }

    //gives position inside spawn bounds
    public Vector3 GetRandomSpawnPos()
    {
        return new Vector3(Random.Range(-Movement.xBound, Movement.xBound), spawnPositionY);
    }

    public Enemy SpawnEnemy(Enemy enemy, Vector3 position)
    {
        totalEnemies++;

        if(position.x > Movement.xBound)
        {
            position.x = Movement.xBound - 0.01f;
        }
        else if(position.x < -Movement.xBound)
        {
            position.x = -(Movement.xBound - 0.01f);
        }

        Enemy outEnemy = Instantiate(enemy, position, Quaternion.identity);

        return outEnemy;
    }

    public void SetListener(EndLevelDelegate endListener)
    {
        this.endListener = endListener;
    }

    private void OnLevelEnd()
    {
        currentLevel = endListener();
    }

    public void EnemyDied()
    {
        killedCount++;
        CheckThreat();
        if (!ended &&
            enemySpawnQueue.Count <= 0 &&
            killedCount >= totalEnemies)
        {
            ended = true;

            Debug.Log("level end");
            OnLevelEnd();
        }
    }

    public void BossDied()
    {
        if(!ended)
        {
            ended = true;

            StartCoroutine(WaitForBossLevelEnd());
        }
    }

    public IEnumerator WaitForBossLevelEnd()
    {
        yield return new WaitForSeconds(5f);
        OnLevelEnd();
    }

    public int FrequencyDeterminer(int current, int longestSpawnTime, int minspawnTime)
    {
        //reaches peak at 7
        int frequency = 7;
        float percent = (current % frequency) / (float)frequency;
        float sinVal = UnityTools.Maths.Trig.PSin(percent * 2 * Mathf.PI);

        //Debug.Log($"{percent} = {sinVal}");
        return (int)(sinVal * longestSpawnTime) + minspawnTime;
    }
}

