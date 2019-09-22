using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIShop : MonoBehaviour
{
    Button[] shopElement;
    int currentIndex = 0;
    int delay = 0;
    const int maxDelay = 10;

    void Start()
    {
        shopElement = GetComponentsInChildren<Button>();
        EventSystem.current.SetSelectedGameObject(null);
        shopElement[currentIndex].Select();
    }

    void FixedUpdate()
    {
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
            else if (Input.GetButtonDown("Fire1"))
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
        currentIndex = (currentIndex + 1) % shopElement.Length;
        shopElement[currentIndex].Select();
    }

    public void SelectPrev()
    {
        //shopElement[currentIndex].SetBool("Select", false);
        currentIndex = (currentIndex - 1 + shopElement.Length) % shopElement.Length;
        shopElement[currentIndex].Select();
    }

    //
    public void SelectButton()
    {
        shopElement[currentIndex].onClick.Invoke();
    }

    public void PurchaseUpgrade(PlayerUpgrade upgrade)
    {

    }

    public void ExitShop()
    {
        
    }
}


