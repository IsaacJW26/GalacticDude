using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrency : MonoBehaviour
{
    [SerializeField]
    Image currencyImage;
    [SerializeField]
    Text currencyText;

    public void UpdateCurrency(int value)
    {
        currencyText.text = string.Format("{0:#,#}", value);
    }

    [ContextMenu("Test 0")]
    public void TestCurrency0()
    {
        UpdateCurrency(0);
    }

    [ContextMenu("Test 1A")]
    public void TestCurrency1()
    {
        UpdateCurrency(123456789);
    }

    [ContextMenu("Test 2B")]
    public void TestCurrency2()
    {
        UpdateCurrency(4532);
    }

}
