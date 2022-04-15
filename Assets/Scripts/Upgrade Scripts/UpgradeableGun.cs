using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq; //required for Intersect() and Sum()

/*
 *  abstract class for guns to extend
 * 
 *  takes care of receiving / validating upgrades 
 *  and of tallying up the modifiers
 *  
 *  it doesn't actually apply the modifiers in any way though - 
 *  leaving that up to the derived classes to figure out
 *  
 *  also - no way to remove upgrades yet :O
 * 
 */

public abstract class UpgradeableGun : MonoBehaviour, IUpgradeable
{
    //upgrade types this gun can use – default to universal "for any gun" upgrades only
    public List<GunUpgrade.GunType> validUpgradeTypes = new List<GunUpgrade.GunType>() { GunUpgrade.GunType.ANY};

    //upgrades currently attached to this gun
    public List<GunUpgrade> upgrades;

    //i had these private at first, but then the subclasses couldn't see them
    protected Dictionary<GunUpgrade, float> damageModifiers;      
    protected Dictionary<GunUpgrade, float> speedModifiers;       
    protected Dictionary<GunUpgrade, float> fireRateModifiers;    
    protected Dictionary<GunUpgrade, float> projectileCountModifiers;
    protected Dictionary<GunUpgrade, float> chainCountModifiers;

    protected Dictionary<GunUpgrade, List<GunUpgrade.SpecialEffect>> specialEffects;

    

    //total modifiers for each stat
    protected int projectileCountModifier;
    protected float damageModifier;
    protected float fireRateModifier;
    protected float speedModifier;
    protected int chainCountModifier;

    protected abstract IEnumerator ShootAmmo();


    /*
    * Initialize all stat/effect dicts and empty the upgrade list before the game starts
    * 
    * call from start in derived classes
    */

    protected virtual void InitializeUpgradeState()
    {        
        damageModifiers = new Dictionary<GunUpgrade, float>();
        speedModifiers = new Dictionary<GunUpgrade, float>();
        fireRateModifiers = new Dictionary<GunUpgrade, float>();
        projectileCountModifiers = new Dictionary<GunUpgrade, float>();
        chainCountModifiers = new Dictionary<GunUpgrade, float>();

        specialEffects = new Dictionary<GunUpgrade, List<GunUpgrade.SpecialEffect>>();     

        upgrades = new List<GunUpgrade>();
        
        UpdateStatModifiers();
    }

    /*
     * check if an incoming upgrade is a GunUpgrade
     * then checks to see if it matches this type of gun
     * 
     * right now this gets called from inside of UpgradeManager.Dispatch 
     *      
     */

    public void ReceiveUpgrade(BaseUpgrade upgrade)
    {
        //attempt to access upgrade as a GunUpgrade object
        GunUpgrade gunUpgrade = upgrade as GunUpgrade;


        //not a GunUpgrade!
        if(gunUpgrade == null)
        {
            //Debug.log("invalid upgrade type; must be of type GunUpgrade");
            return;
        }

        //check for matches between GunUpgrade type and what this gun will accept
        var typeMatches = validUpgradeTypes.Intersect(gunUpgrade.validGunTypes).ToList();
        if(typeMatches.Count == 0)
        {
            //Debug.log($"invalid GunUpgrade for {gameObject.name}");
            return;
        }


        //it's a match! apply it
        //Debug.log($"Woohoo! {gameObject.name} received {gunUpgrade.GetName()}");
        ApplyUpgrade(gunUpgrade);        
    }


    /*
     * subclasses can override this if they need their own implementation 
     * 
     * or we can just add new modifiers here as they come up..?
     */

    public virtual void ApplyUpgrade(BaseUpgrade upgrade)
    {        
        GunUpgrade gunUpgrade = Instantiate(upgrade) as GunUpgrade; //clones the upgrade

        //add the clone to the upgrade list for this gun
        upgrades.Add(gunUpgrade);

        //apply any new stat modifiers from the upgrade
        if (gunUpgrade.damageModifier != 0) damageModifiers.Add(gunUpgrade, gunUpgrade.damageModifier);
        if (gunUpgrade.speedModifier != 0) speedModifiers.Add(gunUpgrade, gunUpgrade.speedModifier);
        if (gunUpgrade.fireRateModifier != 0) fireRateModifiers.Add(gunUpgrade, gunUpgrade.fireRateModifier);
        if (gunUpgrade.projectileCountModifier != 0) projectileCountModifiers.Add(gunUpgrade, gunUpgrade.projectileCountModifier);
        if (gunUpgrade.chainCountModifier != 0) chainCountModifiers.Add(gunUpgrade, gunUpgrade.chainCountModifier);

        //apply any other effects
        if (gunUpgrade.specialEffects.Count > 0)
        {
            specialEffects.Add(gunUpgrade, gunUpgrade.specialEffects);
            foreach (var effect in gunUpgrade.specialEffects)
            {
                //Debug.log($"{gameObject.name} gained special effect {effect}");
            }
        }
        
        //Debug.log($"{gameObject.name} applied {upgrade.GetName()}!");
        //Debug.log($"recalculating stat modifiers for {gameObject.name} after upgrade");

        //calculate the new stat totals and save them
        UpdateStatModifiers();
    }

    protected float CalculateModifier(Dictionary<GunUpgrade, float> modifiers)
    {         
        return modifiers.Values.Sum();
    }

    protected void UpdateStatModifiers()
    {
        damageModifier = CalculateModifier(damageModifiers);
        speedModifier = CalculateModifier(speedModifiers);
        fireRateModifier = CalculateModifier(fireRateModifiers);
        projectileCountModifier = (int)CalculateModifier(projectileCountModifiers);
        chainCountModifier = (int)CalculateModifier(chainCountModifiers);
    }

    /*
     *  RemoveUpgrade(upgrade)
     *  {
     *      remove from dictionaries
     *      remove from upgrade list
     *      update stats and effects
     *  }
     */

    public void StartShootingCoroutine()
    {
        StartCoroutine(ShootAmmo());
    }

    public void StopShootingCoroutine()
    {
        StopAllCoroutines();
    }

}