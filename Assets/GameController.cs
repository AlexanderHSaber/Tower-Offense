using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public float spawnTime = 2f;

    public float radius = 15f;
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);

            Vector2 spawnPoint = Random.insideUnitCircle.normalized * radius;
            SpawnEnemy(spawnPoint);
        }
    }
    
    void SpawnEnemy(Vector2 position)
    {
        Instantiate(enemyPrefab, position, Quaternion.identity);
    }
}
