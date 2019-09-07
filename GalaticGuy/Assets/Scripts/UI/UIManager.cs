using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager INSTANCE = null;

    UICharge charge;
    UIHp health;
    [SerializeField]
    GameObject UiText;
    [SerializeField]
    GameObject Uibackground;

    // Start is called before the first frame update
    void Awake()
    {
        if (INSTANCE == null)
            INSTANCE = this;
        else
            Destroy(this);

        charge = GetComponentInChildren<UICharge>();
        health = GetComponentInChildren<UIHp>();
    }

    // Update is called once per frame
    public void UpdateCharge(float percent)
    {
        charge.UpdateCharge(percent);
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
        UiText.SetActive(false);
        charge.gameObject.SetActive(true);
        health.gameObject.SetActive(true);
        Uibackground.SetActive(true);
    }


    //
    public void EndGame()
    {
        UiText.SetActive(true);

        charge.gameObject.SetActive(false);
        health.gameObject.SetActive(false);

        Uibackground.SetActive(false);
    }

}
