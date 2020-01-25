using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager INSTANCE = null;
    UpgradeManager upgradeManager = null;
    const int START_CURRENCY = int.MaxValue;
    int curr;
    int CurrencyRemaining { get { return curr; } set { curr = value; ui.UpdateCurrency(curr); } }

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
        PopulateShop();
        CurrencyRemaining = START_CURRENCY;

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
            CurrencyRemaining -= upgrade.GetCost();

            success = true;
        }
        else
        {
            Debug.Log("Unable to buy item; "
                + upgrade.GetType().ToString()
                + ",cost:" + upgrade.GetCost()
                + ",cash:" + CurrencyRemaining);

            success = false;
        }
    }

    public bool CanAfford(int cost)
    {
        return CurrencyRemaining >= cost;
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
