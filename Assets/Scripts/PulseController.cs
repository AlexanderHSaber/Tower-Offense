using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//NOTE: currently works by scaling the GameObject
//      prefab sprite, collider, etc. should all be 1 unit in size (or 0.5 radius) in order to work


public class PulseController : MonoBehaviour, IProjectileType
{

    private float maxRadius; //max size of pulse ; atm calculated by camera half diagonal - change later when we know how big the screen is
    private float radius;

    [SerializeField] private float speed = 8; //growth rate in units / sec
    [SerializeField] private float size = 1.05f; //radius multiplier
    [SerializeField] private float damage = 0.5f;
    [SerializeField] private GameObject hitParticles;   //particle effect for enemy hit

    // Start is called before the first frame update
    void Start()
    {        

        float screenHalfHeight = Camera.main.orthographicSize;
        float screenHalfWidth = screenHalfHeight * Camera.main.aspect;
        maxRadius = Mathf.Sqrt(Mathf.Pow(screenHalfWidth, 2) + Mathf.Pow(screenHalfHeight, 2)) * size; //calculate max pulse radius

        //Debug.Log($"max pulse radius: {maxRadius}");

        StartCoroutine(Fire());
    }


    //starts the pulse
    private IEnumerator Fire()
    {
        radius = 0;

        //grow over time
        while (radius < maxRadius)
        {
            radius += speed * Time.deltaTime;

            //scale by diameter
            transform.localScale = Vector3.one * radius * 2;

            yield return null; ;
        }
        
        Destroy(gameObject);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //spawn particles on enemy hit
        if (collision.gameObject.GetComponent<EnemyController>() && hitParticles)
        {
            var particles = Instantiate(hitParticles);
            particles.transform.position = collision.transform.position;
        }
    }

    public void SetTarget(Vector2 target) { }

    public float GetDamage()
    {
        return damage;
    }


}
