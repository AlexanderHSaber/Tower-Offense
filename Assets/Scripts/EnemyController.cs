using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject tower;
    public float speed;
    public Vector2 direction;
    public float stoppingDistance;
    public float health = 3;

    public int damage = 1;

    GameController gc;

    // Start is called before the first frame update
    void Start()
    {

        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        

        if (!tower) tower = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (gc.debug)
        {
            speed = gc.gameSpeed;
        }
        this.MoveTowardsTower();
    }

    private void MoveTowardsTower() {

        // Check distance from tower
        if (Vector2.Distance(tower.transform.position, this.transform.position) >= stoppingDistance)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, tower.transform.position, speed * Time.deltaTime);
        }
        else Destroy(gameObject);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ProjectileController projectile = collision.gameObject.GetComponent<ProjectileController>();
        
        if (projectile)
        {
            health -= projectile.GetDamage();
            if(health <= 0)
            {
                Destroy(gameObject);
            }
        }

        TowerController tower = collision.gameObject.GetComponent<TowerController>();
        if (tower)
        {
            Destroy(gameObject);
        }
    }
}
