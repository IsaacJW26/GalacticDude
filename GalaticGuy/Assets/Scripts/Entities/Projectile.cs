using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    [System.Serializable]
    protected class Stats
    {
        public int damage;
        public float initialSpeed;
        public float yAccelerationOverTime = 0f;
        [Range(1f,1000f)]
        public float dragOverTime = 1f;

        public int maxLifeTime = DEFAULT_LIFETIME;

        public int hp = 1;
        public const int DEFAULT_LIFETIME = 180;
    }
    protected int timeSinceBirth = 0;
    protected int index;

    Rigidbody2D rb;
    protected Vector2 velocity;
    protected IShooter parent;

    [SerializeField]
    protected Stats stats;

    [Header("Collisions")]
    [SerializeField]
    bool player = true;
    [SerializeField]
    bool projectiles = true;
    [SerializeField]
    bool enemies = true;
    [Space(0.1f)]
    [SerializeField]
    bool destroyOnHit = true;
    [SerializeField]
    bool rotateTowardsDirection = false;
    [SerializeField]
    private AudioEventNames audioClip = AudioEventNames.PlayerFireSmallBullet;
    
    protected virtual void FixedUpdate()
    {
        velocity = GetVelocity();
        rb.velocity = velocity;
        timeSinceBirth++;

        if (timeSinceBirth > stats.maxLifeTime)
            DisableObject();
    }

    protected Vector2 GetVelocity()
    {
        Vector2 drag = (velocity / stats.dragOverTime);
        Vector2 accelerated = drag + Vector2.up* stats.yAccelerationOverTime;
        
        return (accelerated*(Time.fixedDeltaTime)  + velocity * (1f-Time.fixedDeltaTime));
    }

    public void Initialise(int index, IShooter parent)
    {
        if (stats.maxLifeTime <= 0)
            stats.maxLifeTime = Stats.DEFAULT_LIFETIME;
        rb = GetComponent<Rigidbody2D>();

        this.index = index;
        this.parent = parent;

        OnInitialise();
    }

    public virtual void OnInitialise()
    {
        //instantiated by children
    }

    public void Activate(Vector3 position, Vector2 direction)
    {
        this.velocity = direction.normalized * stats.initialSpeed;
        transform.position = position;
        timeSinceBirth = 0;
        gameObject.SetActive(this);
        GameManager.AudioEvents.PlayAudio(audioClip);

        // 
        if(rotateTowardsDirection)
        {
            float zRotation = Vector3.Angle(Vector2.down, direction);
            // flip direction if on left
            if(direction.x < 0f)
                zRotation *= -1f;

            transform.rotation = Quaternion.Euler(0, 0, zRotation);
        }
    }

    //default behaviour: destroy on hit
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckEnemy(collision) ||
            CheckPlayer(collision) ||
            CheckProj(collision))
        {
            //damage hit target
            IDamageable damageable = collision.GetComponentInParent<IDamageable>();
            damageable?.OnDamage(stats.damage);

            //disable this object
            if(!CheckProj(collision))
                OnDamage(stats.hp);
        }
    }

    private bool CheckTag(Collider2D col, string tag)
    {
        return col.tag.Equals(tag);
    }

    public bool CheckProj(Collider2D col)
    {
        return CheckTag(col, Labels.Tags.PROJECTILE) && projectiles;
    }

    public bool CheckEnemy(Collider2D col)
    {
        return CheckTag(col, Labels.Tags.ENEMY) && enemies;
    }

    public bool CheckPlayer(Collider2D col)
    {
        return CheckTag(col, Labels.Tags.PLAYER) && player;
    }

    //Destroys 
    public virtual void OnDamage(int inDamage)
    {
        stats.hp--;
        if (destroyOnHit && stats.hp <= 0)
            DisableObject();
    }

    public virtual void OnDeath()
    {  }

    public virtual void DisableObject()
    {
        parent.DisableProjectile(this);
    }

    public int GetIndex()
    {
        return index;
    }
}
