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
    int currentIndex = 0;
    int delay = 0;
    const int maxDelay = 10;
    [SerializeField]
    GameObject uIPanel;
    bool shopActive = false;

    void Awake()
    {
        shopButtons = GetComponentsInChildren<Button>();
        shopItems = GetComponentsInChildren<UIShopItem>();
        EventSystem.current.SetSelectedGameObject(null);
        shopButtons[currentIndex].Select();
    }

    void FixedUpdate()
    {
        if (shopActive)
            InputSelection();
    }

    //Gets input from player
    public void InputSelection()
    {
        if (delay <= 0)
        {
            if (Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS) > 0)
            {
                delay = maxDelay;
                SelectNext();
            }
            else if (Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS) < 0)
            {
                delay = maxDelay;
                SelectPrev();
            }
            else if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Jump"))
            {
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

    internal void SetStoreActive(bool isActive)
    {
        uIPanel.SetActive(isActive);
        shopActive = isActive;
    }

    //
    public void SelectButton()
    {
        shopButtons[currentIndex].onClick.Invoke();
    }

    public void PurchaseUpgrade(PlayerUpgrade upgrade)
    {
        ShopManager.INSTANCE.TryBuyItem(upgrade);
    }

    public void SetUpgrades(PlayerUpgrade[] upgrades)
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

    public void ExitShop()
    {
        GameManager.INST.EndPurchasePhase();
    }
}


