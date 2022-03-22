using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public float spawnTime;

    public float radius = 15f;
    public List<GameObject> enemyPrefabs;

    public float gameSpeed;
    public bool debug;

    public int currentWave;
    public int remainingSpawnCount;


    private VisualElement root;
    private Button nextWaveButton;

    public bool readyForNextWave = true;

    public UIController uiController;

    // Start is called before the first frame update
    void Start()
    {

        // currentWave = 0;
        remainingSpawnCount = getWaveQuantity(currentWave);
        StartCoroutine(SpawnEnemies());

        uiController.HideUI();
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
        
        //Debug.Log(currentWave);
        //Debug.Log(remainingSpawnCount);

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
        int enemyType = Random.Range(0, enemyPrefabs.Count);
        Instantiate(enemyPrefabs[enemyType], position, Quaternion.identity);
    }

    int getWaveQuantity(int n)
    {
        return n + 1;
    }

    void PauseGame() 
    {

        DestroyAllProjectiles();
        uiController.ShowUI();

    }

    void UnpauseGame() 
    {
        uiController.HideUI();
    }

    void DestroyAllProjectiles() 
    {
        // Stop the movement of gameobjects in the scene
        GameObject[] projectileObjects = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject projectile in projectileObjects)
        {
            Destroy(projectile);
        }
    }

    public void setReadyForNextWave() 
    {
        readyForNextWave = true;
    }
}
