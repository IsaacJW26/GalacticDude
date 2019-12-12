using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpgradeCollection : MonoBehaviour
{
    public static UpgradeCollection INSTANCE;

    private PlayerUpgrade[] upgrades;

    [SerializeField]
    private Sprite[] spriteList;

    public void OnEnable()
    {
        if (INSTANCE == null && INSTANCE != this)
            INSTANCE = this;
        else
        {
            Debug.LogError("Deplicate UpgradeCollection found");
            DestroyImmediate(this);
        }

        upgrades = GetUpgradeList();

        //verify upgrades are unique
        if (Application.isEditor)
        {
            AreUnique();
        }
    }

    public static PlayerUpgrade GetUpgrade(int index)
    {
        return INSTANCE.upgrades[index];
    }

    public int GetSize()
    {
        return upgrades.Length;
    }

    private void AreUnique()
    {
        for(int ii = 0; ii < upgrades.Length; ii++)
        {
            for (int jj = 0; jj < upgrades.Length; jj++)
            {
                if(ii != jj)
                {
                    if (upgrades[ii].GetType() == upgrades[jj].GetType())
                    {
                        Debug.LogError(upgrades[ii].GetType().ToString()+" exists twice");
                    }
                }
            }
        }
    }

    private static PlayerUpgrade[] GetUpgradeList()
    {
        return new PlayerUpgrade[] {
            new TestUpgradeSpeedUp(INSTANCE.spriteList[0])//,
            //,
            //,
        };
    }

    [ContextMenu("Get Number of Upgrades")]
    public void GetListSize()
    {
        Debug.Log("Upgrade list has "+ GetUpgradeList().Length+" elements");
    }
}
