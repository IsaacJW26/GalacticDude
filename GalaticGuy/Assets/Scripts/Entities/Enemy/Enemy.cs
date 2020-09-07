using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityTools.Maths;

[RequireComponent(typeof(CharacterShoot))]
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(EnemyAI))]
[RequireComponent(typeof(CharacterAnimator))]
public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    CharacterHealth health = null;
    CharacterShoot shoot;
    Movement movement;
    ICharacterAnimator anim;

    [SerializeField]
    Enemy[] deathChildren = null;

    [Header("Bounty")]
    [SerializeField]
    int minBounty = 1;
    [SerializeField]
    int maxBounty = 1;
    [SerializeField]
    [Range(0,4)]
    int bountyChance = 1;

    protected EnemyAI Ai;
    bool isDead = false;

    [SerializeField]
    [Range(0f, 5f)]
    float explosionScale = 1f;

    private bool isBoss = false;

    void Awake()
    {
        shoot = GetComponent<CharacterShoot>();
        movement = GetComponent<Movement>();
        Ai = GetComponent<EnemyAI>();
        anim = GetComponent<CharacterAnimator>();

        health?.InitialiseMethods(OnDeath);
        Ai?.Initialise(movement, null, shoot);
    }

    public void InitialiseProperties(bool isBoss)
    {
        this.isBoss = isBoss;
    }

    void FixedUpdate()
    {
        if (!isDead)
        {
            Ai.UpdateFrame(transform.position);

            //destroy once end of level is reached
            if (transform.position.y < GameManager.LOWEST_Y)
            {
                Ai.OnDeath();
                OnDeath();
            }
        }
    }

    public virtual void OnDeath()
    {
        if (!isDead)
        {
            isDead = true;
            anim.OnDeath();

            float scale = (transform.localScale.x + transform.localScale.y + transform.localScale.z ) / 3f;

            EffectManager.INSTANCE.CreateExplosion(transform.position, explosionScale * scale);
            //disable everything
                shoot.DestroyPool();
                movement.OnDeath();


            //is there objects to spawn when dead
            if (deathChildren.Length > 0)
                foreach (Enemy enemy in deathChildren)
                {
                    float r = Random.Range(0.5f, 1.0f) * transform.localScale.magnitude, angle = Random.Range(0f, 2f * Mathf.PI);
                    Vector3 location = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * r + transform.position;
                    if (location.y > -1.5f)
                        EnemySpawner.INST.SpawnEnemy(enemy, location);
                }


            //spawn bounty
            if (Random.Range(0,4) < bountyChance)
            {
                StartCoroutine(SpawnRandomCoins());
            }

            StartCoroutine(WaitToDie());
        }
    }

    IEnumerator SpawnRandomCoins()
    {
        int coinLayerOrder = 0;
        int bountyVal = Random.Range(minBounty, maxBounty);
        CoinPickup prefab = GameManager.INST.CurrencyPrefab;
        //spawn random coins for certain value
        while (bountyVal > 0)
        {
            coinLayerOrder++;
            Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f));
            CurrencyType coinType = GetRandomCoin(bountyVal);
            bountyVal -= (int)coinType;
            CoinPickup coin = Instantiate(GameManager.INST.CurrencyPrefab, transform.position +offset, Quaternion.identity);
            coin.Initialise(coinType, coinLayerOrder);
            int del = Mathf.Clamp(Random.Range(-4, 4), 0, 4);
            for (int i = 0; i < del; i++)
            {
                yield return new WaitForFixedUpdate();
            }
        }
    }

    private CurrencyType GetRandomCoin(int maxValue)
    {
        int max = 3;
        if (maxValue >= (int)CurrencyType.big)
            max = 3;
        else if (maxValue >= (int)CurrencyType.medium)
            max = 2;
        else
            max = 1;

        CurrencyType chosenType = CurrencyType.normal;
        switch (Random.Range(0, max))
        {
            case 0:
                chosenType = CurrencyType.normal;
                break;
            case 1:
                chosenType = CurrencyType.medium;
                break;
            case 2:
                chosenType = CurrencyType.big;
                break;
            default:
                break;
        }

        return chosenType;
    }

    IEnumerator WaitToDie()
    {
        yield return null;
        yield return null;

        foreach (var col in GetComponentsInChildren<Collider2D>())
        {
            col.enabled = false;
        }

        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    protected virtual void AnnouceDeath()
    {
        GameManager.INST.EnemyDeath();
    }

    public virtual void OnDamage(int damage)
    {
        if (!isDead)
        {
            anim.OnDamage();
            health.TakeDamage(damage);

            if (damage <= 2)
                EffectManager.INSTANCE.ScreenShakeSmall();
        }
        /*
        else if(damage <= 4)
            ScreenShake.INSTANCE.MediumShake();
        else if(damage >= 5)
            ScreenShake.INSTANCE.BigShake();
            */
        //Stub
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == Labels.Tags.PLAYER)
        {
            MainCharacter player = other.GetComponentInParent<MainCharacter>();
            player.OnDamage();
            OnDeath();
        }
    }

    public CharacterHealth GetHealth()
    {
        return health;
    }
}
