using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class UIShopText : MonoBehaviour
{
    [SerializeField]
    Text flavourText;
    [SerializeField]
    Text shoptext;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (shoptext == null || flavourText == null)
            Debug.LogWarning("Uninitialised text object in UIShopText");
    }

    public void UpdateText(string newText)
    {
        shoptext.text = newText;
    }

    public void AnimateText()
    {
        //play animation
        anim.SetTrigger(Labels.UITextAnimProperties.SELECTED_TRIG);
    }
}
