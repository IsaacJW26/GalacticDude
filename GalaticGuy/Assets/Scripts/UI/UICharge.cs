using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharge : MonoBehaviour
{

    [SerializeField]
    Slider chargebar;
    Animator anim;
    bool charging;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    public void UpdateCharge(float percent)
    {
        charging = true;
        chargebar.value = percent;
    }

    private void FixedUpdate()
    {
        anim.SetBool("Charging", charging);
        charging = false;
    }
}
