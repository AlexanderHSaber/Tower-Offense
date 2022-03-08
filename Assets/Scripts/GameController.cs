using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public float spawnTime;

    public float radius = 15f;
    public GameObject enemyPrefab;

    public float gameSpeed;
    public bool debug;

    public int currentWave;
    public int remainingSpawnCount;

    private UIDocument uidoc;
    private VisualElement root;
    private Button nextWaveButton;

    public bool readyForNextWave = true;

    // Start is called before the first frame update
    void Start()
    {

        currentWave = 0;
        remainingSpawnCount = getWaveQuantity(currentWave);
        StartCoroutine(SpawnEnemies());

        

        uidoc = GameObject.FindObjectOfType<UIDocument>();

        root = uidoc.rootVisualElement;
        nextWaveButton = root.Q<Button>("button-next");

        nextWaveButton.clicked += () => readyForNextWave = true;
        HideUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (debug) {
            spawnTime = 1 / gameSpeed;
        }
    }

    IEnumerator SpawnEnemies()
    {
        
        Debug.Log(currentWave);
        Debug.Log(remainingSpawnCount);

        while (remainingSpawnCount > 0)
        {
            yield return new WaitForSeconds(spawnTime);
            Vector2 spawnPoint = Random.insideUnitCircle.normalized * radius;

            SpawnEnemy(spawnPoint);
            remainingSpawnCount -= 1;
        }

        readyForNextWave = false;

        yield return new WaitUntil(() => GameObject.FindGameObjectsWithTag("Enemy").Length == 0); //check for all enemies destroyed

        PauseGame();
        
        yield return new WaitUntil(() => readyForNextWave); //next wave button clicked

        UnpauseGame();

        currentWave += 1;
        remainingSpawnCount = getWaveQuantity(currentWave);
        
        StartCoroutine(SpawnEnemies());
    }

    void SpawnEnemy(Vector2 position)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }

    int getWaveQuantity(int n) {
        return n + 1;
    }

    void ShowUI()
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
        for(int i = 0; i < cards.Count; i++)
        {
            string cardDescription;
            CardTexts.TryGetValue(possibleChoices[i], out cardDescription);
            cards[i].text = cardDescription;

        }    

        root.style.display = DisplayStyle.Flex;
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
    void HideUI() 
    {
        root.style.display = DisplayStyle.None;
    }

    void PauseGame() 
    {

        DestroyAllProjectiles();
        ShowUI();

    }

    void UnpauseGame() 
    {
        HideUI();
    }

    void DestroyAllProjectiles() 
    {
        // Stop the movement of gameobjects in the scene
        ProjectileController[] projectileControllers = GameObject.FindObjectsOfType<ProjectileController>();
        foreach (ProjectileController a in projectileControllers)
        {
            a.DestroyProjectile();
        }
    }
}
