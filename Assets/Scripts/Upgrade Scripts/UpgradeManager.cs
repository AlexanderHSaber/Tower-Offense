using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;

[CreateAssetMenu(fileName ="Upgrade Table")]
public class UpgradeManager : ScriptableObject
{
    [SerializeField][Header("Any upgrades that can appear in the game should go here.")]
    private List<BaseUpgrade> upgrades; 

    // # of times each upgrade has been picked during the game
    private Dictionary<BaseUpgrade, int> selectionHistory;


    private void Initialize()
    {
        selectionHistory = new Dictionary<BaseUpgrade, int>();
        Debug.Log("upgrade table awake");
        Debug.Log($"total upgrade count: {upgrades.Count}");
        Debug.Log($"selection history count: {selectionHistory.Count}");
    }

    //start with a blank dictionary
    public void OnEnable()
    {        
        Initialize();
    }

    public List<BaseUpgrade> GetRandomUpgrades(int number, bool allowDuplicateResults = false)
    {
        var results = new List<BaseUpgrade>();  //search results

        //Linq query to select valid offerings from the full upgrade list
        var query =
            from u in upgrades
            where u.IsRepeatable() || selectionHistory.ContainsKey(u) == false //not chosen yet, or an upgrade you can get more than once
            select u;

        //save to a new list
        var possibleResults = query.ToList();
        
        //keep going until reached desired number or ran out of offerings
        while(results.Count < number && possibleResults.Count > 0)
        {
            int i = Random.Range(0, possibleResults.Count);
            var upgrade = possibleResults[i];
            results.Add(upgrade);

            //if duplicate results aren't allowed, or if this upgrade is non-repeatable, remove it from the next loop cycle
            if (allowDuplicateResults == false || upgrade.IsRepeatable() == false) possibleResults.Remove(upgrade);
        }

        return results;
    }

    //call this when the player actually chooses a new upgrade
    public void AddToSelectionHistory(BaseUpgrade chosenUpgrade)
    {
        if (selectionHistory.ContainsKey(chosenUpgrade) == false) selectionHistory.Add(chosenUpgrade, 1);
        else selectionHistory[chosenUpgrade]++;
    }


    /*
       Dispatch
       
       figure out who should get the upgrade and send it to them
       gets called from UIController when the player chooses an upgrade card
       
       eventually the upgrade could go into some kind of inventory if we get the slot idea up and working       
       for now though this just checks the upgrade type and then blasts it directly to any object that might be able to use it
       
       GunController does its own additional checking according to the specifics of each upgrade and gun
    */

    public void Dispatch(BaseUpgrade upgrade)
    {        

        var recipients = new List<IUpgradeable>();

        GunUpgrade gunUpgrade = upgrade as GunUpgrade;
        TowerUpgrade towerUpgrade = upgrade as TowerUpgrade;

        if(gunUpgrade != null)
        {
            var allTheGuns = FindObjectsOfType<UpgradeableGun>();
            recipients.AddRange(allTheGuns);
        }

        if(towerUpgrade != null)
        {
            var tower = FindObjectOfType<TowerController>();
            recipients.Add(tower);
        }        

        foreach(IUpgradeable recipient in recipients)
        {
            recipient.ReceiveUpgrade(upgrade);
        }
    }
}
