using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    public GameObject arrowPrefab;

    public float radius;
    public float bulletInterval = 1f;
    public bool shooting = true;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ShootAmmo");
    }

    // Update is called once per frame
    void Update()
    {


    }

    void spawnAmmo(Vector2 target) {
        GameObject ammo = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        ammo.GetComponent<ArrowController>().SetTarget(target);
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
        Debug.Log(enemies.Length);
        if (enemies.Length > 0)
        {
            return enemies[0].transform.gameObject;
        }
        return null; 
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
}
