using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    public GameObject projectilePrefab;
    public GameController gc;

    public float radius;
    public float bulletInterval;
    public bool shooting = true;

    int health = 5;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ShootAmmo");

        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

    }

    // Update is called once per frame
    void Update()
    {
        if (gc.debug)
        {
            bulletInterval = 1/gc.gameSpeed;
        }

    }

    void spawnAmmo(Vector2 target) {
        GameObject ammo = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        ammo.GetComponent<ProjectileController>().SetTarget(target);
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

    GameObject getClosestEnemy(RaycastHit2D[] enemies) {
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
            }

            yield return new WaitForSeconds(delay);
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HELO");
        if (collision.gameObject.GetComponent<EnemyController>())
        {
            Debug.Log("HELO2");
            health -= collision.gameObject.GetComponent<EnemyController>().damage;

            if (health <= 0) {
                Debug.Log("DEAD");
            }
        }
    }
}
