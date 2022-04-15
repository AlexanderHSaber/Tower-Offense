using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowGunController : UpgradeableGun 
{ 

    public GameController gc;

    public GameObject projectilePrefab;

    public float chainSpeed = 0.005f;

    public float radius;

    [SerializeField]
    private float baseFireRate = 1f;
    [SerializeField]
    private float baseDamage = 0.5f;

    public bool shooting = true;

    public float minRange = 10f;
    public float maxRange = 15f;



    //getter properties for effective stat values
    public float FireRate => baseFireRate + fireRateModifier;
    public float Damage => baseDamage + damageModifier;

    // Start is called before the first frame update
    void Start()
    {
        InitializeUpgradeState();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        StartCoroutine("CheckEnemies");
        StartCoroutine("ShootAmmo");

    }

    private void Update()
    {

        
    }

    private bool EnemiesPresent() 
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        return enemies.Length > 0;
    }


    protected override IEnumerator ShootAmmo()
    {
        // Spawn lightning between tower and closest enemy unit.
        while (true)
        {
            float bulletInterval = 1 / FireRate;
            if (gc.debug) bulletInterval /= gc.gameSpeed;

            float distance = Random.Range(minRange, maxRange);

            Vector2 direction = Random.insideUnitCircle;
            Quaternion rotation = Quaternion.FromToRotation(Vector2.right, direction);

            GameObject slowProjectile = Instantiate(projectilePrefab, transform.position, rotation);
            slowProjectile.GetComponent<SlowInitialProjectileController>().setRange(distance);

            yield return new WaitForSeconds(bulletInterval);
        }
    }
}

