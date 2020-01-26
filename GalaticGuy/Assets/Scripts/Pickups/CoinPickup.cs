using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField]
    CurrencyTypes type;
    Movement movement;

    void OnEnable()
    {
        movement = GetComponent<Movement>();
    }

    void FixedUpdate()
    {
        movement.InputDirectionX(0f);
        movement.InputDirectionY(-1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Labels.Tags.PLAYER)
        {
            ShopManager.INSTANCE.AddCurrency(type);
            Destroy(gameObject);
        }
    }
}
