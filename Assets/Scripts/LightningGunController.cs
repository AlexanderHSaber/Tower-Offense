using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningGunController : UpgradeableGun
{
    public GameController gc;

    private GameObject targetedEnemy;
    public GameObject projectilePrefab;

    public float chainSpeed = 0.005f;

    public float radius;

    [SerializeField]
    private float baseFireRate = 1f;
    [SerializeField]
    private float baseDamage = 0.5f;

    public bool shooting = true;

    

    //getter properties for effective stat values
    public float FireRate => baseFireRate + fireRateModifier;
    public float Damage => baseDamage + damageModifier;

    // Start is called before the first frame update
    void Start()
    {
        InitializeUpgradeState();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        StartCoroutine("ShootAmmo");

    }


    IEnumerator startChainLightning(GameObject target, Vector2 currentLocation)
    {

        List<GameObject> enemiesAlreadyHit = new List<GameObject>();

        int maxChain = 7;
        while (maxChain >= 0) 
        {
            Vector2 dir = (Vector2)target.transform.position - currentLocation;

            // Get midpoint between location and target
            Vector2 midpoint = currentLocation + (dir) / 2;

            // Get quaternion between location and target
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.Euler(0f, 0f, angle + 90.0f);

            // Instantiate lightning.
            GameObject lightning = Instantiate(projectilePrefab, midpoint, q);
            SpriteRenderer lightningSR = lightning.GetComponent<SpriteRenderer>();

            lightningSR.size = new Vector2(4.0f, Vector2.Distance(currentLocation, (Vector2)target.transform.position));

            EnemyController EC = target.gameObject.GetComponent<EnemyController>();
            if (EC) 
            {
                EC.TakeDamage(Damage);
            }

            float randomizer = Random.Range(-0.001f, 0.001f);
            yield return new WaitForSeconds(chainSpeed + randomizer);

            if (!target)
                break;

            // Get the location of the next target.
            currentLocation = new Vector2(target.transform.position.x, target.transform.position.y);
            target = GetRandomTargetInRange(50.0f, target.transform.position, target);

            if (!target)
                break;

            maxChain -= 1;

        }
        
    }

    GameObject GetRandomTargetInRange(float range, Vector2 startLocation, GameObject toExclude = null)
    {
        RaycastHit2D[] enemies = Physics2D.CircleCastAll(
            startLocation,
            range,
            Vector2.zero,
            0.0f,
            LayerMask.GetMask("Enemy"));

        if (enemies.Length > 0)
        {
            return enemies[Random.Range(0, enemies.Length)].transform.gameObject;
        }
        return null;
    }

    IEnumerator ShootAmmo()
    {
        // Spawn lightning between tower and closest enemy unit.
        GameObject target = null;

        while (shooting)
        {
            float bulletInterval = 1 / FireRate;
            if (gc.debug) bulletInterval /= gc.gameSpeed;            

            target = GetRandomTargetInRange(50.0f, transform.position);

            if (target)
            {
                StartCoroutine(startChainLightning(target, transform.position));
            }

            yield return new WaitForSeconds(bulletInterval);
        }
    }
}
