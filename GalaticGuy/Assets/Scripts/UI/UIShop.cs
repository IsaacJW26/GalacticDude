using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    Button[] shopButtons;
    UIShopItem[] shopItems;
    UIShopText shopText;
    UICurrency currencyText;

    int currentIndex = 0;
    int delay = 0;
    const int MAX_DELAY = 10;
    [SerializeField]
    GameObject uiPanel = null;
    bool shopActive = false;
    const bool SHOP_FEATURE_ENABLED = false;
    void Awake()
    {
        shopButtons = GetComponentsInChildren<Button>();
        shopItems = GetComponentsInChildren<UIShopItem>();
        shopText = GetComponentInChildren<UIShopText>();
        currencyText = GetComponentInChildren<UICurrency>();

        EventSystem.current.SetSelectedGameObject(null);
        //shopButtons[currentIndex].Select();
    }

    void FixedUpdate()
    {
        if (shopActive && SHOP_FEATURE_ENABLED)
            InputSelection();
    }

    //Gets input from player
    public void InputSelection()
    {
        if (delay <= 0)
        {
            if (Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS) > 0)
            {
                delay = MAX_DELAY;
                SelectNext();
            }
            else if (Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS) < 0)
            {
                delay = MAX_DELAY;
                SelectPrev();
            }
            else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
                if (currentIndex >= 0)
                    SelectButton();
            }
        }

        if (delay > 0)
            delay--;
    }

    public void SelectNext()
    {
        //shopElement[currentIndex].Select;
        currentIndex = (currentIndex + 1) % shopButtons.Length;
        shopButtons[currentIndex].Select();
    }

    public void SelectPrev()
    {
        //shopElement[currentIndex].SetBool("Select", false);
        currentIndex = (currentIndex - 1 + shopButtons.Length) % shopButtons.Length;
        shopButtons[currentIndex].Select();
    }

    public void SetStoreActive(bool isActive)
    {
        if(SHOP_FEATURE_ENABLED)
        {
            uiPanel.SetActive(isActive);
            shopActive = isActive;
            if(isActive)
                shopButtons[0].Select();
        }
    }

    private IEnumerator WaitToSelectInitial()
    {
        //for(int ii = 0; ii < MAX_DELAY; ii++)
            yield return null;
        shopButtons[0].Select();
        Debug.Log("AAAAAA");
    }

    //
    public void SelectButton()
    {
        shopButtons[currentIndex].onClick.Invoke();
    }

    public void PurchaseUpgrade(PlayerUpgrade upgrade)
    {
        bool canAfford;
        ShopManager.INSTANCE.TryBuyItem(upgrade, out canAfford);

        if(canAfford)
        {
            shopText.UpdateText($"{upgrade.GetType().Name} upgrade purchased");
            shopText.AnimateText();
        }
    }

    public void UpdateCurrency(int newValue)
    {
        currencyText.UpdateCurrency(newValue);
    }

    public void SetUpgrades(PlayerUpgrade[] upgrades)
    {
        if(SHOP_FEATURE_ENABLED)
        {
            //compare if upgrades are the same, not including the length
            if(upgrades.Length != (shopButtons.Length -1))
            {
                Debug.LogError("Upgrades are not same length as number of shop items");
            }

            for(int ii = 0; ii < shopItems.Length; ii++)
            {
                Debug.LogWarning("Initialising shop item with magic number");
                shopItems[ii].InitialiseShopItem(upgrades[0]);
            }
        }
    }

    public void ExitShop()
    {
        if(SHOP_FEATURE_ENABLED)
        {
            GameManager.INST.EndPurchasePhase();
        }
    }
}


