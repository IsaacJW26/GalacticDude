using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIShopItem : MonoBehaviour
{
    UIShop shop;
    PlayerUpgrade upgrade;
    private Text uICost = null;
    public Text UiCost
    {
        get
        {
            if (uICost == null)
            {
                uICost = GetComponentInChildren<Text>();
                Debug.LogWarning("uICost is unassigned, set to default");
            }
            return uICost;
        }
    }

    private Image uiImage = null;
    public Image UiImage
    {
        get
        {
            if (uiImage == null)
            {
                uiImage = Array.Find(
                    GetComponentsInChildren<Image>(),
                    image => { return image.name == "Icon"; });
                Debug.LogWarning("uiImage is unassigned, set to default");
            }
            return uiImage;
        }
    }

    public void Awake()
    {
        shop = GetComponentInParent<UIShop>();
    }

    //Set values of upgrades to item
    public void InitialiseShopItem(PlayerUpgrade upgrade)
    {
        if(shop == null)
            shop = GetComponentInParent<UIShop>();

        this.upgrade = upgrade;
        Debug.Log(this.upgrade.ToString() + " " + upgrade.ToString());
        UiImage.sprite = upgrade.Sprite;
        UiCost.text = upgrade.GetCost().ToString();

        if (!ShopManager.INSTANCE.CanAfford(upgrade.GetCost()))
        {
            //GetComponent<Animator>().SetTrigger(Labels.UIAnimProperties.DISABLED);
            GetComponent<Button>().enabled = false;
            UiImage.color = Color.grey;
        }
        else
        {
            GetComponent<Button>().enabled = true;
            UiImage.color = Color.white;
        }
    }

    public void PurchaseUpgrade()
    {
        Debug.Log("Purchasing upgrade " + upgrade.GetType());
        shop.PurchaseUpgrade(upgrade);
    }
}

