using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIShop : MonoBehaviour
{
    Animator[] shopElement;
    int currentIndex = 0;
    int delay = 0;
    const int maxDelay = 10;
    // Start is called before the first frame update
    void Start()
    {
        shopElement = GetComponentsInChildren<Animator>();
        foreach(Animator elem in shopElement)
        {
            if(elem.gameObject != gameObject)
                elem.SetBool("Select", false);
        }

        while (shopElement[currentIndex].gameObject == gameObject)
        {
            currentIndex = (currentIndex + 1) % shopElement.Length;
        }
        shopElement[currentIndex].SetBool("Select", true);

    }

    // Update is called once per frame
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
        shopElement[currentIndex].SetBool("Select", false);
        do
        {
            currentIndex = (currentIndex + 1) % shopElement.Length;
        } while (shopElement[currentIndex].gameObject == gameObject);
        shopElement[currentIndex].SetBool("Select", true);
    }

    public void SelectPrev()
    {
        shopElement[currentIndex].SetBool("Select", false);
        do
        {
            currentIndex = (currentIndex - 1 + shopElement.Length) % shopElement.Length;
        } while (shopElement[currentIndex].gameObject == gameObject);
        shopElement[currentIndex].SetBool("Select", true);
    }
}


