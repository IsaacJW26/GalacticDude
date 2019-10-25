using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager INSTANCE = null;

    UICharge charge;
    UIHp health;
    UIShop storeUI;

    [SerializeField]
    UnityEngine.UI.Text UiText;
    [SerializeField]
    GameObject Uibackground;
    readonly string STR_CLEAR = "Wave Cleared";
    readonly string STR_INCOMING = "Wave Incoming";
    readonly string STR_DEAD = "Game Over\nHold any key to try again";


    // Start is called before the first frame update
    void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(this);

        charge = GetComponentInChildren<UICharge>();
        health = GetComponentInChildren<UIHp>();
        storeUI = GetComponentInChildren<UIShop>();
    }

    // Update is called once per frame
    public void UpdateChargeL(float percent)
    {
        charge.UpdateChargeL(percent);
    }
    public void UpdateChargeR(float percent)
    {
        charge.UpdateChargeR(percent);
    }

    public void UpdateHP(int hp)
    {
        health.UpdateHP(hp);
    }

    public void RemoveHP(int newHp)
    {
        health.RemoveHP(newHp);
    }

    //
    public void StartGame()
    {
        UiText.gameObject.SetActive(false);
        //activate UI
        charge.gameObject.SetActive(true);
        health.gameObject.SetActive(true);
        Uibackground.SetActive(true);
        //disable store
        storeUI.gameObject.SetActive(false);
    }

    //
    public void EndGame()
    {
        //activate text
        UiText.text = STR_CLEAR;
        UiText.gameObject.SetActive(true);

        //disable everything else
        charge.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        Uibackground.SetActive(false);
    }

    //de activate everything
    public void PurchasePhase()
    {
        //activate store ui
        storeUI.gameObject.SetActive(true);
        //
        UiText.gameObject.SetActive(false);
        charge.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        Uibackground.SetActive(false);
    }

    //playerdeath
    public void PlayerDeath()
    {
        //activate text
        UiText.text = STR_DEAD;
        UiText.gameObject.SetActive(true);

        //disable everything else
        charge.gameObject.SetActive(false);
        health.gameObject.SetActive(false);
        Uibackground.SetActive(false);
    }
}
