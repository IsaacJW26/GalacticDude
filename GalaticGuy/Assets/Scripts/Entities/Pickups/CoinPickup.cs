﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Movement))]
public class CoinPickup : MonoBehaviour
{
    Movement movement;
    SpriteRenderer rend;
    [SerializeField]
    void OnEnable()
    {
        movement = GetComponent<Movement>();
        rend = GetComponentInChildren<SpriteRenderer>();
    }

    public void Initialise(int layerOrder)
    {
        rend.sortingOrder = layerOrder;
    }

    void FixedUpdate()
    {
        movement.InputDirectionX(0f);
        movement.InputDirectionY(-1f);

        if(transform.position.y < GameManager.LOWEST_Y)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == Labels.Tags.PLAYER)
        {
            GameManager.AudioEvents.PlayAudio(AudioEventNames.CoinPickup);
            UICurrency.INST.AddCurrency();
            Destroy(gameObject);
        }
    }
}
