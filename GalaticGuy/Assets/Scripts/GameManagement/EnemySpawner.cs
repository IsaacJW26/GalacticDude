using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner INST = null;

    [SerializeField]
    float spawnPositionY = 6;
    [SerializeField]
    Level[] levels;
    int currentLevel;

    int killedCount = 0;
    int spawnedCount = 0;

    const int MAXTIME = 450;
    const int MINTIME = 10;
    const int REROLL_REDUCER = 13;

    const int MAX_REROLLS = 10;

    const int DIFF_MULTI = 10;

    [System.Serializable]
    public struct Tier
    {
        [SerializeField]
        public string name;
        [SerializeField]
        public Enemy[] enemiesPrefabs;
        [SerializeField]
        public Enemy[] bossPrefabs;
        [SerializeField]
        public Squad[] squads;
    }

    [System.Serializable]
    public struct Squad
    {
        [System.Serializable]
        public struct SquadEnemy
        {
            public Enemy enemy;
            public Vector2 spawnPosition;
        }

        public SquadEnemy[] squadMembers;
    }

    [System.Serializable]
    public struct EnemyInfo
    {
        public Enemy enemy;
        public int delay;
    }

    [SerializeField]
    Tier[] tiers = new Tier[0];

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
        SpawnRandomBoss(levels[currentLevel].difficulty);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //boss levels
        if (levels[currentLevel].containsBoss)
        {
            if (spawnedCount <= 0)
                SpawnRandomBoss(levels[currentLevel].difficulty);
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
                OnLevelEnd();
                Debug.Log("spawning stopped");
                System.GC.Collect();
            }
        }
    }

    private void AddEnemyToQueue(Enemy prefab, Vector3 position)
    {
        Enemy enemy = SpawnEnemy(prefab, position);
        enemy.gameObject.SetActive(false);
        EnemyInfo info = new EnemyInfo();

        info.enemy = enemy;
        info.delay = GetNextTime(levels[currentLevel]);
        //info.spawnPosition = spawnpos;

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
        int spawnIndex = Random.Range(0, tiers[difficulty].bossPrefabs.Length);

        Vector3 spawnPosition = new Vector3(0f, spawnPositionY);

        enemy = SpawnEnemy(tiers[difficulty].bossPrefabs[spawnIndex], spawnPosition);

        return enemy;
    }

    private void SpawnRandomSquad(int difficulty)
    {
        int spawnIndex = Random.Range(0, tiers[difficulty].squads.Length);
        Squad squad = tiers[difficulty].squads[spawnIndex];
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
        int spawnIndex = Random.Range(0, tiers[difficulty].enemiesPrefabs.Length);

        //enemy = SpawnEnemy(tiers[difficulty].enemiesPrefabs[spawnIndex], spawnPosition);

        return tiers[difficulty].enemiesPrefabs[spawnIndex];
    }


    //gives position inside spawn bounds
    public Vector3 GetRandomSpawnPos()
    {
        return new Vector3(Random.Range(-CharacterMovement.xBound, CharacterMovement.xBound), spawnPositionY);
    }

    public Enemy SpawnEnemy(Enemy enemy, Vector3 position)
    {
        spawnedCount++;

        if(position.x > CharacterMovement.xBound)
        {
            position.x = CharacterMovement.xBound - 0.01f;
        }
        else if(position.x < -CharacterMovement.xBound)
        {
            position.x = -(CharacterMovement.xBound - 0.01f);
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

        int enemiesToSpawn = levels[currentLevel].length;

        for(int ii = 0; ii < enemiesToSpawn; ii++)
        {
            Enemy enemy = GetRandomEnemyPrefab(levels[currentLevel].difficulty);
            AddEnemyToQueue(enemy, GetRandomSpawnPos());
        }
        SpawnNextEnemy();
    }

    public void EnemyDied()
    {
        killedCount++;
    }
}
