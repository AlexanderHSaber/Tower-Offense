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

    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.debug)
        {
            speed = gc.gameSpeed;
        }

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
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    public void SetTarget(Vector2 newTarget) {
        target = newTarget;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>()) {
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
