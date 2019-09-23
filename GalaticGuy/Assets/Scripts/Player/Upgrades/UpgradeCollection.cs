using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UpgradeCollection : MonoBehaviour
{
    PlayerUpgrade[] upgrades;

    public void OnEnable()
    {
        upgrades = new PlayerUpgrade[] { new TestUpgrade() };

        //verify upgrades are unique
        if (Application.isEditor)
        {
            AreUnique();
        }
    }

    public PlayerUpgrade GetUpgrade(int index)
    {
        return upgrades[index];
    }

    public int GetSize()
    {
        return upgrades.Length;
    }

    private void AreUnique()
    {
        foreach(PlayerUpgrade upgradeA in upgrades)
        {
            foreach(PlayerUpgrade upgradeB in upgrades)
            {
                if(upgradeA != upgradeB)
                {
                    if (upgradeA.GetType() == upgradeB.GetType())
                    {
                        Debug.LogError(upgradeA.GetType().ToString()+" exists twice");
                    }
                }
            }
        }
    }

}
