using System.Collections;
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
        uIImage.sprite = upgrade.Sprite;
        uICost.text = upgrade.GetCost().ToString();

        if (!ShopManager.INSTANCE.CanAfford(upgrade.GetCost()))
        {
            //GetComponent<Animator>().SetTrigger(Labels.UIAnimProperties.DISABLED);
            GetComponent<Button>().enabled = false;
            uIImage.color = Color.grey;
        }
        else
        {
            GetComponent<Button>().enabled = true;
            uIImage.color = Color.white;
        }
    }

    public void PurchaseUpgrade()
    {
        Debug.Log("Purchasing upgrade " + upgrade.GetType());
        shop.PurchaseUpgrade(upgrade);
    }
}

