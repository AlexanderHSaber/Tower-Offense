using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{

    private UIDocument uidoc;
    private VisualElement root;
    private Button nextWaveButton;
    private VisualElement CardSelectionScreen;
    private VisualElement UpgradeScreen;

    public GameController gc;
    public UpgradeManager upgradeManager;
    
    //save a list of cleanup actions to run after an upgrade is selected
    //for unregistering callbacks before the next upgrade cycle
    private List<System.Action> cleanupActions;
    

    // Start is called before the first frame update
    void Start()
    {
        // Initialize UI Objects
        uidoc = gameObject.GetComponent<UIDocument>();
        root = uidoc.rootVisualElement;
        nextWaveButton = root.Q<Button>("button-next");
        CardSelectionScreen = root.Q("CardScreen");
        UpgradeScreen = root.Q("UpgradeScreen");

        InitializeWeaponTabs();

        nextWaveButton.clicked += () => gc.setReadyForNextWave();
        
    }

    void Shuffle(int[] list)
    {
        for (int i = 0; i < list.Length - 1; i++)
        {
            int rnd = Random.Range(i, list.Length);
            int temp = list[rnd];
            list[rnd] = list[i];
            list[i] = temp;
        }
    }

    public void HideUI()
    {
        toggleUIRoot();
    }

    public void ShowUI()
    {

        var cards = new List<Button>();
        root.Query<Button>(className: "Card").ToList(cards);


        var upgradeOptions = upgradeManager.GetRandomUpgrades(cards.Count);


        cleanupActions = new List<System.Action>();

        for (int i = 0; i < cards.Count; i++)
        {
            Button card = cards[i];

            if (upgradeOptions.Count < i + 1)
            {
                //no upgrade available for this card — hide it
                //Debug.log($"no upgrade option available for card {i}; hiding card {i} instead");
                card.style.display = DisplayStyle.None;
            }
            else
            {
                BaseUpgrade upgrade = upgradeOptions[i];
                                                
                card.text =  upgrade.GetName() + "\n\n" + upgrade.GetDescription();
                card.style.display = DisplayStyle.Flex;

                //register a callback to select this upgrade
                EventCallback<ClickEvent> selectUpgrade = (evt) => MakeSelection(upgrade);
                card.RegisterCallback(selectUpgrade);

                //add a function that unregisters this callback
                //to the list of cleanup actions that will get called when a selection is made
                System.Action removeButtonCallback = () => card.UnregisterCallback(selectUpgrade);
                cleanupActions.Add(removeButtonCallback);
                
            }
            
        }

        toggleUIRoot();
        switchScreen(CardSelectionScreen, UpgradeScreen);
    }
    private void toggleUIRoot() 
    {
        root.style.display = root.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
    }
    private void switchScreen(VisualElement toShow, VisualElement toHide) 
    {
        if (toHide != null)
            toHide.style.display = DisplayStyle.None;
        if (toShow != null)
            toShow.style.display = DisplayStyle.Flex;
    }

    private void showScreen(VisualElement ve)
    {
        if (ve != null)
            ve.style.display = DisplayStyle.Flex;
    }

    private void hideScreen(VisualElement ve)
    {
        if (ve != null)
            ve.style.display = DisplayStyle.None;
    }

    private void MakeSelection(BaseUpgrade selection)
    {
        //Debug.log($"UIController.SelectUpgrade : clicked {selection.GetName()}!");

        //tell the system which upgrade got picked
        upgradeManager.AddToSelectionHistory(selection);

        //and have it pass the upgrade along to whoever should get it next
        upgradeManager.Dispatch(selection);
        
        CleanupCallbacks();

        switchScreen(UpgradeScreen, CardSelectionScreen);
    }

    private void CleanupCallbacks()
    {
        foreach(var cleanupFunction in cleanupActions)
        {
            cleanupFunction();
        }

        cleanupActions.Clear();
    }
   

    private void InitializeWeaponTabs() 
    {
        var weaponIcons = new List<VisualElement>();
        root.Query<VisualElement>(className: "weaponIcon").ToList(weaponIcons);

        var weaponUpgradeScreens = new List<VisualElement>();
        root.Query<VisualElement>(className: "weaponUpgradeScreen").ToList(weaponUpgradeScreens);

        for (int i = 0; i < weaponIcons.Count; i++)
        {
            ////Debug.log($"attached callback to weaponIcon{i}");
            VisualElement weapon = weaponIcons[i];

            int curr = i; //read i outside of callback to get actual value
            weapon.RegisterCallback<ClickEvent>(evt =>
            {
                ////Debug.log(i); //not sure why this is always 4 inside the callback?

                ////Debug.log("Clicked weapon icon" + curr);
                VisualElement weaponUpgradeScreen = weaponUpgradeScreens[curr];
                showWeaponUpgradeScreen(weaponUpgradeScreen, weaponUpgradeScreens);
            });
        }

        showWeaponUpgradeScreen(weaponUpgradeScreens[0], weaponUpgradeScreens);
    }

    private void showWeaponUpgradeScreen(VisualElement weaponUpgradeScreen, List<VisualElement> weaponUpgradeScreens) 
    {
        for (int i = 0; i < weaponUpgradeScreens.Count; i++)
        {
            VisualElement currScreen = weaponUpgradeScreens[i];
            hideScreen(currScreen);
        }
        showScreen(weaponUpgradeScreen);
    }
}
