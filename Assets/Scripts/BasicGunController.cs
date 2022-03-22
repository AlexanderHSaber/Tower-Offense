using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicGunController : MonoBehaviour
{
    public GameController gc;

    private GameObject targetedEnemy;
    public GameObject projectilePrefab;

    private Material material;

    public float radius;
    public float firingRate = 2;
    private float bulletInterval;
    public bool shooting = true;

    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        material = GetComponent<SpriteRenderer>().sharedMaterial; // project-wide reference to this material type; changes will show in all gameobjects using it
        material.SetFloat("_EffectStrength", 0);

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


        if (gc.debug)
        {
            bulletInterval = 1 / gc.gameSpeed;
        }

        bulletInterval = gc.debug ? 1 / (firingRate * gc.gameSpeed) : 1 / firingRate;
    }

    void spawnAmmo(Vector2 target)
    {
        GameObject ammo = Instantiate(projectilePrefab, transform.position, Quaternion.identity);


        ammo.GetComponent<IProjectileType>().SetTarget(target);

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

            float delay = 0.05f;

            if (target)
            {
                spawnAmmo(target.transform.position);
                delay = bulletInterval;

                targetedEnemy = target;
            }

            yield return new WaitForSeconds(delay);
        }


    }
}
