using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * a class for making new GunUpgrade assets
 * 
 * add new upgrades to the UpgradeManager's list for them to show up in-game
 */

[CreateAssetMenu(fileName = "G UpgradeName", menuName = "Upgrade/Gun Upgrade")]
public class GunUpgrade : BaseUpgrade
{
    
    public enum GunType { ANY, BASIC, LIGHTNING, PULSE, SLOW}; //enum of gun type(s) that can use this upgrade

    public List<GunType> validGunTypes = new List<GunType>() { GunType.ANY};  // which guns can receive this upgrade? default to any gun

    [Header("STAT EFFECTS")]
    public float damageModifier;
    public float fireRateModifier;
    public float speedModifier;
    public float projectileCountModifier;
    public float chainCountModifier;
    /*
     * probably more interesting ones could go here too
     * 
     * bullet spread
     * 
     * enemy knockback
     * 
     * critical effects
     * 
     * gun coolness??
     * 
     * piercing
     * 
     * # of bounces
     * 
     * some kind of overheat mechanic
     * 
     * projectile size
     * 
     * target-seeking
     */


    /*
     * other effects that don't make sense to pass in as numerical stats
     * 
     * or is it better to keep everything a float?
     */
    public enum SpecialEffect {COOL_STUFF, WEIRD_STUFF}

    [Space(20)]
    public List<SpecialEffect> specialEffects;
                    
                    
}
