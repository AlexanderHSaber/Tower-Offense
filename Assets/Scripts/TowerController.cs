using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{

    public GameObject projectilePrefab;
    public GameController gc;

    public float radius;
    public float firingRate = 2;
    private float bulletInterval;
    public bool shooting = true;

    int health = 5;

    private GameObject targetedEnemy;

    public GameObject gun;
    private Material material;    
    private Coroutine flashRoutine;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("ShootAmmo");

        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        
        material = GetComponent<SpriteRenderer>().sharedMaterial; // project-wide reference to this material type; changes will show in all gameobjects using it
        material.SetFloat("_EffectStrength", 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.debug)
        {
            bulletInterval = 1/gc.gameSpeed;            
        }

        bulletInterval = gc.debug ? 1 / (firingRate * gc.gameSpeed) : 1 / firingRate;

        //rotate gun towards target
        if (targetedEnemy)
        {
            float degreesPerSec = 1800f;
            Quaternion atTarget = Quaternion.FromToRotation(Vector3.right, targetedEnemy.transform.position - transform.position);
            gun.transform.rotation = Quaternion.RotateTowards(gun.transform.rotation, atTarget, degreesPerSec * Time.deltaTime);
        }
    }

    void spawnAmmo(Vector2 target) {
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

                targetedEnemy = target;
            }

            yield return new WaitForSeconds(delay);
        }
        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("HELO");
        if (collision.gameObject.GetComponent<EnemyController>())
        {
            if (flashRoutine != null) StopCoroutine(flashRoutine);
            flashRoutine = StartCoroutine(Flash(0.2f, 0.1f));

            Debug.Log("HELO2");
            health -= collision.gameObject.GetComponent<EnemyController>().damage;

            if (health <= 0) {
                Debug.Log("DEAD");
            }
        }
    }

    private IEnumerator Flash(float rampTime, float holdTime = 0)
    {
        if(holdTime > 0)
        {
            material.SetFloat("_EffectStrength", 1);
            yield return new WaitForSeconds(holdTime);
        }

        float startTime = Time.time;

        while(Time.time - startTime < rampTime)
        {
            float t = (Time.time - startTime) / rampTime;
            float strength = Mathf.Lerp(1, 0, t);
            material.SetFloat("_EffectStrength", strength);            
            yield return null;
        }

        material.SetFloat("_EffectStrength", 0);
    }
}
