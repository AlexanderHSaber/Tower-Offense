using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGunController : UpgradeableGun
{
    public GameController gc;

    private GameObject targetedEnemy;
    public GameObject crosshair;
    public GameObject projectilePrefab;

    public float radius;
    public float baseFireRate = 2;
    
    public bool shooting = true;

    private int baseProjectileCount = 1;

    //degrees to rotate each additional projectile by
    private float perProjectileArc = 5;

    //max degrees deviation from target angle (to either side)
    private float maxDeviation = 2.5f;

    //get effective stat values after upgrades
    public float FireRate => baseFireRate + fireRateModifier;
    public int ProjectileCount => baseProjectileCount + projectileCountModifier;

    void Start()
    {
        InitializeUpgradeState();
        projectileCountModifiers = new Dictionary<GunUpgrade, float>();

        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();        

        StartCoroutine("ShootAmmo");

    }

    // Update is called once per frame
    void Update()
    {
        
        //rotate gun towards target
        if (targetedEnemy)
        {
            float degreesPerSec = 1800f;
            Quaternion atTarget = Quaternion.FromToRotation(Vector3.right, targetedEnemy.transform.position - transform.position);
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, atTarget, degreesPerSec * Time.deltaTime);
        }

        if (crosshair)
        {
            UpdateCrosshair();
        }



        
    }

    GameObject spawnAmmo(Vector2 target)
    {
        GameObject ammo = Instantiate(projectilePrefab, transform.position, Quaternion.identity);


        ammo.GetComponent<IProjectileType>().SetTarget(target);



        return ammo;

    }

    private void UpdateCrosshair()
    {
        if (targetedEnemy)
        {
            crosshair.SetActive(true);
            crosshair.transform.rotation = Quaternion.identity;
            crosshair.transform.position = targetedEnemy.transform.position;
        }

        else
        {
            crosshair.SetActive(false);
        }    
    }

    GameObject GetTarget()
    {
        radius = 20.0f;

        RaycastHit2D[] enemies = Physics2D.CircleCastAll(
            transform.position,
            radius,
            Vector2.zero,
            0.0f,
            LayerMask.GetMask("Enemy"));

        if (enemies.Length > 0)
        {
            return getClosestEnemy(enemies);
        }
        return null;
    }

    GameObject getClosestEnemy(RaycastHit2D[] enemies)
    {
        GameObject closestSoFar = null;

        foreach (RaycastHit2D hit in enemies)
        {
            if (closestSoFar == null)
            {
                closestSoFar = hit.transform.gameObject;
            }
            else if (Vector2.SqrMagnitude(transform.position - hit.transform.position) < Vector2.SqrMagnitude(transform.position - closestSoFar.transform.position))
            {
                closestSoFar = hit.transform.gameObject;
            }
        }

        return closestSoFar;
    }

    IEnumerator ShootAmmo()
    {
        while (shooting)
        {
            GameObject target = GetTarget();

            float bulletInterval = 1 / FireRate;
            if (gc.debug) bulletInterval /= gc.gameSpeed;

            if (target)
            {

                float startAngle;
                if (ProjectileCount == 1) startAngle = 0;

                else startAngle = ProjectileCount % 2 == 0 ? (perProjectileArc * ProjectileCount / 2) - perProjectileArc/2 : (perProjectileArc * ProjectileCount) / 2 - perProjectileArc / 2;

                for (int p = 0; p < ProjectileCount; p++)
                {
                    GameObject bullet = spawnAmmo(target.transform.position);

                    float adjustment = startAngle - p * perProjectileArc + Random.Range(-maxDeviation, maxDeviation);

                    bullet.GetComponent<ProjectileController>().AdjustHeading(adjustment);
                }

                

                targetedEnemy = target;
            }

            yield return new WaitForSeconds(bulletInterval);
        }


    }    

    
}
