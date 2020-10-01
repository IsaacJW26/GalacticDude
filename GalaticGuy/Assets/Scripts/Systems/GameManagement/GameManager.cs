using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(EnemySpawner))]
[RequireComponent(typeof(MusicManager))]
[RequireComponent(typeof(AudioEventHandler))]
public class GameManager : MonoBehaviour
{
    public static GameManager INST = null;

    public static IAudio AudioEvents { get { return INST.audioEventHandler; } }

    private EnemySpawner spawner;
    private MusicManager music;
    private AudioEventHandler audioEventHandler;
    GameState gameState = GameState.playing;
    IEnumerator waitingFunction = null;

    const float endOfLevelDelay = 5f;

    //player
    public delegate void ActivePlayer(bool active);
    ActivePlayer playerActive;
    MainCharacter player;

    //respawn hold time
    const float RESPAWN_HOLD_DURATION = 1f;
    float heldTime = -RESPAWN_HOLD_DURATION;

    [SerializeField]
    CoinPickup currencyPrefab;

    //public static AudioManager audioManager { get; private set; }

    public CoinPickup CurrencyPrefab { get => currencyPrefab; private set => currencyPrefab = value; }

    public const float LOWEST_Y = -7f;

    private int currentLevel = 0;
     
    void Awake()
    {
        if (INST == null)
            INST = this;
        else
            Destroy(this);

        //DontDestroyOnLoad(gameObject);
        UIManager.INSTANCE.StartGame();

        //audioManager = GetComponent<AudioManager>();
        SetPlayer(FindObjectOfType<MainCharacter>());
        spawner = GetComponent<EnemySpawner>();
        music = GetComponent<MusicManager>();

        audioEventHandler = GetComponent<AudioEventHandler>();

        if(audioEventHandler == null)
            audioEventHandler = gameObject.AddComponent<AudioEventHandler>();
        audioEventHandler.Initialise();
        music.Initialise(audioEventHandler);

        spawner.SetListener(EndLevel);
        currentLevel = 0;
    }

    private void Update()
    {
        if (gameState == GameState.dead)
        {
            if (Input.anyKey)
            {
                if (heldTime <= 0f)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                else
                    heldTime -= Time.deltaTime;
            }
            else
            {
                heldTime = RESPAWN_HOLD_DURATION;
            }
        }
    }

    public void SetPlayer(MainCharacter character)
    {
        player = character;
    }

    public Vector3 GetPlayerPos()
    {
        return player.transform.position;
    }

    public T GetPlayerComponent<T>()
    {
        return player.GetComponent<T>();
    }

    public void EndGame()
    {
        UIManager.INSTANCE.EndGame();
    }

    private int EndLevel()
    {
        if (waitingFunction == null)
        {
            Debug.Log("Start ended phase");
            gameState = GameState.endlevel;

            //disable player
            playerActive(false);

            //
            waitingFunction = EndLevelDelay();
            StartCoroutine(waitingFunction);

            //activate end game ui
            UIManager.INSTANCE.ClearedWave();
        }

        // increment current level and return the value
        return ++currentLevel;
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

    public int GetLevelNumber()
    {
        return currentLevel;
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
        heldTime = RESPAWN_HOLD_DURATION;

        gameState = GameState.dead;

        UIManager.INSTANCE.PlayerDeath();

        Debug.Log("Player died");
    }

    public void InitialisePlayer(ActivePlayer enablePlayer)
    {
        playerActive = enablePlayer;
    }

    public void OnBossEnter()
    {
        music.OnBossEnter();
    }

    public void OnBossDeath()
    {
        EnemyDeath();
        music.OnBossDeath();
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
    playing, endlevel, purchaseUpgrade, dead, endgame
}

public delegate void BasicMethod();
