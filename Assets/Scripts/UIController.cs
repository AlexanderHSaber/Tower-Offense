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

    // Update is called once per frame
    void Update()
    {
        
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
        var CardTexts = new Dictionary<int, string>() {
            { 0, "Example 1" },
            { 1, "Example 2" },
            { 2, "Example 3" }
        };

        var possibleChoices = new int[] { 0, 1, 2 };
        Shuffle(possibleChoices);


        var cards = new List<Button>();
        root.Query<Button>(className: "Card").ToList(cards);
        for (int i = 0; i < cards.Count; i++)
        {
            // Display the text on each card
            string cardDescription;
            CardTexts.TryGetValue(possibleChoices[i], out cardDescription);
            cards[i].text = cardDescription;

            // Set the on click method to switch to the new Upgrade Screen and TODO: DO THE UPGRADE
            Button card = cards[i];
            cards[i].clicked += () => cardSelected(card);

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



    private void cardSelected(Button card) 
    {
        switchScreen(UpgradeScreen, CardSelectionScreen);
    }

    private void InitializeWeaponTabs() 
    {
        var weaponIcons = new List<VisualElement>();
        root.Query<VisualElement>(className: "weaponIcon").ToList(weaponIcons);

        //Debug.Log(weaponIcons.Count);

        var weaponUpgradeScreens = new List<VisualElement>();
        root.Query<VisualElement>(className: "weaponUpgradeScreen").ToList(weaponUpgradeScreens);

        for (int i = 0; i < weaponIcons.Count; i++)
        {
            //Debug.Log($"attached callback to weaponIcon{i}");
            VisualElement weapon = weaponIcons[i];

            int curr = i; //read i outside of callback to get actual value
            weapon.RegisterCallback<ClickEvent>(evt =>
            {
                //Debug.Log(i); //not sure why this is always 4 inside the callback?

                //Debug.Log("Clicked weapon icon" + curr);
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
