using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulseGunController : UpgradeableGun
{
    public GameController gc;

    public GameObject projectilePrefab;    

    [SerializeField]
    private float radius = 20;
    [SerializeField]
    private float baseFireRate = 2;
    public bool shooting = true;

    public float FireRate => baseFireRate + fireRateModifier;

    // Start is called before the first frame update
    void Start()
    {
        InitializeUpgradeState();
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        StartCoroutine("ShootAmmo");

    }


    void spawnAmmo(Vector2 target)
    {
        GameObject ammo = Instantiate(projectilePrefab, transform.position, Quaternion.identity);


        ammo.GetComponent<IProjectileType>().SetTarget(target);

    }

    GameObject GetTarget()
    {       

        RaycastHit2D enemy = Physics2D.CircleCast(
            transform.position,
            radius,
            Vector2.zero,
            0.0f,
            LayerMask.GetMask("Enemy"));

        if (enemy) return enemy.collider.gameObject;
        return null;
    }


    protected override IEnumerator ShootAmmo()
    {
        while (shooting)
        {
            GameObject target = GetTarget();

            float bulletInterval = 1/FireRate;
            if (gc.debug) bulletInterval /= gc.gameSpeed;

            if (target)
            {
                spawnAmmo(target.transform.position);
            }

            yield return new WaitForSeconds(bulletInterval);
        }


    }
}
