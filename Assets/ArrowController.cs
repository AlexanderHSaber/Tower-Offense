using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{

    public float speed;
    public Vector2 target;
    public float stoppingDistance;
    public int damage = 1;

    void Start()
    {
        speed = 1.0f;
        stoppingDistance = 0.1f;
    }

    // Update is called once per frame
    void Update()
    {
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

}
