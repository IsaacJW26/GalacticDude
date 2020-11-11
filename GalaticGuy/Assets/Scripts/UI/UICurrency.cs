using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICurrency : MonoBehaviour
{
    public static UICurrency INST = null;

    [SerializeField]
    private Image currencyImage = null;

    public int currency = 0;

    public Image CurrencyImage
    {
        get
        {
            if (currencyImage == null)
                currencyImage = GetComponent<Image>();

            return currencyImage;
        }
    }

    public void Awake()
    {
        if(INST == null)
            INST = this;
        else
            Destroy(this);
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

    // Place holder to load currency, as shop manager managed it previously
    // and now is disabled
    public void AddCurrency()
    {
        currency++;
        UpdateCurrency(currency*10);
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
