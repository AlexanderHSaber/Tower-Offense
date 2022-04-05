using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UpgradeableTower : MonoBehaviour, IUpgradeable
{
    public List<TowerUpgrade> upgrades;

    protected Dictionary<TowerUpgrade, float> healthModifiers;
    protected Dictionary<TowerUpgrade, float> shieldModifiers;

    //initialize dictionaries and upgrade list; call this on start in TowerController
    protected void InitializeUpgradeState()
    {
        
        healthModifiers = new Dictionary<TowerUpgrade, float>();
        shieldModifiers = new Dictionary<TowerUpgrade, float>();

        upgrades = new List<TowerUpgrade>();
    }

    public void ReceiveUpgrade(BaseUpgrade upgrade)
    {
        TowerUpgrade towerUpgrade = upgrade as TowerUpgrade;

        if (towerUpgrade == null)
        {
            //Debug.log($"invalid upgrade! {upgrade.GetName()} is not a TowerUpgrade");
            return;
        }


        //Debug.log($"Yay! The tower received {towerUpgrade.GetName()}");
        ApplyUpgrade(towerUpgrade);
    }

    public void ApplyUpgrade(BaseUpgrade upgrade)
    {
        TowerUpgrade towerUpgrade = Instantiate(upgrade) as TowerUpgrade;   //save a copy of the upgrade to the upgrade list
                                                                            
        upgrades.Add(towerUpgrade);

        if (towerUpgrade.healthModifier != 0) healthModifiers.Add(towerUpgrade, towerUpgrade.healthModifier);
        if (towerUpgrade.shieldModifier != 0) shieldModifiers.Add(towerUpgrade, towerUpgrade.shieldModifier);

        //Debug.log($"The tower applied {towerUpgrade.GetName()}. It feels *much* better now.");
    }
}
