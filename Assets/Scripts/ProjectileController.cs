using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour, IProjectileType
{

    public float speed = 1;
    //public Vector2 target;


    public float damage = 2;
    GameController gc;

    public float range;

    private Vector3 lastPosition;

    private float eSpeed;   // effective speed after taking GC speed into account
    public GameObject particles;

    private float sqDistTraveled;
       
        
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        sqDistTraveled = 0;
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * eSpeed * Time.deltaTime, Space.Self);

        sqDistTraveled += Vector2.SqrMagnitude(transform.position) - Vector2.SqrMagnitude(lastPosition);
        lastPosition = transform.position;

        Debug.Log(sqDistTraveled);
        if(sqDistTraveled > range * range)
        {
            DestroyProjectile();
        }

        eSpeed = gc.debug ? speed * gc.gameSpeed : speed;

        //if (target != null) {
        //    MoveTowardsTarget();
        //}
        
        //destroy bullet when target reached
        //if(Vector2.Distance(transform.position, target) <= 0)
        //{
        //    Destroy(gameObject);
        //}
        
    }

    //private void MoveTowardsTarget()
    //{
    //    transform.position = Vector2.MoveTowards(transform.position, target, eSpeed * Time.deltaTime);
    //}

    public void SetTarget(Vector2 newTarget) {
        //target = newTarget;

        Quaternion towardsTarget = Quaternion.FromToRotation(Vector2.right, newTarget - (Vector2)transform.position);
        transform.rotation = towardsTarget;        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<EnemyController>()) {

            if (particles)
            {
                GameObject hitParticles = Instantiate(particles);
                hitParticles.transform.position = transform.position;
            }


            DestroyProjectile();
        }
    }

    public float GetDamage()
    {
        return damage;
    }

    public void DestroyProjectile()
    {
        
        Destroy(gameObject);
    }

    public void AdjustHeading(float degrees)
    {
        transform.Rotate(Vector3.forward * degrees);
    }
}
