using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrency : MonoBehaviour
{
    [SerializeField]
    private Image currencyImage = null;

    public Image CurrencyImage
    {
        get
        {
            if (currencyImage == null)
                currencyImage = GetComponent<Image>();

            return currencyImage;
        }
    }

    [SerializeField]
    private Text currencyText = null;

    public Text CurrencyText
    {
        get
        {
            if (currencyText == null)
                currencyText = GetComponent<Text>();

            return currencyText;
        }
    }

    public void UpdateCurrency(int value)
    {
        CurrencyText.text = string.Format("{0:#,#}", value);
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
