using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageable
{
    [System.Serializable]
    private struct Stats
    {
        public int shootInterval;
        public int damage;
        public float initialSpeed;
        public float projectileDrag;
        public int maxLifeTime;
    }
    protected int timeSinceBirth = 0;
    protected int index;

    Rigidbody2D rb;
    protected Vector2 velocity;
    protected CharacterShoot parent;

    [SerializeField]
    Stats stats;

    [Header("Collisions")]
    [SerializeField]
    bool player = true;
    [SerializeField]
    bool projectiles = true;
    [SerializeField]
    bool enemies = true;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        rb.velocity = velocity;
        timeSinceBirth++;

        if (timeSinceBirth > stats.maxLifeTime)
            DisableObject();
    }

    public void Initialise(int index, CharacterShoot parent)
    {
        this.index = index;
        this.parent = parent;
    }

    public void Activate(Vector3 position, Vector2 direction)
    {
        this.velocity = direction.normalized * stats.initialSpeed;
        transform.position = position;
        timeSinceBirth = 0;
        gameObject.SetActive(this);
    }

    //default behaviour: destroy on hit
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckEnemy(collision)||
            CheckPlayer(collision))
        {
            //damage hit target
            IDamageable damageable = collision.GetComponentInParent<IDamageable>();
            damageable?.OnDamage(stats.damage);
            //disable this object
            OnDamage(0);
        }
    }

    private bool CheckTag(Collider2D col, string tag)
    {
        return col.tag.Equals(tag);
    }
    /*
    public bool CheckProj(Collider2D col)
    {
        return CheckTag(col, Labels.TAGS.PROJECTILE) && projectiles;
    }
    */
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
        DisableObject();
    }

    public virtual void DisableObject()
    {
        parent.DisableProjectile(index);
    }
}
