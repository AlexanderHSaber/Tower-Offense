using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * a class for making new TowerUpgrade assets
 * 
 * add new upgrades to the UpgradeManager's list for them to show up in-game
 */

[CreateAssetMenu(fileName = "T UpgradeName", menuName = "Upgrade/Tower Upgrade")]
public class TowerUpgrade : BaseUpgrade
{

    [Header("STAT EFFECTS")]
    public float healthModifier = 0;
    public float shieldModifier = 0;
    
    /*
     * probably more interesting ones could go here too
     * 
     * ...
     * ...
     * ...
     * 
     */

    
    public enum SpecialEffect { COOL_STUFF, WEIRD_STUFF }

    [Space(20)]
    public List<SpecialEffect> specialEffects;
}
