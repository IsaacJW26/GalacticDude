using LevelInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner INST = null;

    [SerializeField]
    LevelContainer levelInfo;

    [SerializeField]
    float spawnPositionY = 6;
    
    int currentLevel;

    int killedCount = 0;
    int spawnedCount = 0;

    const int MAXTIME = 450;
    const int MINTIME = 10;
    const int REROLL_REDUCER = 13;

    const int MAX_REROLLS = 10;

    const int DIFF_MULTI = 10;

    public delegate void EndLevelDelegate();
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

        currentLevel = -1;
        killedCount = 0;

        StartLevel();
    }

    [ContextMenu("Spawn boss")]
    public void SpawnBossTest()
    {
        SpawnRandomBoss(levelInfo.Levels[currentLevel].difficulty);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //boss levels
        if (levelInfo.Levels[currentLevel].containsBoss)
        {
            if (spawnedCount <= 0)
                SpawnRandomBoss(levelInfo.Levels[currentLevel].difficulty);
            else if (killedCount > 0 && !ended)
            {
                ended = true;
                OnLevelEnd();
                Debug.Log("spawning stopped");
            }
        }
        //other level
        else
        {
            if (!ended &&
                enemySpawnQueue.Count <= 0 &&
                killedCount >= spawnedCount)
            {
                ended = true;
                //Game ended
                if(currentLevel + 1 >= levelInfo.Levels.Count)
                {
                    Debug.Log("Game ended");
                }
                else
                    OnLevelEnd();

                Debug.Log("spawning stopped");
                //System.GC.Collect();
            }
        }
    }

    private void AddEnemyToQueue(Enemy prefab, Vector3 position)
    {
        Enemy enemy = SpawnEnemy(prefab, position);
        enemy.gameObject.SetActive(false);
        EnemyInfo info = new EnemyInfo();

        info.enemy = enemy;
        info.delay = GetNextTime(levelInfo.Levels[currentLevel]);
        //info.spawnPosition = spawnpos;
        //Debug.Log($"time = {info.delay}");
        enemySpawnQueue.Enqueue(info);
    }

    private void SpawnNextEnemy()
    {
        if (enemySpawnQueue.Count > 0)
        {
            EnemyInfo enemyInfo = enemySpawnQueue.Dequeue();
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

        return FrequencyDeterminer(spawnedCount, nextTime * 6, 5);
    }
    private Enemy SpawnRandomBoss(int difficulty)
    {
        Enemy enemy = null;
        spawnedCount++;
        int spawnIndex = Random.Range(0, levelInfo.DifficultyTiers[difficulty].bossPrefabs.Length);

        Vector3 spawnPosition = new Vector3(0f, spawnPositionY);

        enemy = SpawnEnemy(levelInfo.DifficultyTiers[difficulty].bossPrefabs[spawnIndex], spawnPosition);

        return enemy;
    }

    private void SpawnRandomSquad(int difficulty)
    {
        int spawnIndex = Random.Range(0, levelInfo.DifficultyTiers[difficulty].squads.Length);
        Squad squad = levelInfo.DifficultyTiers[difficulty].squads[spawnIndex];
        Vector2 squadPosition = GetRandomSpawnPos();

        foreach (Squad.SquadEnemy enemyIn in squad.squadMembers)
        {
            //SpawnEnemy(enemyIn.enemy, squadPosition + enemyIn.spawnPosition);
            //SpawnEnemy()
        }
    }

    private Enemy GetRandomEnemyPrefab(int difficulty)
    {
        //Enemy enemy;
        //Vector3 spawnPosition = new Vector3(Random.Range(-4f, 4f), spawnPositionY);
        int spawnIndex = Random.Range(0, levelInfo.DifficultyTiers[difficulty].enemyPrefabs.Length);

        //enemy = SpawnEnemy(tiers[difficulty].enemiesPrefabs[spawnIndex], spawnPosition);

        return levelInfo.DifficultyTiers[difficulty].enemyPrefabs[spawnIndex];
    }


    //gives position inside spawn bounds
    public Vector3 GetRandomSpawnPos()
    {
        return new Vector3(Random.Range(-Movement.xBound, Movement.xBound), spawnPositionY);
    }

    public Enemy SpawnEnemy(Enemy enemy, Vector3 position)
    {
        spawnedCount++;

        if(position.x > Movement.xBound)
        {
            position.x = Movement.xBound - 0.01f;
        }
        else if(position.x < -Movement.xBound)
        {
            position.x = -(Movement.xBound - 0.01f);
        }

        return Instantiate(enemy, position, Quaternion.identity);
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

        int enemiesToSpawn = levelInfo.Levels[currentLevel].length;

        for(int ii = 0; ii < enemiesToSpawn; ii++)
        {
            Enemy enemy = GetRandomEnemyPrefab(levelInfo.Levels[currentLevel].difficulty);
            AddEnemyToQueue(enemy, GetRandomSpawnPos());
        }
        SpawnNextEnemy();
    }

    [ContextMenu("spawn enemy")]
    public void SpawnRandomEnemyTest()
    {
        
    }

    public void EnemyDied()
    {
        killedCount++;
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

