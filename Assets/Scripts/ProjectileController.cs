using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    public float speed;
    public Vector2 target;
    public float stoppingDistance;
    public int damage = 2;
    GameController gc;


    private float eSpeed;   // effective speed after taking GC speed into account
    public GameObject particles;
        
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {

        eSpeed = gc.debug ? speed * gc.gameSpeed : speed;

        if (target != null) {
            MoveTowardsTarget();
        }
        
        //destroy bullet when target reached
        if(Vector2.Distance(transform.position, target) <= 0)
        {
            Destroy(gameObject);
        }
        
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector2.MoveTowards(transform.position, target, eSpeed * Time.deltaTime);
    }

    public void SetTarget(Vector2 newTarget) {
        target = newTarget;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>()) {

            if (particles)
            {
                GameObject hitParticles = Instantiate(particles);
                hitParticles.transform.position = transform.position;

            }
                

            Destroy(gameObject);
        }
    }

    public int GetDamage()
    {
        return damage;
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
