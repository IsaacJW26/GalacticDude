﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    UIShop shop;
    PlayerUpgrade upgrade;
    [SerializeField]
    Text uICost;
    [SerializeField]
    Image uIImage;
    [SerializeField]
    TestUpgradeClass testUpgradeObj;
    [SerializeField]
    TestUpgradeClass testUpgradeObj2;

    public void Awake()
    {
        shop = GetComponentInParent<UIShop>();
    }

    //Set values of upgrades to item
    public void InitialiseShopItem(PlayerUpgrade upgrade)
    {
        this.upgrade = upgrade;
        uIImage.sprite = upgrade.Sprite;
        uICost.text = upgrade.Cost.ToString();
    }

    [ContextMenu("Test Upgrade")]
    public void TestUpgrade()
    {
        InitialiseShopItem(testUpgradeObj);
    }

    [ContextMenu("Test Upgrade - original")]
    public void TestUpgrade2()
    {
        InitialiseShopItem(testUpgradeObj2);
    }


    public void PurchaseUpgrade()
    {
            
    }

    [System.Serializable]
    private class TestUpgradeClass : PlayerUpgrade
    {
        public override void UpdateFrame(Vector3 currentPosition)
        {
            throw new System.NotImplementedException();
        }
    }
}
