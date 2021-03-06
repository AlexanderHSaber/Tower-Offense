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


    IEnumerator SpawnEnemies()
    {
        
        ////Debug.log(currentWave);
        ////Debug.log(remainingSpawnCount);

        while (remainingSpawnCount > 0)
        {
            float spawnDelay = debug ? spawnTime / gameSpeed : spawnTime;

            yield return new WaitForSeconds(spawnDelay);
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
        StopAllGunControllers();
        uiController.ShowUI();

    }

    void UnpauseGame() 
    {
        uiController.HideUI();
        StartAllGunControllers();
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

    private void StopAllGunControllers() 
    {
        UpgradeableGun[] guns = GameObject.FindObjectsOfType<UpgradeableGun>();
        foreach (UpgradeableGun gun in guns) 
        {
            gun.StopShootingCoroutine();
        }
    }

    private void StartAllGunControllers()
    {
        UpgradeableGun[] guns = GameObject.FindObjectsOfType<UpgradeableGun>();
        foreach (UpgradeableGun gun in guns)
        {
            gun.StartShootingCoroutine();
        }
    }
}
