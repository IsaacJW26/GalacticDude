using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager INSTANCE = null;
    UpgradeManager upgradeManager = null;
    int currencyRemaining = int.MaxValue;
    PlayerUpgrade[] currentUpgrades;
    [SerializeField]
    UIShop ui = null;

    public void Start()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else if (INSTANCE != this)
            DestroyImmediate(this);

        upgradeManager = GameManager.INST.GetPlayerComponent<UpgradeManager>();
        //ui = FindObjectOfType<UIShop>();
        PopulateShop();
    }

    public PlayerUpgrade[] GetRandomItems(int size)
    {
        //stub-------------------------------
        Debug.LogWarning("Stub called - GetRandomItems");
        return UpgradeCollection.GetAllUpgrades();
    }

    public void TryBuyItem(PlayerUpgrade upgrade, out bool success)
    {
        if (CanAfford(upgrade.GetCost()))
        {
            Debug.Log("Upgrade bought " + upgrade.GetType());
            upgradeManager.AddUpgrade(upgrade);

            success = true;

        }
        else
        {
            Debug.Log("Unable to buy item; "
                + upgrade.GetType().ToString()
                + ",cost:" + upgrade.GetCost()
                + ",cash:" + currencyRemaining);

            success = false;
        }

    }

    public bool CanAfford(int cost)
    {
        return currencyRemaining >= cost;
    }

    //populates shop with items appropriate for the level
    public void PopulateShop()
    {
        Debug.LogWarning("Stub");

        currentUpgrades = GetRandomItems(0);
        StartCoroutine(WaitToPoplulateUpgrades());
    }

    IEnumerator WaitToPoplulateUpgrades()
    {
        yield return null;
        yield return null;
        ui.SetUpgrades(currentUpgrades);
    }
}
