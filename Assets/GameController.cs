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
        ShowUI(false);
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

        ShowUI(true);

        yield return new WaitUntil(() => readyForNextWave); //next wave button clicked
        
        ShowUI(false);


        

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

    void ShowUI(bool visible)
    {
        root.style.display = visible ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
