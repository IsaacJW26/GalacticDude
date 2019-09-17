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
        /*
        foreach (Button elem in shopElement)
        {
            if(elem.gameObject != gameObject)
                elem.OnDeselect();
        }
        */
        shopElement[currentIndex].Select();
    }

    void FixedUpdate()
    {
        if(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS) > 0)
        {
            if(delay <= 0)
            {
                delay = maxDelay;
                SelectNext();
            }
        }
        else if(Input.GetAxis(Labels.Inputs.HORIZONTAL_AXIS) < 0)
        {
            if (delay <= 0)
            {
                delay = maxDelay;
                SelectPrev();
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
}


